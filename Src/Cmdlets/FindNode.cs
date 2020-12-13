using System;
using System.Management.Automation;
using System.Collections.Generic;

namespace FlowChartCore.Cmdlets {

    // Get-FlowChartNode, will return "nodes" from a PS1 script or a scriptblock
    [Cmdlet(VerbsCommon.Get,"FLowChartNode",DefaultParameterSetName = PathParameterSet)]
    [OutputType(typeof(Node))]
    public class GetFlowChartNodes : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            ParameterSetName = ScriptBlockParameterSet
        )]
        [ValidateNotNullAttribute]
        public ScriptBlock ScriptBlock { get; set; }

        /// <summary>
        /// Path parameter.
        /// The paths of the files.
        /// Resolved wildcards.
        /// </summary>
        /// <value></value>
        [Parameter(
            Mandatory = true,
            ParameterSetName = PathParameterSet,
            Position = 0,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true
        )]
        public string[] Path
        {
            get
            {
                return _paths;
            }

            set
            {
                _paths = value;
            }
        }

        /// <summary>
        /// LiteralPath parameter.
        /// The literal paths of the files.
        /// Don't resolved wildcards.
        /// </summary>
        /// <value></value>
        [Parameter(
            Mandatory = true,
            ParameterSetName = LiteralPathParameterSet,
            Position = 0,
            ValueFromPipelineByPropertyName = true
        )]
        [Alias("PSPath", "LP")]
        public string[] LiteralPath
        {
            get
            {
                return _paths;
            }

            set
            {
                _paths = value;
            }
        }

        private string[] _paths;

        /// <summary>
        /// Parameter set names.
        /// </summary>
        private const string PathParameterSet = "Path";
        private const string LiteralPathParameterSet = "LiteralPath";
        private const string ScriptBlockParameterSet = "ScriptBlock";

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            List<Node> ListOfNodes = new List<Node>();
            ProviderInfo pi;
            switch (this.ParameterSetName)
            {
                case PathParameterSet:
                    foreach (var item in _paths)
                    {
                        String file = this.SessionState.Path.GetResolvedProviderPathFromPSPath(item, out pi)[0];
                        ListOfNodes =  FlowChartCore.Utility.ParseFile(file);
                        if (ListOfNodes.Count > 0 )
                        {
                            WriteObject(ListOfNodes);   
                        }

                    }
                    break;
                case LiteralPathParameterSet :
                    foreach (var item in _paths)
                    {
                        String file = this.SessionState.Path.GetResolvedProviderPathFromPSPath(item, out pi)[0];
                        ListOfNodes =  FlowChartCore.Utility.ParseFile(file);
                        if (ListOfNodes.Count > 0 )
                        {
                            WriteObject(ListOfNodes);   
                        }

                    }
                    break;
                case ScriptBlockParameterSet:
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