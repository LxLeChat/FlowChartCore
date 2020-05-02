// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using FlowChartCore;
using System;

namespace FlowChartCore.Graph
{
    public interface IBuilder
    {
        void CreateNode();
        void CreateEdgeToNextSibling();
        void CreateEdgeToFirstChildren();
        void CreateEndNode();
    }

    public interface IBuilderIf
    {
        void CreateTrueEdge();
        void CreateFalseEdge();
    }

    public interface IBuilderLoops
    {
        void CreateLoopEdge();
    }

    public interface IBuilderTry
    {
        void CreateSubGraph();
    }

    public interface IBuilderBreak
    {
        void CreateBreakEdge();
    }

    public class IfBuilder : IBuilder, IBuilderIf
    {
        public List<String> DotDefinition = new List<string>();
        private IfNode node;

        public IfBuilder(IfNode IfNode)
        {
            node = IfNode;
            CreateNode();
            CreateEndNode();
            CreateTrueEdge();
            CreateFalseEdge();
            CreateEdgeToNextSibling();
        }

        public void CreateEdgeToFirstChildren()
        {
            throw new NotImplementedException();
        }

        public void CreateEdgeToNextSibling()
        {
            // Node NextSibling = node.;
            if(!node.IsLast)
            {
                // draw edge from end node to next sibling
                string plop = $"edge -from {node.GetEndId()} -to {node.GetNextNode().Id}";
                DotDefinition.Add(plop);
            } else {
                if (node.depth == 0 )
                {
                    // draw edge to end of script
                    string plop = $"edge -from {node.GetEndId()} -to 'end_of_script'";
                    DotDefinition.Add(plop);
                } else {
                    // draw edge end of parent node
                    string plop = $"edge -from {node.GetEndId()} -to {node.parent.GetEndId()}";
                    DotDefinition.Add(plop);
                }
            }
        }

        public void CreateEndNode()
        {
            // throw new NotImplementedException();
            string plop = $"node end_{node.Id}";
            DotDefinition.Add(plop);
        }

        public void CreateFalseEdge()
        {
            // throw new NotImplementedException();
            if (node.children.Count > 0)
            {
                Node nodeFalse = node.children.Find(x => x.GetType() == typeof(FlowChartCore.ElseNode) || x.GetType() == typeof(FlowChartCore.ElseIfNode) );
                if ( nodeFalse != null ) {
                    string plop = $"edge -from {node.Id} -to {nodeFalse.Id} -attributes @{{Label='False'}}";
                    DotDefinition.Add(plop);
                }   
            }
        }

        public void CreateNode()
        {
            // throw new NotImplementedException();
            string plop = $"node {node.Id} -attributes @{{Label='ifnode'}}";
            DotDefinition.Add(plop);
        }

        public void CreateTrueEdge()
        {
            // throw new NotImplementedException();
            string plop = $"edge -from {node.Id} -to {node.children[0].Id} -attributes @{{Label='True'}}";
            DotDefinition.Add(plop);
        }
    }

    public class ForeachBuilder : IBuilder, IBuilderLoops
    {
        public List<String> DotDefinition = new List<string>();
        private ForeachNode node;
        
        public ForeachBuilder(ForeachNode Foreach)
        {
            node = Foreach;
            CreateNode();
            CreateEndNode();
            CreateEdgeToFirstChildren();
            CreateLoopEdge();
            CreateEdgeToNextSibling();
        }

        public void CreateEdgeToFirstChildren()
        {
            // throw new NotImplementedException();
            string plop = $"edge -from {node.Id} -to {node.children[0].Id}";
            DotDefinition.Add(plop);
        }

        public void CreateEdgeToNextSibling()
        {
            // throw new NotImplementedException();
            if(!node.IsLast)
            {
                // draw edge from end node to next sibling
                string plop = $"edge -from {node.GetEndId()} -to {node.GetNextNode().Id}";
                DotDefinition.Add(plop);
            } else {
                if (node.depth == 0 )
                {
                    // draw edge to end of script
                    string plop = $"edge -from {node.GetEndId()} -to 'end_of_script'";
                    DotDefinition.Add(plop);
                } else {
                    // draw edge end of parent node
                    string plop = $"edge -from {node.GetEndId()} -to {node.parent.GetEndId()}";
                    DotDefinition.Add(plop);
                }
            }
        }

        public void CreateEndNode()
        {
            // throw new NotImplementedException();
            string plop = $"node end_{node.Id}";
            DotDefinition.Add(plop);
        }

        public void CreateLoopEdge()
        {
            // throw new NotImplementedException();
            string plop = $"edge -from {node.children[-1].GetEndId()} -to {node.Id}";
            DotDefinition.Add(plop);
        }

        public void CreateNode()
        {
            // throw new NotImplementedException();
            string plop = $"node {node.Id} -attributes @{{Label='ForeachNode'}}";
            DotDefinition.Add(plop);
        }
    }

}