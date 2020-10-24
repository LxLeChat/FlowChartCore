// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
// using DotNetGraph.Core;
using DotNetGraph.Node;

namespace FlowChartCore.Graph
{
    public class CatchBuilder : IBuilder
    {
        public List<IDotElement> DotDefinition { get ; set; }
        private CatchNode node;

        public CatchBuilder(CatchNode catchnode)
        {
            node = catchnode;
            DotDefinition = new List<IDotElement>();

            CreateNode();
            CreateEdgeToFirstChildren();
        }

        public void CreateEdgeToFirstChildren()
        {
            DotEdge edge = new DotEdge(node.Id,node.children[0].Id);
            DotDefinition.Add(edge);
        }

        public void CreateEdgeToNextSibling()
        {

            // si c'est le dernier on a rien à faire
            // le edge du dernier enfant, ira vers la fin du try
            // même si c'est pas le denier on s en fout enfait !

            throw new System.NotImplementedException();
        }

        public void CreateEndNode()
        {
            throw new System.NotImplementedException();
        }

        public void CreateNode()
        {
            DotNode newnode = new DotNode(node.Id);
            if(node.CatchTypes.Count > 0 ) {
                string catchtypes = string.Join("\n",node.CatchTypes);
                newnode.Label = "Catch\n"+catchtypes;
            } else {
                newnode.Label = "Catch";
            }
            
            DotDefinition.Add(newnode);
        }

    }

}