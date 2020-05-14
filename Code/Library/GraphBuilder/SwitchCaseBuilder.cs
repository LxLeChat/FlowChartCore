using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class SwitchCaseBuilder : IBuilder
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private SwitchCaseNode node;

        public SwitchCaseBuilder(SwitchCaseNode switchasenode)
        {
            node = switchasenode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEdgeToFirstChildren();
            CreateEdgeToNextSibling();

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Switch Case";
            DotDefinition.Add(newnode);
        }

        public void CreateEdgeToNextSibling()
        {
            DotEdge edge = new DotEdge(node.Id,node.GetNextId());
            edge.Label = "False";
            DotDefinition.Add(edge);
        }

        public void CreateEdgeToFirstChildren()
        {
            DotEdge edge = new DotEdge(node.Id,node.Children[0].Id);
            DotDefinition.Add(edge);
        }

        public void CreateEndNode()
        {
            throw new System.NotImplementedException();
        }

    }

}