using System.Management.Automation.Language;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace FlowChartCore
{
    public class ReturnNode : Node
    {
        protected ReturnStatementAst RawAst {get;set;}
        public string Pipeline { get => RawAst.Pipeline.Extent.Text;}
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}

        public ReturnNode(ReturnStatementAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "ReturnNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            parentroot = _tree;
            SetOffToRemove();
            
        }

        public override String GetEndId() {
            return Id;
        }

        public override void GenerateGraph(bool recursive){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ReturnBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ReturnBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }


        // return the rawast
        public override Ast GetAst() {
            return RawAst;
        }


    }
}