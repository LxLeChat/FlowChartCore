using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class ElseIfNode : Node
    {
        protected StatementBlockAst RawAst {get;set;}
        // public StatementBlockAst plop { get => RawAst;}

        public ElseIfNode(StatementBlockAst _ast, int _depth, int _position, Node _parent)
        {
            name = "ElseIfNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            SetChildren();
            CreateCodeNode(0);
            
        }

        // internal override void SetChildren(){}
        internal override void SetChildren() {
            
            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST
            IEnumerable<Ast> Childs = RawAst.GetChildAst();
            
            int p = 0;
            foreach (var item in Childs)
            {
                // On appelle CreateNode qui est une extension pour AST
                children.Add(item.CreateNode(depth+1,p,this,null));
                p++;
            }
        }

        public override void GenerateGraph(bool recursive){
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ElseIfBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

        public override String GetEndId() {
            return parent.GetEndId();
        }

    }
}