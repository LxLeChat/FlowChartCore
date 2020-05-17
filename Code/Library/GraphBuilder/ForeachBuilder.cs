// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class ForeachBuilder : IBuilder, IBuilderLoops
    {
        public List<IDotElement> DotDefinition {get;set;}
        private ForeachNode node;
        
        public ForeachBuilder(ForeachNode Foreach)
        {
            node = Foreach;
            DotDefinition = new List<IDotElement>();
            CreateNode();
            CreateEdgeToFirstChildren();
            CreateEndNode();
            CreateLoopEdge();
            CreateEdgeToNextSibling();
        }

        public void CreateEdgeToFirstChildren()
        {
            DotEdge edge = new DotEdge(node.Id,node.Children.First().Id);
            DotDefinition.Add(edge);
        }

        public void CreateEdgeToNextSibling()
        {
            DotEdge Edge = new DotEdge(node.GetEndId(),node.GetNextId());
            DotDefinition.Add(Edge);
        }

        public void CreateEndNode()
        {
            DotNode newnode = new DotNode(node.GetEndId());
            newnode.Shape = DotNodeShape.Ellipse;
            // newnode.Label = $"Loop to {node.Id}";
            newnode.Label = $"Next item In {node.GetAst().Condition}";
            DotDefinition.Add(newnode);
        }

        public void CreateLoopEdge()
        {
            DotEdge edge = new DotEdge(node.GetEndId(),node.Id);
            DotDefinition.Add(edge);
        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            // newnode.Label = "Foreach";
            newnode.Label = $"Foreach {node.GetAst().Variable} In {node.GetAst().Condition}";
            DotDefinition.Add(newnode);
        }

    }
}