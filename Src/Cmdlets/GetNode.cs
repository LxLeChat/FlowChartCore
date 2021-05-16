using System;
using System.Collections.ObjectModel;
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
            Position = 0,
            ParameterSetName = ScriptBlockParameterSet
        )]
        [ValidateNotNullAttribute]
        public ScriptBlock ScriptBlock { get; set; }

        /// <summary>
        /// Path parameter.
        /// The paths of the files to calculate hash values.
        /// Resolved wildcards.
        /// </summary>
        /// <value></value>
        [Parameter(Mandatory = true, ParameterSetName = PathParameterSet, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
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
        /// The literal paths of the files to calculate a hashs.
        /// Don't resolved wildcards.
        /// </summary>
        /// <value></value>
        [Parameter(Mandatory = true, ParameterSetName = LiteralPathParameterSet, Position = 0, ValueFromPipelineByPropertyName = true)]
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

            List<string> pathsToProcess = new List<string>();
            ProviderInfo provider = null;
            List<Node> ListOfNodes = new List<Node>();
  
            switch (ParameterSetName)
            {
                case PathParameterSet:
                    foreach (string path in _paths)
                    {
                        try
                        {
                            Collection<string> newPaths = this.SessionState.Path.GetResolvedProviderPathFromPSPath(path, out provider);
                            if (newPaths != null)
                            {
                                pathsToProcess.AddRange(newPaths);
                            }
                        }
                        catch (ItemNotFoundException e)
                        {
                            if (!WildcardPattern.ContainsWildcardCharacters(path))
                            {
                                ErrorRecord errorRecord = new ErrorRecord(e,
                                    "FileNotFound",
                                    ErrorCategory.ObjectNotFound,
                                    path);
                                WriteError(errorRecord);
                            }
                        }
                    }
                    break;
                case LiteralPathParameterSet :
                    foreach (string path in _paths)
                    {
                        string newPath = this.SessionState.Path.GetUnresolvedProviderPathFromPSPath(path);
                        pathsToProcess.Add(newPath);
                    }
                    break;
            }

            foreach (string path in pathsToProcess)
            {
                ListOfNodes =  FlowChartCore.Utility.ParseFile(path);
                if (ListOfNodes.Count > 0 )
                {
                    WriteObject(ListOfNodes);   
                }
            }
        
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            List<Node> ListOfNodes = new List<Node>();
            if (ParameterSetName == ScriptBlockParameterSet)
            {
                ListOfNodes =  FlowChartCore.Utility.ParseScriptBlock(ScriptBlock);
                if (ListOfNodes.Count > 0 )
                {
                    WriteObject(ListOfNodes);   
                }
            }
        }
    }

}