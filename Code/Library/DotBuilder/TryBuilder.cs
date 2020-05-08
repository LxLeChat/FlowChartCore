// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
// using DotNetGraph.Core;
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

            CreateNode();
            CreateEndNode();
            CreateCatchEdge();
            CreateEdgeToFirstChildren();
            CreateEdgeToNextSibling();
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
            Node FinallyNode = node.children.Find(x => x.GetType() == typeof(FinallyNode));
            if ( FinallyNode != null) {
                CreateFinallyEdge(FinallyNode);
            } else {
                
                DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextId());
                DotDefinition.Add(edge);

            }

            // throw new System.NotImplementedException();            
        }

        public void CreateEndNode()
        {
            // throw new System.NotImplementedException();
            DotNode newnode = new DotNode(node.GetEndId());
            newnode.Shape = DotNodeShape.MDiamond;
            newnode.Label = "End Try";
            DotDefinition.Add(newnode);
        }

        public void CreateFinallyEdge(Node FinallyNode)
        {
            // throw new System.NotImplementedException();
            DotEdge edge = new DotEdge(node.GetEndId(),FinallyNode.Id);
            DotDefinition.Add(edge);
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