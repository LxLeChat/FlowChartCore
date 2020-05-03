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
            Console.WriteLine("code graph");
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
                DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextNode().Id);
                DotDefinition.Add(edge);
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
        }

        public void CreateEndNode()
        {
            throw new NotImplementedException();
        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            DotDefinition.Add(newnode);
        }
    }
}