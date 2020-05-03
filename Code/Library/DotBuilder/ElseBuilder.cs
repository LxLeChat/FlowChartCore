// using System.Management.Automation.Language;
using System.Collections.Generic;
using System;
using DotNetGraph.Node;
using DotNetGraph.Edge;
using DotNetGraph.Core;

namespace FlowChartCore.Graph
{
    public class ElseBuilder : IBuilder
    {
        public List<IDotElement> DotDefinition {get;set;}
        private ElseNode node;

        public ElseBuilder(ElseNode elseNode)
        {
            node = elseNode;
            DotDefinition = new List<IDotElement>();
                
        }

        public void CreateNode()
        {
            throw new NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            throw new NotImplementedException();
        }

        public void CreateEdgeToFirstChildren()
        {
            throw new NotImplementedException();
        }

        public void CreateEndNode()
        {
            throw new NotImplementedException();
        }
    }
}