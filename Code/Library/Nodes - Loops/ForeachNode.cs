using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class ForeachNode : Node
    {
        protected ForEachStatementAst RawAst {get;set;}
        public string Label { get => label;}

        public ForeachNode(ForEachStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "ForeachNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;

            SetLabel();
            SetChildren();
            CreateCodeNode(0);
            
        }

        // Set Label
        internal void SetLabel () {
            label = RawAst.Label;
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
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ForeachBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                Console.WriteLine("recurse..");
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

        public override String GetEndId() {
            return $"loop_{Id}";
        }

    }
}