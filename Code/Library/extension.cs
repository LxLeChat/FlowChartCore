using System;
using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using FlowChartCore;

// refaire les commentaires: au dessus de chaque classe
namespace ExtensionMethods
{
    public static class ASTExtensions
    {
        // Extensions Methods To Help Create Nodes From an unknown Ast
        // Special case for StatementBlockAST... Go to Extension Methods for this type
        public static Node CreateNode (this Ast _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            switch (_ast)
            {
                case Ast a when _ast is ForEachStatementAst : 
                    return ((ForEachStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is ForStatementAst : 
                    return ((ForStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is IfStatementAst : 
                    return ((IfStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is SwitchStatementAst : 
                    return ((SwitchStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is WhileStatementAst : 
                    return ((WhileStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is DoWhileStatementAst : 
                    return ((DoWhileStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is DoUntilStatementAst : 
                    return ((DoUntilStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is TryStatementAst : 
                    return ((TryStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent, _tree);
                case Ast a when _ast is CatchClauseAst : 
                    return ((CatchClauseAst)_ast).CreateNodeFromAst(_depth, _position, _parent);
                case Ast a when _ast is BreakStatementAst : 
                    return ((BreakStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent);
                case Ast a when _ast is ContinueStatementAst : 
                    return ((ContinueStatementAst)_ast).CreateNodeFromAst(_depth, _position, _parent);
            }
            return null;
        }

    }

    public static class ContinueStatementExtension {
        // ContinueStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition, Parent) => Creates a Node
        // - GetChildAst() => retourne only ASTs we are looking for ...
        public static ContinueNode CreateNodeFromAst(this ContinueStatementAst _ast,int _depth, int _position, Node _parent)
        {
            return new ContinueNode(_ast,_depth, _position, _parent);
        }
        
    }

    public static class BreakStatementExtension {
        // BreakStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition, Parent) => Creates a Node
        // - GetChildAst() => retourne only ASTs we are looking for ...
        public static BreakNode CreateNodeFromAst(this BreakStatementAst _ast,int _depth, int _position, Node _parent)
        {
            return new BreakNode(_ast,_depth, _position, _parent);
        }
        
    }
    
    public static class StatementBlockExtensions {
        // StatementBlockAst Extension Methods
        // - CreateNode() => Return ElseIf, Else, SwitchCaseNode... because there is no specific AST for this kind
        // - GetChildAst() => retourne only ASTs we are looking for ...
        public static Node CreateNode(this StatementBlockAst _ast,int _depth, int _position, Node _parent, FlowChartCore.StatementType type)
        {
            switch (type)
            {
                case FlowChartCore.StatementType.Else : {
                    return new ElseNode(_ast, _depth, _position, _parent);
                }
                case FlowChartCore.StatementType.ElseIf : {
                    return new ElseIfNode(_ast, _depth, _position, _parent);
                }
                case FlowChartCore.StatementType.SwitchCase : {
                    return new SwitchCaseNode(_ast, _depth, _position, _parent);
                }
                case FlowChartCore.StatementType.SwitchDefault : {
                    return new SwitchDefaultNode(_ast, _depth, _position, _parent);
                }
                case FlowChartCore.StatementType.Finally : {
                    return new FinallyNode(_ast, _depth, _position, _parent);
                }
                
            }
            return null;
        }

        // public static IEnumerable<Ast> GetChildAst (this StatementBlockAst _ast)
        // {
        //     return _ast.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast, false);
        // }
        public static IEnumerable<Ast> GetChildAst (this StatementBlockAst _ast)
        {
            return _ast.FindAll(Args => Args is Ast && Args.Parent == _ast, false);
        }
    }
   
    public static class ForEachStatementExtensions {
        // ForStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition, Parent) => Creates a Node
        // - GetChildAst() => retourne only ASTs we are looking for ...
        public static ForeachNode CreateNodeFromAst(this ForEachStatementAst _ast,int _depth, int _position, Node _parent, Tree _tree)
        {
            return new ForeachNode(_ast,_depth, _position, _parent, _tree);
        }


        // public static IEnumerable<Ast> GetChildAst (this ForEachStatementAst _ast)
        // {
        //     return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        // }
        
        public static IEnumerable<Ast> GetChildAst (this ForEachStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && Args.Parent == _ast.Body, false);
        }

    }

    public static class ForStatementExtensions {
        // ForStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static ForNode CreateNodeFromAst(this ForStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            return new ForNode(_ast,_depth, _position, _parent, _tree);
        }

        // public static IEnumerable<Ast> GetChildAst (this ForStatementAst _ast)
        // {
        //     return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        // }

        public static IEnumerable<Ast> GetChildAst (this ForStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && Args.Parent == _ast.Body, false);
        }

    }

    public static class IfStatementExtensions {
        // IfStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition, Parent) => Creates a Node
        // - GetChildAst() => retourne only ASTs we are looking for ...
        public static IfNode CreateNodeFromAst(this IfStatementAst _ast,int _depth, int _position, Node _parent, Tree _tree)
        {
            return new IfNode(_ast,_depth, _position, _parent, _tree);
        }

        // public static IEnumerable<Ast> GetChildAst (this IfStatementAst _ast)
        // {
        //     // throw new NotImplementedException("Not implemented at the moment");
        //     return _ast.Clauses[0].Item2.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Clauses[0].Item2, false);
        // }
        public static IEnumerable<Ast> GetChildAst (this IfStatementAst _ast)
        {
            return _ast.Clauses[0].Item2.FindAll(Args => Args is Ast && Args.Parent == _ast.Clauses[0].Item2, false);
        }

        // Return Else Clause as a StatementBlockAst
        public static StatementBlockAst GetElse (this IfStatementAst _ast)
        {
            // throw new NotImplementedException("Not implemented at the moment");
            if ( _ast.ElseClause != null ) {
                return _ast.ElseClause;
            }
            return null;

        }

        // Return ElseIf Clauses as a list of StatementBlockAst
        public static IEnumerable<StatementBlockAst> GetElseIf (this IfStatementAst _ast)
        {
            if (_ast.Clauses.Count > 1)
            {
                List<StatementBlockAst> ElseIfs = new List<StatementBlockAst>();
                for (int i = 1; i < _ast.Clauses.Count; i++)
                {
                    ElseIfs.Add(_ast.Clauses[i].Item2);
                }

                return ElseIfs;
            }
            return null;
        }

    }

    public static class SwitchStatementExtensions
    {
        // SwitchStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition, Parent) => Creates a Node
        // - GetChildAst() => retourne only ASTs we are looking for ...
        public static SwitchNode CreateNodeFromAst(this SwitchStatementAst _ast,int _depth, int _position, Node _parent, Tree _tree)
        {
            return new SwitchNode(_ast,_depth, _position, _parent, _tree);
        }

        // Return Else Clause as a StatementBlockAst
        public static StatementBlockAst GetDefault (this SwitchStatementAst _ast)
        {
            // throw new NotImplementedException("Not implemented at the moment");
            if ( _ast.Default != null ) {
                return _ast.Default;
            }
            return null;

        }

        // Return ElseIf Clauses as a list of StatementBlockAst
        public static IEnumerable<StatementBlockAst> GetCases (this SwitchStatementAst _ast)
        {
            if (_ast.Clauses.Count > 1)
            {
                List<StatementBlockAst> Cases = new List<StatementBlockAst>();
                for (int i = 0; i < _ast.Clauses.Count; i++)
                {
                    Cases.Add(_ast.Clauses[i].Item2);
                }

                return Cases;
            }
            return null;
        }

    }
    
    public static class WhileStatementExtensions {
        // WhileStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static WhileNode CreateNodeFromAst(this WhileStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            return new WhileNode(_ast,_depth, _position, _parent, _tree);
        }

        // public static IEnumerable<Ast> GetChildAst (this WhileStatementAst _ast)
        // {
        //     return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        // }

        public static IEnumerable<Ast> GetChildAst (this WhileStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && Args.Parent == _ast.Body, false);
        }

    }

    public static class DoWhileStatementExtensions {
        // DoWhileStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static DoWhileNode CreateNodeFromAst(this DoWhileStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            return new DoWhileNode(_ast,_depth, _position, _parent, _tree);
        }

        // public static IEnumerable<Ast> GetChildAst (this DoWhileStatementAst _ast)
        // {
        //     return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        // }
        public static IEnumerable<Ast> GetChildAst (this DoWhileStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && Args.Parent == _ast.Body, false);
        }

    }

    public static class DoUntilStatementExtensions {
        // DoUntilStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static DoUntilNode CreateNodeFromAst(this DoUntilStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            return new DoUntilNode(_ast,_depth, _position, _parent, _tree);
        }

        // public static IEnumerable<Ast> GetChildAst (this DoUntilStatementAst _ast)
        // {
        //     return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        // }
        public static IEnumerable<Ast> GetChildAst (this DoUntilStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && Args.Parent == _ast.Body, false);
        }

    }

    public static class TryStatementExtensions {
        // TryStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static TryNode CreateNodeFromAst(this TryStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            return new TryNode(_ast,_depth, _position, _parent, _tree);
        }

        // public static IEnumerable<Ast> GetChildAst (this TryStatementAst _ast)
        // {
        //     return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        // }
        public static IEnumerable<Ast> GetChildAst (this TryStatementAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && Args.Parent == _ast.Body, false);
        }

        public static IEnumerable<Ast> GetCatch (this TryStatementAst _ast)
        {
            return _ast.FindAll(Args => Args is CatchClauseAst && Args.Parent == _ast, false);
        }

        public static StatementBlockAst GetFinally (this TryStatementAst _ast)
        {
            // throw new NotImplementedException("Not implemented at the moment");
            if ( _ast.Finally != null ) {
                return _ast.Finally;
            }
            return null;

        }
    }

    public static class CatchStatementExtensions {
        // TryStatementAst Extension Methods
        // New Methods Available:
        // - CreateNodeFromAST(NodeDepth, NodePosition) => Creates a Node
        // - CreateChildNodes ($item in $collection) {} => Creates Child Nodes
        public static CatchNode CreateNodeFromAst(this CatchClauseAst _ast, int _depth, int _position, Node _parent)
        {
            return new CatchNode(_ast,_depth, _position, _parent);
        }

        // public static IEnumerable<Ast> GetChildAst (this CatchClauseAst _ast)
        // {
        //     return _ast.Body.FindAll(Args => Args is Ast && FlowChartCore.Utility.GetValidTypes().Contains(Args.GetType()) && Args.Parent == _ast.Body, false);
        // }

        public static IEnumerable<Ast> GetChildAst (this CatchClauseAst _ast)
        {
            return _ast.Body.FindAll(Args => Args is Ast && Args.Parent == _ast.Body, false);
        }

    }

}