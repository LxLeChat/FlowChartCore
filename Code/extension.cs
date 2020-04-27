using System;
using System.Management.Automation.Language;
using System.Collections.Generic;
using FlowChartCore;

namespace ExtensionMethods
{
    public static class ASTExtensions
    {
        // ForStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static ForeachNode CreateNodeFromAst(this ForEachStatementAst f,int x, int z,Node p)
        {
            ForeachNode Freach = new ForeachNode(f,x,z,p);
            return Freach;
        }

        public static List<Node> CreateChildNodes (this ForEachStatementAst f, int Depth,Node p)
        {
            List<Node> Nodes = new List<Node>();
            IEnumerable<Ast> enumerable = f.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == f.Body, false);

            int Position = 1;

            foreach ( var block in enumerable ) {
                    Nodes.Add(FlowChartCore.Utility.CastedItem(block,Depth,Position,p));
                    Position++;
            }

            return Nodes;
        }
        
        // ForStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static ForNode CreateNodeFromAst(this ForStatementAst f,int x, int z, Node p)
        {
            ForNode Freach = new ForNode(f,x,z,p);
            return Freach;
        }

        public static List<Node> CreateChildNodes (this ForStatementAst f, int Depth, Node p)
        {
            List<Node> Nodes = new List<Node>();
            IEnumerable<Ast> enumerable = f.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == f.Body, false);

            int Position = 1;

            foreach ( var block in enumerable ) {
                    Nodes.Add(FlowChartCore.Utility.CastedItem(block,Depth,Position,p));
                    Position++;
            }

            return Nodes;
        }

    }
}