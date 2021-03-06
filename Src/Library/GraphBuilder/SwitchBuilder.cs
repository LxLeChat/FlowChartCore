using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class SwitchBuilder : IBuilder
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private SwitchNode node;

        public SwitchBuilder(SwitchNode switchnode)
        {
            node = switchnode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEndNode();
            CreateEdgeToFirstChildren();
            CreateEdgeToNextSibling();

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);

            // fix issue #54
            if (node.flags != null)
            {
                newnode.Label = $"Switch {node.flags} {node.Condition}";    
            } else {
                newnode.Label = $"Switch {node.Condition}";
            }
            
            DotDefinition.Add(newnode);
        }

        public void CreateEdgeToNextSibling()
        {
            DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextId());
            DotDefinition.Add(edge);
        }

        public void CreateEdgeToFirstChildren()
        {
            DotEdge edge = new DotEdge(node.Id,node.Children[0].Id);
            DotDefinition.Add(edge);
        }

        public void CreateEndNode()
        {
            DotNode endnode = new DotNode(node.GetEndId());
            endnode.Shape = DotNodeShape.Diamond;
            DotDefinition.Add(endnode);
        }

    }

}