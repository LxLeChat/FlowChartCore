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
            if (!(node.IsLast && node.depth == 0))
            {
                DotEdge edge = new DotEdge(node.GetEndId(),node.GetNextId());
                edge.Style = DotEdgeStyle.Dotted;
                DotDefinition.Add(edge);
            }
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
            DotEdge specialedge = null;
            if (node.label == null)
            {
                breakablenode = node.FindNodesUp(x => x is ForeachNode || x is ForNode || x is WhileNode || x is DoWhileNode || x is DoUntilNode || x is ForNode || x is SwitchNode);

                // New fix for Issue #17
                if (breakablenode == null){
                    specialedge = new DotEdge(node.Id,"end_of_script");
                } else {
                    specialedge = new DotEdge(node.Id,breakablenode.GetNextId());
                    // fix when label is null, label = "Break From Previous Loop"
                    if (node.label != null)
                    {
                        specialedge.Label = $"Break From {node.Label}";
                    } else {
                        specialedge.Label = $"Break From Previous Loop";
                    }
                    
                }
            } else {
                breakablenode = node.FindNodesUp(x => x.label == node.label);
                specialedge = new DotEdge(node.Id,breakablenode.GetNextId());
                //fix, missing label
                specialedge.Label = $"Break From {node.Label}";
            }

            
            DotDefinition.Add(specialedge);
        }
    }

}