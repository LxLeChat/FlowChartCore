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
        [Parameter(Mandatory = true, Position = 0, ValueFromPipeline = true)]
        public List<Node> Nodes { get; set; }

        [Parameter(Mandatory = false, ValueFromPipeline = false)]
        [ValidateSet("Standard","Formatted")]
        public String CodeAsText { get; set; }

        private PowerShell psinstance {get;set;}
        private bool IsPSSAPresent {get;set;}

        // This method gets called once for each cmdlet in the pipeline when the pipeline starts executing
        protected override void BeginProcessing()
        {
            if (CodeAsText == "Formatted")
            {
                // Create a PowerShell instance, in order to call PSSA
                string moduleName = "PSScriptAnalyzer";
                WriteVerbose($"Creating an new PowerShell instance for {moduleName}...");
                psinstance = PowerShell.Create();

                WriteVerbose($"Checing if Module {moduleName} is Present...");
                psinstance.AddScript($"Get-Module -ListAvailable -Name {moduleName}").Invoke();
                var result = psinstance.Invoke();

                if( result.Count > 0 ) {
                    IsPSSAPresent = true;
                    WriteVerbose($"Module {moduleName} is Present...");
                } else {
                    WriteVerbose($"Module {moduleName} is not installed... is it loaded ?");    
                    // Maybe PSSA has been loaded from source ?
                    psinstance.Commands.Clear();
                    var result2 = psinstance.AddScript($"Get-Module -Name {moduleName}").Invoke();

                    if( result2.Count > 0 ) {
                        IsPSSAPresent = true;
                        WriteVerbose($"Module {moduleName} is Loaded...");
                    } else {
                        IsPSSAPresent = false;
                        WriteVerbose($"Module {moduleName} is Missing, try 'Install-Module -Name PSScriptAnalyzer'...");
                    }
                }
                psinstance.Commands.Clear();
                
                // If PSSA was not found, or not loaded from source,
                // we dont need the psinstance anymore
                if ( !IsPSSAPresent )
                {
                    WriteWarning($"Module {moduleName} is missing... Falling back to a Standard CodeBlock(s) representation...");
                    WriteVerbose("Disposing of the PowerShell instance...");
                    psinstance.Dispose();
                }
            }
        }

        // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
        protected override void ProcessRecord()
        {
            WriteVerbose($"Processing Nodes...");
            foreach (Node item in Nodes)
            {

                // revoir les conditions
                // reesayer une az fonction en c# voir les erreurs
                if (CodeAsText == "Formatted")
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
            if ( CodeAsText == "Formatted" && IsPSSAPresent ) {
                WriteVerbose("Disposing of the Powershell instance...");
                psinstance.Dispose();
            }
        }
    
    }

}