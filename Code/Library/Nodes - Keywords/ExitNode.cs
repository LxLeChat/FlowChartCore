using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class ExitNode : Node
    {
        protected ExitStatementAst RawAst {get;set;}
        public string Label { get => label;}

        public ExitNode(ExitStatementAst _ast, int _depth, int _position, Node _parent)
        {
            name = "ExitNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            RawAst = _ast;
            
        }

        public override String GetEndId() {
            return Id;
        }

        public override void GenerateGraph(bool recursive){

            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.ExitBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

    }
}