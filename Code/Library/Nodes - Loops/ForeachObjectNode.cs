using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;
using System.Linq;

namespace FlowChartCore
{
    public class ForeachObjectNode : Node
    {
        protected PipelineAst RawAst {get;set;}
        protected NamedBlockAst ScriptBlock {get;set;}
        public string Label { get => label;}
        protected internal string condition;
        public string Condition { get => condition; }
        internal override int OffSetStatementStart {get => ScriptBlock.Extent.StartOffset-OffSetToRemove;}
        internal override int OffSetScriptBlockStart {get => ScriptBlock.Extent.StartOffset-OffSetToRemove;}
        internal override int OffSetScriptBlockEnd {get => ScriptBlock.Extent.EndOffset-OffSetToRemove;}

        public ForeachObjectNode(PipelineAst _ast, int _depth, int _position, Node _parent, Tree _tree, CommandElementAst _scriptblock)
        {
            name = "ForeachObjectNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            var plop = (ScriptBlockExpressionAst)_scriptblock;
            ScriptBlock = plop.ScriptBlock.EndBlock;
        
            parentroot = _tree;

            SetOffToRemove();
            SetCondition();
            SetChildren();
            CreateCodeNode(0);
            
        }


        internal override void SetChildren() {

            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST
            IEnumerable<Ast> Childs = ScriptBlock.FindAll(Args => Args is Ast && Args.Parent == ScriptBlock, false);
            
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
                    Node node = item.CreateNode(depth+1,p,this,null);
                    // if (null == node && tmp) {
                    if (null == node) {
                        if ( tmp )
                        {
                            children.Add(new CodeNode(depth+1,p,this,null));
                            tmp = false;
                        }
                        
                    } else {
                        children.Add(item.CreateNode(depth+1,p,this,null));
                        tmp = true;
                    }
                    p++;
                }
            }
        }

        public override void GenerateGraph(bool recursive){
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ForeachObjectBuilder(this);
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

        public Ast GetAst() {
            return RawAst;
        }

        public IEnumerable<Ast> plop() {
            return ScriptBlock.FindAll(Args => Args is Ast && Args.Parent == ScriptBlock, false);
        }

        internal override void SetCondition(){
            
            // something like Get-Service | Foreach-Object {}...
            if ( RawAst.PipelineElements[0] is CommandAst ) {
                CommandAst FirstPipeLineElement = (CommandAst)RawAst.PipelineElements[0];
                condition = FirstPipeLineElement.GetCommandName();
            }

            // something like 1..100 | Foreach-Object {}...
            if ( RawAst.PipelineElements[0] is CommandExpressionAst ) {
                CommandExpressionAst FirstPipeLineElement = (CommandExpressionAst)RawAst.PipelineElements[0];
                condition = FirstPipeLineElement.Extent.Text;
            }
            
        }

    }
}