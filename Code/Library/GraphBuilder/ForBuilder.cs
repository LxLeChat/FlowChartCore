// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class ForBuilder : IBuilder, IBuilderLoops
    {
        public List<IDotElement> DotDefinition {get;set;}
        private ForNode node;
        
        public ForBuilder(ForNode fornode)
        {
            node = fornode;
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
            newnode.Label = "Loop";
            DotDefinition.Add(newnode);
        }

        public void CreateLoopEdge()
        {
            DotEdge edge2 = new DotEdge(node.GetEndId(),node.Id);
            edge2.Label = node.Iterator;
            DotDefinition.Add(edge2);
        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = $"For {node.Condition}";
            DotDefinition.Add(newnode);
        }

    }
}