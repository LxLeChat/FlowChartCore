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
            // throw new NotImplementedException();
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
            // throw new NotImplementedException();
            DotNode newnode = new DotNode(node.GetEndId());
            newnode.Shape = DotNodeShape.Ellipse;
            newnode.Label = $"Loop to {node.Id}";
            DotDefinition.Add(newnode);
        }

        public void CreateLoopEdge()
        {
            // Node lastchild = node.Children.Last();
            // DotEdge edge = new DotEdge(lastchild.GetEndId(),node.GetEndId());
            // DotDefinition.Add(edge);
            DotEdge edge2 = new DotEdge(node.GetEndId(),node.Id);
            DotDefinition.Add(edge2);
        }

        public void CreateNode()
        {
            // throw new NotImplementedException();
            string plop = $"node {node.Id} -attributes @{{Label='ForeachNode'}}";
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Foreach";
            DotDefinition.Add(newnode);
        }

    }
}