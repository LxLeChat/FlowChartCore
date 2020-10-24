// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
// using DotNetGraph.Core;
using DotNetGraph.Node;
using System;

namespace FlowChartCore.Graph
{
    public class FinallyBuilder : IBuilder, IBuilderFinally
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private FinallyNode node;

        public FinallyBuilder(FinallyNode finallynode)
        {
            node = finallynode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEndNode();
            CreateEdgeToFirstChildren();
            CreateEndTryEdge();
        }

        public void CreateEdgeToFirstChildren()
        {
            DotEdge edge = new DotEdge(node.Id,node.children[0].Id);
            DotDefinition.Add(edge);
        }

        public void CreateEdgeToNextSibling()
        {

            // il n y a pas de nextsibling pour un node finally
            throw new System.NotImplementedException();

        }

        public void CreateEndNode()
        {
            // throw new System.NotImplementedException();
            DotNode newnode = new DotNode(node.GetEndId());
            newnode.Shape = DotNodeShape.MDiamond;
            newnode.Label = "End Finally";
            DotDefinition.Add(newnode);
        }

        public void CreateNode()
        {
            // throw new System.NotImplementedException();
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Finally";
            DotDefinition.Add(newnode);
        }

        public void CreateEndTryEdge()
        {
            Node nodeparent = node.parent;
            // Console.WriteLine("plop");
            
            if(!nodeparent.IsLast)
            {
                // draw edge from end node to next sibling
                Node nextnodeparent = nodeparent.GetNextNode();
                if( nextnodeparent.GetType() == typeof(ElseNode) || nextnodeparent.GetType() == typeof(ElseIfNode) ) {
                    DotEdge Edge = new DotEdge(node.GetEndId(),nextnodeparent.GetEndId());
                    DotDefinition.Add(Edge);
                } else {
                    DotEdge Edge = new DotEdge(node.GetEndId(),nextnodeparent.Id);
                    DotDefinition.Add(Edge);
                }
                
            } else {

                if (nodeparent.depth == 0 )
                {
                    DotEdge edge = new DotEdge(node.GetEndId(),"end_of_script");
                    DotDefinition.Add(edge);
                } else {
                    // draw edge end of parent node
                    DotEdge edge = new DotEdge(node.GetEndId(),nodeparent.parent.GetEndId());
                    DotDefinition.Add(edge);
                }
            }

            // throw new System.NotImplementedException();
        }
    }

}