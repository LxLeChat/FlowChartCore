using System.Collections.Generic;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;
using System.Management.Automation;

namespace FlowChartCore.Graph
{
    public class CodeNodeBuilder : IBuilder
    {
        
        public List<IDotElement> DotDefinition {get;set;}
        private CodeNode node;

        public CodeNodeBuilder(CodeNode codeNode)
        {
            node = codeNode;
            DotDefinition = new List<IDotElement>();
            CreateNode();
            CreateEdgeToNextSibling();
        }

        public CodeNodeBuilder(CodeNode codeNode, bool codeAsText)
        {
            node = codeNode;
            DotDefinition = new List<IDotElement>();
            CreateNode(codeAsText);
            CreateEdgeToNextSibling();
        }

        public CodeNodeBuilder(CodeNode codeNode, bool codeAsText, PowerShell PSInstance)
        {
            node = codeNode;
            DotDefinition = new List<IDotElement>();
            CreateNode(codeAsText, PSInstance);
            CreateEdgeToNextSibling();
        }


        public void CreateEdgeToFirstChildren()
        {
            throw new NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextId());
            DotDefinition.Add(edge);
        }

        public void CreateEndNode()
        {
            throw new NotImplementedException();
        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = $"CodeBlock";
            DotDefinition.Add(newnode);
        }

        public void CreateNode(bool codeAsText)
        {
            DotNode newnode = new DotNode(node.Id);
            // need to replace \n in label with \l
            // this will align text to the left
            // Change made in DotNetGraph DotCompiler, FormatString Method
            String label = node.discovercode();
            newnode.Label = label;
            newnode.Shape = DotNodeShape.Box;
            DotDefinition.Add(newnode);
        }

        // Formatting the label, discovercode, with PSScriptAnalyzer
        // Make things slower, but worse it!
        // Fix #103
        public void CreateNode(bool codeAsText, PowerShell PSInstance)
        {
            DotNode newnode = new DotNode(node.Id);
            // need to replace \n in label with \l
            // this will align text to the left
            // Change made in DotNetGraph DotCompiler, FormatString Method
            String label = node.discovercode();
            PSInstance.Commands.Clear();
            PSInstance.AddCommand("Invoke-Formatter");
            PSInstance.AddParameter("ScriptDefinition", label);
            var PSSAResult = PSInstance.Invoke();

            string PSSAResultString = null;
            if ( PSSAResult.Count == 0 ) {
                PSSAResultString = " ERROR SCRIPTANALYZER";
            } else {
                PSSAResultString = PSSAResult[0].BaseObject.ToString();
            }
            newnode.Label = PSSAResultString + System.Environment.NewLine;
            newnode.Shape = DotNodeShape.Box;
            DotDefinition.Add(newnode);
        }
    }

}