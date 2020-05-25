using System;
using System.Collections.Generic;
using System.Management.Automation;
using DotNetGraph.Core;

namespace FlowChartCore.Cmdlets {

// New-FLowChartNodesGraph, will create a dot definition
    [Cmdlet(VerbsCommon.New,"FLowChartGraph")]
    [OutputType(typeof(string))]
    public class NewFlowChartGraph : PSCmdlet
    {
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true
        )]
        public List<Node> Nodes { get; set; }
        

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {

            foreach (Node item in Nodes)
            {
                item.GenerateGraph(true);
            }
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            List<IDotElement> dotElements = FlowChartCore.Utility.Plop(Nodes);
            String dotGraph = FlowChartCore.Utility.CompileDot(dotElements);
            WriteObject(dotGraph);
        }
    }
}