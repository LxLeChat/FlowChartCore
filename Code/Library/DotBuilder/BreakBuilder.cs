// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
// using DotNetGraph.Core;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class BreakBuilder : IBuilder, IBuilderKeyWords
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private BreakNode node;

        public BreakBuilder(BreakNode breaknode)
        {
            node = breaknode;
            DotDefinition = new List<IDotElement>();

            CreateNode();

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            DotDefinition.Add(newnode);
            // throw new System.NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            // si y a un sibling, on fait des petits points..
            DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextId());
            edge.Style = DotEdgeStyle.Dotted;
            DotDefinition.Add(edge);
            // throw new System.NotImplementedException();
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
            Node breakablenode = null;
            if (node.label == null)
            {
                breakablenode = node.FindNodesByTypeUp(typeof(ForeachNode));
            } else {
                breakablenode = node.FindNodesByLabelUp(node.Label);
            }

            DotEdge specialedge = new DotEdge(node.Id,breakablenode.GetNextNode().Id);
            DotEdge dottededge = new DotEdge(node.Id,node.GetNextNode().Id);
            DotDefinition.Add(dottededge);
            DotDefinition.Add(specialedge);
            throw new System.NotImplementedException();
        }
    }

}