// using System.Management.Automation.Language;
using System.Collections.Generic;
using DotNetGraph.Core;
using DotNetGraph.Edge;
// using DotNetGraph.Core;
using DotNetGraph.Node;

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

        }

        public void CreateNode()
        {
            throw new System.NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            // si y a un sibling, on fait des petits points..
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }

}