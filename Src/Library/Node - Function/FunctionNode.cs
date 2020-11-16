using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class FunctionNode : Node
    {
        protected FunctionDefinitionAst RawAst {get;set;}
        public string FunctionName { get => RawAst.Name;}
        internal override int OffSetStatementStart {get => RawAst.Extent.StartOffset-OffSetToRemove;}
        internal override int OffSetScriptBlockStart {get => RawAst.Body.Extent.StartOffset-OffSetToRemove+1;}
        internal override int OffSetScriptBlockEnd {get => RawAst.Body.Extent.EndOffset-OffSetToRemove;}

        public FunctionNode(FunctionDefinitionAst _ast, int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "FunctionNode";
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
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.FunctionNodeBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            GenerateGraph(recursive);
        }

    }
}