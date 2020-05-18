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
        protected internal string condition;
        public string Condition { get => condition; }
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}
        internal override int OffSetScriptBlockStart {get => RawAst.Body.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Body.Extent.EndOffset-OffSetToRemove-1;}

        public ForeachNode(ForEachStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "ForeachNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;

            SetOffToRemove();
            SetLabel();
            SetCondition();
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
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ForeachBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }

        public override String GetEndId() {
            return $"loop_{Id}";
        }

        public ForEachStatementAst GetAst() {
            return RawAst;
        }

        internal override void SetCondition(){
            condition = RawAst.Condition.Extent.Text;
        }

    }
}