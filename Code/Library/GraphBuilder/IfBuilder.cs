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
            DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextId());
            DotDefinition.Add(edge);
        }

        public void CreateEndNode()
        {
            // throw new NotImplementedException();
            DotNode newnode = new DotNode(node.GetEndId());
            newnode.Shape = DotNodeShape.Point;
            DotDefinition.Add(newnode);
        }

        public void CreateFalseEdge()
        {
            if (node.children.Count > 0)
            {   
                // Node nodeFalse = node.children.Find(x => x.GetType() == typeof(FlowChartCore.ElseNode) || x.GetType() == typeof(FlowChartCore.ElseIfNode) ) ?? null;
                Node nodeFalse = node.children.Find(x => x is ElseNode || x is ElseIfNode ) ?? null;
                
                if ( nodeFalse != null ) {

                    // If the first child if a else node
                    // edge is drawn directly to the node id
                    // of the first child of the else
                    // if(nodeFalse.GetType() == typeof(ElseNode) )
                    if(nodeFalse is ElseNode )
                    {
                        DotEdge edge = new DotEdge(node.Id,nodeFalse.children[0].Id);
                        edge.Label="False";
                        DotDefinition.Add(edge);
                    }

                    // If the first child if a elseif node
                    // if(nodeFalse.GetType() == typeof(ElseIfNode) )
                    if(nodeFalse is ElseIfNode )
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
            newnode.Label = "If";
             DotDefinition.Add(newnode);
        }

        public void CreateTrueEdge()
        {
            // need to get the first children that is not a else
            // or a elseif node
            // Node firstchild = node.children.Find(x => x.GetType() != typeof(FlowChartCore.ElseNode) || x.GetType() != typeof(FlowChartCore.ElseIfNode))
            Console.WriteLine($"if first child true: {node.children[0].Name}");
            DotEdge edge = new DotEdge(node.Id,node.children[0].Id);
            edge.Label = "True";
            DotDefinition.Add(edge);
        }

    }
}