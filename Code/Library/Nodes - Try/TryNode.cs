using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class TryNode : Node
    {
        protected TryStatementAst RawAst {get;set;}
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}
        internal override int OffSetScriptBlockStart {get => RawAst.Body.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Body.Extent.EndOffset-OffSetToRemove-1;}
        internal override int OffSetGlobalEnd {get => RawAst.Extent.EndOffset-OffSetToRemove+1;}

        public TryNode(TryStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "TryNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;

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

                    // Because i introduced PipelineAST as a valid AST
                    // stuff like Write-Host will be present in Childs array ...
                    // Maybe it's not such a good idea to create node for Foreach-Object...
                    // Node childnode = item.CreateNode(depth+1,p,this,null); 
                    // if ( null != childnode)
                    // {
                    //     children.Add(childnode);
                    //     tmp = true;
                    //     p++;
                    // } else if(tmp) {
                    //     children.Add(new CodeNode(depth+1,p,this,null));
                    //     tmp = false;
                    //     p++;
                    // }
                }
            }

            // On Appelle GetCatch pour recup les clauses catch
            IEnumerable<Ast> CatchClauses = RawAst.GetCatch();
            foreach (var item in CatchClauses)
            {
                // On appelle CreateNode qui est une extension pour AST
                children.Add(item.CreateNode(depth+1,p,this,null));
                p++;
            }

            // On Appelle GetFinally pour recup le finally
            StatementBlockAst Finally = RawAst.GetFinally();
            if (Finally != null) {
                children.Add(Finally.CreateNode(depth+1,p,this,StatementType.Finally));
            }
            {
                
            }
        }

        // pour les catches il y a un CatchClauseAst

        public override void GenerateGraph(bool recursive){
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.TryBuilder(this);
            Graph.AddRange(x.DotDefinition);

            if(recursive) {
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }
    }
}