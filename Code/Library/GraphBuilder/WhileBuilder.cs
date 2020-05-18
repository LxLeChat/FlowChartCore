// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class WhileBuilder : IBuilder, IBuilderLoops
    {
        public List<IDotElement> DotDefinition {get;set;}
        private WhileNode node;
        
        public WhileBuilder(WhileNode whilenode)
        {
            node = whilenode;
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
            newnode.Label = "Next Element";
            DotDefinition.Add(newnode);
        }

        public void CreateLoopEdge()
        {
            DotEdge edge2 = new DotEdge(node.GetEndId(),node.Id);
            DotDefinition.Add(edge2);
        }

        public void CreateNode()
        {
            // throw new NotImplementedException();
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = $"While {node.Condition}";
            DotDefinition.Add(newnode);
        }

    }
}