using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace FlowChartCore.Cmdlets {

    // Find-FlowChartNodes, will help find "nodes" in a PS1 script
    [Cmdlet(VerbsCommon.Find,"FLowChartNodes")]
    [OutputType(typeof(Node))]
    public class FindFlowChartNodes : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "Script")]
        [ValidateNotNullAttribute]
        public ScriptBlock ScriptBlock { get; set; }

        private String[] _paths;
        private Boolean  _wildcards;

        [Parameter(
            ParameterSetName="Path",
            Mandatory=true,
            Position=0,
            ValueFromPipeline=true,
            ValueFromPipelineByPropertyName=true
        )]
        public String[] Path {
            get { return _paths; }
            set {
                _wildcards = true;
                _paths = value;
            }
        }

        [Parameter(
            ParameterSetName="Path",
            Mandatory=false,
            Position=0,
            ValueFromPipeline=true,
            ValueFromPipelineByPropertyName=true
        )]
        [Alias("PSPath")]
        public String[] LiteralPath {
            get { return _paths; }
            set { _paths = value; }
        }



        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            List<Node> ListOfNodes = new List<Node>();
            switch (this.ParameterSetName)
            {
                case "Path":
                    ProviderInfo pi;

                    foreach (var item in _paths)
                    {
                        String file = _wildcards ? this.SessionState.Path.GetResolvedProviderPathFromPSPath(item, out pi)[0] : this.SessionState.Path.GetUnresolvedProviderPathFromPSPath(item);
                        ListOfNodes =  FlowChartCore.Utility.ParseFile(file);
                        if (ListOfNodes.Count > 0 )
                        {
                            WriteObject(ListOfNodes);   
                        }

                    }
                    break;
                case "Script":
                    // Changing behavior, for example -scriptblocl {} return 0 nodes. We Want to return 0, not an empty List.
                    ListOfNodes =  FlowChartCore.Utility.ParseScriptBlock(ScriptBlock);
                    if (ListOfNodes.Count > 0 )
                    {
                        WriteObject(ListOfNodes);   
                    }
                    break;
                default:
                    break;
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
        }
    }

}