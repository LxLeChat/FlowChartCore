// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
using DotNetGraph.Core;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class TryBuilder : IBuilder, IBuilderTry
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private TryNode node;

        public TryBuilder(TryNode trynode)
        {
            node = trynode;
            DotDefinition = new List<IDotElement>();
        }

        public void CreateCatchEdge()
        {
            // throw new System.NotImplementedException();
            List<Node> catchnodes = node.children.FindAll(x => x.GetType() == typeof(CatchNode));
            foreach (var item in catchnodes) {
                DotEdge edge = new DotEdge(node.Id,item.Id);
                edge.Label = "Catches Error";
                DotDefinition.Add(edge);

            }
            
        }

        public void CreateEdgeToFirstChildren()
        {
            // throw new System.NotImplementedException();
            Node child = node.children.Find(x => x.GetType() != typeof(CatchNode) || x.GetType() != typeof(FinallyNode));
            DotEdge edge = new DotEdge(node.Id,child.Id);
            DotDefinition.Add(edge);
        }

        public void CreateEdgeToNextSibling()
        {
            if(!node.IsLast)
            {
                // draw edge from end node to next sibling
                Node nextnode = node.GetNextNode();

                // si le nextnode est un else, on draw vers la fin du endif
                if( nextnode.GetType() == typeof(ElseNode) || nextnode.GetType() == typeof(ElseIfNode) ) {
                    DotEdge Edge = new DotEdge(node.GetEndId(),nextnode.GetEndId());
                    DotDefinition.Add(Edge);
                } else {
                    DotEdge Edge = new DotEdge(node.GetEndId(),nextnode.Id);
                    DotDefinition.Add(Edge);
                }

            } else {
                
                if (node.depth == 0 )
                {
                    // draw edge to end of script
                    string plop = $"edge -from {node.GetEndId()} -to 'end_of_script'";
                } else {
                    // draw edge end of parent node
                    DotEdge edge = new DotEdge(node.GetEndId(),node.parent.GetEndId());
                    DotDefinition.Add(edge);
                }
            }

            // throw new System.NotImplementedException();
            Node FinallyNode = node.children.Find(x => x.GetType() == typeof(FinallyNode));
            if ( FinallyNode == null) {
                DotEdge edge = new DotEdge(node.GetEndId(),)
            }
        }

        public void CreateEndNode()
        {
            // throw new System.NotImplementedException();
            DotNode newnode = new DotNode(node.GetEndId());
            newnode.Shape = DotNodeShape.MDiamond;
            newnode.Label = "End Try";
            DotDefinition.Add(newnode);
        }

        public void CreateFinallyEdge()
        {
            throw new System.NotImplementedException();
        }

        public void CreateNode()
        {
            // throw new System.NotImplementedException();
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Try";
            DotDefinition.Add(newnode);
        }

    }

}