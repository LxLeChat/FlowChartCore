using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class ElseNode : Node
    {
        protected StatementBlockAst RawAst {get;set;}
        // public StatementBlockAst ShowAst { get => RawAst;}
        internal override int OffSetScriptBlockStart {get => RawAst.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Extent.EndOffset-OffSetToRemove-1;}

        public ElseNode(StatementBlockAst _ast, int _depth, int _position, Node _parent)
        {
            name = "ElseNode";
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
                // p++;
            }
        }

        public override void GenerateGraph(bool recursive){
            // FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.IfBuilder(this);
            // Graph.AddRange(x.DotDefinition);
            // on graph rien ... ou pas ... si on graph pas
            // il faudra ajouter une condition a chaque builder,
            // en disant si le parent est un else, et que c'est le dernier enfant,
            // alors on draw le end edge vers la fin du if ...
            // ou alors le graph du else, ne buildera un edge que depuis le dernier enfant
            // vers la fin du if ...

            // FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ElseBuilder(this);
            // Graph.AddRange(x.DotDefinition);

            if(recursive) {
                Console.WriteLine("recurse..");
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