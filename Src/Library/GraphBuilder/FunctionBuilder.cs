// using System.Management.Automation.Language;
using System.Collections.Generic;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class FunctionNodeBuilder : IBuilder
    {
        
        public List<IDotElement> DotDefinition {get;set;}
        private FunctionNode node;

        public FunctionNodeBuilder(FunctionNode functionNode)
        {
            node = functionNode;
            DotDefinition = new List<IDotElement>();
            CreateNode();
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
            newnode.Label = $"Function Declaration: {node.FunctionName}";
            DotDefinition.Add(newnode);
        }

        // public void CreateNode(bool codeAsText)
        // {
        //     DotNode newnode = new DotNode(node.Id);
        //     // need to replace \n in label with \l
        //     // this will align text to the left
        //     // Change made in DotNetGraph DotCompiler, FormatString Method
        //     String label = node.discovercode();
        //     newnode.Label = label;
        //     newnode.Shape = DotNodeShape.Box;
        //     DotDefinition.Add(newnode);
        // }
    }

}