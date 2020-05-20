using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace FlowChartCore.Cmdlets {

    [Cmdlet(VerbsCommon.Get,"FLowChartNodes")]
    [OutputType(typeof(Node))]
    public class FLowChartNodesCmdletCommand : PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            Position = 0,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "Script")]
        public ScriptBlock ScriptBlock { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            ParameterSetName = "File")]
        public String[] Name { get; set; }
        // public string Path { get; set; } = "Dog"; //Default value.. !!!

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            WriteVerbose("Begin!");
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            List<Node> nodes = null;

            switch (this.ParameterSetName)
            {
                case "File":
                    WriteVerbose("File");
                    foreach (var item in Name)
                    {
                        nodes = FlowChartCore.Utility.ParseFile(item);
                        WriteObject(nodes);
                    }
                    break;
                case "Script":
                    nodes = FlowChartCore.Utility.ParseScriptBlock(ScriptBlock);
                    WriteObject(nodes);
                    break;
                default:
                    WriteVerbose("Hump..!");
                    break;
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            WriteVerbose("End!");
        }
    }

    // public class FavoriteStuff
    // {
    //     public int FavoriteNumber { get; set; }
    //     public string FavoritePet { get; set; }
    // }
}