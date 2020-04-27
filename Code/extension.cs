using System;
using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using FlowChartCore;

namespace ExtensionMethods
{
    public static class ASTExtensions
    {
        public static Node CreateNode (this Ast _ast, int _depth, int _position, Node _parent)
        {
            switch (_ast)
            {
                case Ast a when _ast is ForEachStatementAst : 
                    return ((ForEachStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent);
                case Ast a when _ast is ForStatementAst : 
                    return ((ForStatementAst)_ast).CreateNodeFromAst(_depth,_position,_parent);
            }
            return null;
        }
        

        // ForStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition, Parent) => Creates a Node
        // - GetChildAst() => retourne only ASTs we are looking for ...
        public static ForeachNode CreateNodeFromAst(this ForEachStatementAst _ast,int _depth, int _position,Node _parent)
        {
            return new ForeachNode(_ast,_depth,_position,_parent);
        }


        public static IEnumerable<Ast> GetChildAst (this ForEachStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        }
        
        // ForStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static ForNode CreateNodeFromAst(this ForStatementAst _ast, int _depth, int _position, Node _parent)
        {
            return new ForNode(_ast,_depth,_position,_parent);
        }

        public static IEnumerable<Ast> GetChildAst (this ForStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        }

    }
}