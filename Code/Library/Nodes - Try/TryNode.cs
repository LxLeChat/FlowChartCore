using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class TryNode : Node
    {
        protected TryStatementAst RawAst {get;set;}
        // public TryStatementAst plop { get => RawAst;}

        public TryNode(TryStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "TryNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;

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
                children.Add(item.CreateNode(depth+1,p,this,null));
                p++;
            }

            // si pas de chil de type identifier on met un codeblock
            CreateCodeNode(0);
            p++;

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
                Console.WriteLine("recurse..");
                foreach (var child in Children) {
                    child.GenerateGraph(recursive);
                }
            }
        }
    }
}