// using System.Management.Automation.Language;
using System.Collections.Generic;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class CodeNodeBuilder : IBuilder
    {
        
        public List<IDotElement> DotDefinition {get;set;}
        private CodeNode node;

        public CodeNodeBuilder(CodeNode codeNode)
        {
            node = codeNode;
            DotDefinition = new List<IDotElement>();
            CreateNode();
            CreateEdgeToNextSibling();
        }


        public void CreateEdgeToFirstChildren()
        {
            throw new NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            if(!node.IsLast)
            {
                // draw edge from end node to next sibling
                Node nextnode = node.GetNextNode();

                // si le nextnode est un else, on draw vers la fin du endif
                // cela veut dire qu on est dans un If !
                if( nextnode.GetType() == typeof(ElseNode) || nextnode.GetType() == typeof(ElseIfNode) || nextnode.GetType() == typeof(CatchNode) ) {
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
                    DotEdge edge = new DotEdge(node.GetEndId(),"end_of_script");
                    DotDefinition.Add(edge);
                } else {
                    // draw edge end of parent node
                    DotEdge edge = new DotEdge(node.GetEndId(),node.parent.GetEndId());
                    DotDefinition.Add(edge);
                }
            }
        }

        public void CreateEndNode()
        {
            throw new NotImplementedException();
        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = $"CodeBlock\n{node.Id}";
            DotDefinition.Add(newnode);
        }
    }
}