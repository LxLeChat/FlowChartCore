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
        // List of nodes
        [Parameter(
            Mandatory = true,
            Position = 0,
            ValueFromPipeline = true
        )]
        public List<Node> Nodes { get; set; }


        [Parameter(
            Mandatory = false,
            Position = 1,
            ValueFromPipeline = false
        )]
        public SwitchParameter CodeAsText { get; set; }

                [Parameter(
            Mandatory = false,
            Position = 1,
            ValueFromPipeline = false
        )]        
        private PowerShell psinstance {get;set;}
        private bool IsPSSAPresent {get;set;}

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            // Create a PowerShell instance, in order to call PSSA
            if( CodeAsText.IsPresent ) {
                WriteVerbose("Creating an new PowerShell instance for PSScriptAnalyzer...");
                psinstance = PowerShell.Create();
                string moduleName = "PSScriptAnalyzer";

                WriteVerbose("Checing if Module PSScriptAnalyzer is Present...");
                psinstance.AddScript($"Get-Module -ListAvailable -Name {moduleName}");
                var result = psinstance.Invoke();

                if( result.Count > 0 ) {
                    IsPSSAPresent = true;
                    WriteVerbose("Module PSScriptAnalyzer is Present...");
                } else {
                    IsPSSAPresent = false;
                    WriteVerbose("Module PSScriptAnalyzer is Missing...");
                    WriteWarning("Module PSScriptAnalyzer is missing... CodeBlock(s) representation might be strange...");
                }
                psinstance.Commands.Clear();
            }
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            WriteVerbose($"Processing {Nodes.Count} Nodes...");
            foreach (Node item in Nodes)
            {
                if (CodeAsText.IsPresent)
                {
                    if ( IsPSSAPresent )
                    {
                        item.GenerateGraph(true,true,psinstance);
                    } else {
                        item.GenerateGraph(true,true);
                    }
                    
                } else
                {
                    item.GenerateGraph(true);    
                }
                
            }

            List<IDotElement> dotElements = FlowChartCore.Utility.AddGraph(Nodes);
            String dotGraph = FlowChartCore.Utility.CompileDot(dotElements);
            WriteObject(dotGraph);
        }

        // This method will be called once at the end of pipeline execution; if no input is received, this method is not called
        protected override void EndProcessing()
        {
            if ( CodeAsText.IsPresent ) {
                WriteVerbose("Disposing of the Powershell instance...");
                psinstance.Dispose();
            }
        }
    
    }

}