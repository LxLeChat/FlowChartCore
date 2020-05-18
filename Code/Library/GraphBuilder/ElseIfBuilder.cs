// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Node;
using DotNetGraph.Edge;

namespace FlowChartCore.Graph
{
    public class ElseIfBuilder : IBuilder, IBuilderIf
    {
        private ElseIfNode node;
        public List<IDotElement> DotDefinition { get ; set ; }
        public ElseIfBuilder(ElseIfNode elseIf)
        {
            node = elseIf;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateTrueEdge();
            CreateFalseEdge();
        }

        public void CreateEdgeToFirstChildren()
        {
            throw new System.NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            // not implemented because, the next sibling is always
            // another elseif or a else node... instead we use
            // the CreateFalseEdge
            throw new System.NotImplementedException();
        }

        public void CreateEndNode()
        {
            // not implement because, the endid of an elseif node
            // is in fact the the end id of the parent if node
            throw new System.NotImplementedException();
        }

        public void CreateFalseEdge()
        {
            // throw new System.NotImplementedException();
            Node nextnode = node.GetNextNode();
            if (nextnode.GetType() == typeof(ElseNode) )
            {
                DotEdge edge = new DotEdge(node.Id,node.GetNextNode().children[0].Id);
                edge.Label = "False";
                DotDefinition.Add(edge);
            } else {
                DotEdge edge = new DotEdge(node.Id,node.GetNextNode().Id);
                edge.Label = "False";
                DotDefinition.Add(edge);
            }

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = $"If {node.Condition}";
            newnode.Shape = DotNodeShape.Diamond;
            DotDefinition.Add(newnode);
        }

        public void CreateTrueEdge()
        {
            // throw new System.NotImplementedException();
            DotEdge edge = new DotEdge(node.Id,node.children[0].Id);
            edge.Label = "True";
            DotDefinition.Add(edge);
        }
    }
}