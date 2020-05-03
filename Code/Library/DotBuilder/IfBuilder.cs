// using System.Management.Automation.Language;
using System.Collections.Generic;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class IfBuilder : IBuilder, IBuilderIf
    {
        public List<IDotElement> DotDefinition {get;set;}
        private IfNode node;

        public IfBuilder(IfNode IfNode)
        {
            node = IfNode;
            DotDefinition = new List<IDotElement>();
            CreateNode();
            CreateEndNode();
            CreateTrueEdge();
            CreateFalseEdge();
            CreateEdgeToNextSibling();
        }

        public void CreateEdgeToFirstChildren()
        {
            throw new NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            Console.WriteLine("if next");
            // Node NextSibling = node.;
            if(!node.IsLast)
            {
                // draw edge from end node to next sibling
                // string plop = $"edge -from {node.GetEndId()} -to {node.GetNextNode().Id}";
                DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextNode().Id);
                // DotDefinition.Add(plop);
                DotDefinition.Add(edge);
            } else {
                if (node.depth == 0 )
                {
                    // draw edge to end of script
                    // string plop = $"edge -from {node.GetEndId()} -to 'end_of_script'";
                    DotEdge edge = new DotEdge(node.GetEndId(),"end_of_script");
                    // DotDefinition.Add(plop);
                    DotDefinition.Add(edge);
                } else {
                    // draw edge end of parent node
                    // string plop = $"edge -from {node.GetEndId()} -to {node.parent.GetEndId()}";
                    DotEdge edge = new DotEdge(node.GetEndId(),node.parent.GetEndId());
                    // DotDefinition.Add(plop);
                    DotDefinition.Add(edge);
                }
            }
        }

        public void CreateEndNode()
        {
            // throw new NotImplementedException();
            Console.WriteLine("if endnode");
            // string plop = $"node end_{node.Id}";
            DotNode newnode = new DotNode($"end_{node.Id}");
            newnode.Shape = DotNodeShape.Point;
            DotDefinition.Add(newnode);
        }

        public void CreateFalseEdge()
        {
            Console.WriteLine("if falsch");
            // throw new NotImplementedException();
            if (node.children.Count > 0)
            {
                Node nodeFalse = node.children.Find(x => x.GetType() == typeof(FlowChartCore.ElseNode) || x.GetType() == typeof(FlowChartCore.ElseIfNode) );
                if ( nodeFalse != null ) {

                    // If the first child if a else node
                    // edge is drawn directly to the node id
                    // of the first child of the else
                    if(nodeFalse.GetType() == typeof(ElseNode) )
                    {
                        DotEdge edge = new DotEdge(node.Id,nodeFalse.children[0].Id);
                        edge.Label="False";
                        DotDefinition.Add(edge);
                    }

                    // If the first child if a elseif node
                    if(nodeFalse.GetType() == typeof(ElseIfNode) )
                    {
                        DotEdge edge = new DotEdge(node.Id,nodeFalse.Id);
                        edge.Label="False";
                        DotDefinition.Add(edge);
                    }
                }   
            }
        }

        public void CreateNode()
        {
            // throw new NotImplementedException();
            Console.WriteLine("if create node");
            // string plop = $"node {node.Id} -attributes @{{Label='ifnode'}}";
            DotNode newnode = new DotNode(node.Id);
            // DotDefinition.Add(plop);
             DotDefinition.Add(newnode);
        }

        public void CreateTrueEdge()
        {
            DotEdge edge = new DotEdge(node.Id,node.children[0].Id);
            edge.Label = "True";
            DotDefinition.Add(edge);
        }

    }
}