using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class FinallyNode : Node
    {
        protected StatementBlockAst RawAst {get;set;}
        public override int OffSetScriptBlockStart {get => RawAst.Extent.StartOffset-OffSetToRemove+1;}

        public FinallyNode(StatementBlockAst _ast, int _depth, int _position, Node _parent)
        {
            name = "FinallyNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            SetOffToRemove();
            SetChildren();
            CreateCodeNode(0);
            
        }

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
            Console.WriteLine("prout ahaha");
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.FinallyBuilder(this);
            Graph.AddRange(x.DotDefinition);
            Console.WriteLine("prout");

            if(recursive) {
                Console.WriteLine("recurse..");
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

    }
}