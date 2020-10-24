// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class ForeachObjectBuilder : IBuilder, IBuilderLoops
    {
        public List<IDotElement> DotDefinition {get;set;}
        private ForeachObjectNode node;
        
        public ForeachObjectBuilder(ForeachObjectNode foreachobjectnode)
        {
            node = foreachobjectnode;
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
            Edge.Label = "Loop End";
            DotDefinition.Add(Edge);
        }

        public void CreateEndNode()
        {
            DotNode newnode = new DotNode(node.GetEndId());
            newnode.Shape = DotNodeShape.Ellipse;
            newnode.Label = "Loop";
            DotDefinition.Add(newnode);
        }

        public void CreateLoopEdge()
        {
            DotEdge edge2 = new DotEdge(node.GetEndId(),node.Id);
            edge2.Label = $"Next Item From {node.Condition}";
            DotDefinition.Add(edge2);
        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = $"Foreach Object in {node.Condition}";
            DotDefinition.Add(newnode);
        }

    }
}