using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class ReturnBuilder : IBuilder, IBuilderKeyWords
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private ReturnNode node;

        public ReturnBuilder(ReturnNode returnNode)
        {
            node = returnNode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEdgeToNextSibling();
            CreateSpecialEdge();

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Return";
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
            DotEdge specialedge = new DotEdge(node.Id,"end_of_script");
            
            specialedge.Label = "Exit";
            DotDefinition.Add(specialedge);
        }
    }

}