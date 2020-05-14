using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
using DotNetGraph.Node;
using System;

namespace FlowChartCore.Graph
{
    public class ExitBuilder : IBuilder, IBuilderKeyWords
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private ExitNode node;

        public ExitBuilder(ExitNode exitnode)
        {
            node = exitnode;
            DotDefinition = new List<IDotElement>();

            Console.WriteLine("1");
            CreateNode();
            Console.WriteLine("2");
            CreateEdgeToNextSibling();
            Console.WriteLine("3");
            CreateSpecialEdge();
            Console.WriteLine("4");

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Exit";
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
            Console.WriteLine("caca");
            DotEdge specialedge = new DotEdge(node.Id,"end_of_script");
            
            specialedge.Label = "Exit";
            DotDefinition.Add(specialedge);
        }
    }

}