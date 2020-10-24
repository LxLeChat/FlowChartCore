using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class SwitchDefaultBuilder : IBuilder
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private SwitchDefaultNode node;

        public SwitchDefaultBuilder(SwitchDefaultNode switchasenode)
        {
            node = switchasenode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEdgeToFirstChildren();

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Default";
            DotDefinition.Add(newnode);
        }

        public void CreateEdgeToNextSibling()
        {
            throw new System.NotImplementedException();
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