using System.Management.Automation.Language;
using System.Management.Automation;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace FlowChartCore
{
    public class BreakNode : Node
    {
        protected BreakStatementAst RawAst {get;set;}
        public string Label { get => label;}
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}

        public BreakNode(BreakStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "BreakNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;
            SetLabel();
            // fix issue #32
            SetOffToRemove();
            
        }

        // Set Label
        internal void SetLabel () {
            if (RawAst.Label != null)
            {
                label = RawAst.Label.ToString();

                if (Regex.IsMatch(label,@"\$"))
                {
                    // fix issue #17
                    // Make sure the label is not assigned to a variable
                    // Break can have a child ast, if its a variableexpression ast then
                    // the break look like that: break $somevar
                    // $somevar is assigned somewhere in the script or file

                    // looking for the PossibleVar from the root
                    Ast PossibleVar =  RawAst.Parent.Find(x=> x is VariableExpressionAst, true);
                    
                    if (PossibleVar != null)
                    {
                        //we need to cast PossibleVar as a variableExpressionAst
                        // VariablePath contains the name of the variable
                        VariableExpressionAst SeriousVar = (VariableExpressionAst)PossibleVar;
                        string PsBreakVariable = "$" + SeriousVar.VariablePath;
                        
                        // then we need to find all assigments variables from the tree
                        IEnumerable<Ast> Variables =  GetRootNode().parentroot.Ast.FindAll(x => x is AssignmentStatementAst, true);

                        foreach (AssignmentStatementAst item in Variables)
                        {
                            // if the variable left side is == PsBreakVariable
                            // it's the variable we are looking for..
                            if (item.Left.Extent.Text == PsBreakVariable){
                                StringConstantExpressionAst labelvar = (StringConstantExpressionAst)item.Find(x => x is StringConstantExpressionAst,false);
                                label = labelvar.Value;
                                break;
                            }
                        }

                    }
                }

            }
        }

        public override String GetEndId() {
            return Id;
        }

        public override void GenerateGraph(bool recursive){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.BreakBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.BreakBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText, PowerShell PSinstance){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.BreakBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }


        // return the rawast
        public override Ast GetAst() {
            return RawAst;
        }


    }
}