using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class CatchNode : Node
    {
        protected CatchClauseAst RawAst {get;set;}
        // public CatchClauseAst plop { get => RawAst;}

        public CatchNode(CatchClauseAst _ast, int _depth, int _position, Node _parent)
        {
            name = "CatchNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;

            SetChildren();
            
        }

        internal override void SetChildren() {
            // On appelle GetChildAST qui est une extension pour le type
            // Ca nous retourne une liste d'AST
            IEnumerable<Ast> Childs = RawAst.GetChildAst();
            int p = 0;
            foreach (var item in Childs)
            {
                // On appelle CreateNode qui est une extension pour AST
                children.Add(item.CreateNode(Depth+1,p,this,null));
                p++;
            }
        }

    }
}