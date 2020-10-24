// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
// using DotNetGraph.Core;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class ContinueBuilder : IBuilder, IBuilderKeyWords
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private ContinueNode node;

        public ContinueBuilder(ContinueNode continueNode)
        {
            node = continueNode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEdgeToNextSibling();
            CreateSpecialEdge();

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Continue";
            DotDefinition.Add(newnode);
        }

        public void CreateEdgeToNextSibling()
        {
            // si y a un sibling, on fait des petits points..
            DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextId());
            edge.Style = DotEdgeStyle.Dotted;
            DotDefinition.Add(edge);
        }

        public void CreateEdgeToFirstChildren()
        {
            throw new System.NotImplementedException();
        }

        public void CreateEndNode()
        {
            throw new System.NotImplementedException();
        }

        public void CreateSpecialEdge()
        {
            Node ContinueNode = null;
            DotEdge specialedge = null;
            if (node.label == null)
            {
                ContinueNode = node.FindNodesUp(x => x is ForeachNode || x is WhileNode || x is DoWhileNode || x is DoUntilNode || x is ForNode);
                specialedge = new DotEdge(node.Id,ContinueNode.Id);
                specialedge.Label = $"Continue To {node.Label}";
                
            } else {
                ContinueNode = node.FindNodesUp(x => x.label == node.label);
                specialedge = new DotEdge(node.Id,ContinueNode.Id);
            }
            DotDefinition.Add(specialedge);
        }
    }

}