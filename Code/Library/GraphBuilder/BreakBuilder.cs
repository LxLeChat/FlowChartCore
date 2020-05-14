// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
using DotNetGraph.Node;
using System;

namespace FlowChartCore.Graph
{
    public class BreakBuilder : IBuilder, IBuilderKeyWords
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private BreakNode node;

        public BreakBuilder(BreakNode breaknode)
        {
            node = breaknode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEdgeToNextSibling();
            CreateSpecialEdge();

        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            newnode.Label = "Break";
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
            
            Node breakablenode = null;
            if (node.label == null)
            {
                breakablenode = node.FindNodesUp(x => x is ForeachNode || x is WhileNode || x is DoWhileNode || x is DoUntilNode || x is ForNode);
            } else {
                breakablenode = node.FindNodesUp(x => x.label == node.label);
            }

            DotEdge specialedge = new DotEdge(node.Id,breakablenode.GetNextId());
            // DotEdge dottededge = new DotEdge(node.Id,node.GetNextId());
            // DotDefinition.Add(dottededge);
            DotDefinition.Add(specialedge);
        }
    }

}