using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class IfNode : Node
    {
        protected IfStatementAst RawAst { get; set; }
        // public IfStatementAst ShowAst {get => RawAst;}
        public override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}
        public override int OffSetScriptBlockStart {get => RawAst.Clauses[0].Item2.Extent.StartOffset-OffSetToRemove+1;}
        public override int OffSetScriptBlockEnd {get => RawAst.Clauses[0].Item2.Extent.EndOffset-OffSetToRemove-1;}
        public override int OffSetGlobalEnd {get => RawAst.Extent.EndOffset-OffSetToRemove+1;}
        // Constructor
        public IfNode(IfStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "IfNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            ast = _ast;
            parentroot = _tree;

            SetOffToRemove();
            SetChildren();
            
            
        }

        internal override void SetChildren()
        {

            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST
            IEnumerable<Ast> Childs = RawAst.GetChildAst();
            int p = 0;
            bool tmp = true;

            foreach (var item in Childs)
            {
                if (tmp && !FlowChartCore.Utility.GetValidTypes().Contains(item.GetType()) ) {
                    children.Add(new CodeNode(depth+1,p,this,null));
                    tmp = false;
                    p++;
                }
                else if(FlowChartCore.Utility.GetValidTypes().Contains(item.GetType())){
                    // On appelle CreateNode qui est une extension pour AST
                    children.Add(item.CreateNode(depth+1,p,this,null));
                    tmp = true;
                    p++;
                }
            }

            // if children is empty create pseudo codeblock
            if(children.Count == 0 ) {
                CreateCodeNode(p);
                p++;
            }

            // Recuperaction des Elsefis
            IEnumerable<StatementBlockAst> ElseIfs = RawAst.GetElseIf();
            if (ElseIfs != null)
            {
                foreach (var item in ElseIfs)
                {
                    // On appelle CreateNode qui est une extension de StatementBlockAST, avec un param enum qui identifie le elseif
                    children.Add(item.CreateNode(depth + 1, p, this, FlowChartCore.StatementType.ElseIf));
                    p++;
                }
            }

            // Recuperaction du Else
            StatementBlockAst Else = RawAst.GetElse();
            // On appelle CreateNode qui est une extension de StatementBlockAST, avec un param enum qui identifie le else
            if (Else != null)
            {
                children.Add(Else.CreateNode(depth + 1, p, this, FlowChartCore.StatementType.Else));
            }
        }

        public override void GenerateGraph(bool recursive){
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.IfBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                Console.WriteLine("recurse..");
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

        public override String GetEndId() {
            return $"end_{Id}";
        }

         
    }
}