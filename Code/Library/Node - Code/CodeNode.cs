using System.Management.Automation.Language;
using System.Collections.Generic;
using ExtensionMethods;
using System;

namespace FlowChartCore
{
    public class CodeNode : Node
    {
        public CodeNode(int _depth, int _position, Node _parent, Tree _tree)
        {
            name = "CodeNode";
            position = _position;
            depth = _depth;
            parent = _parent;
            parentroot = _tree;
            
        }

        public override void GenerateGraph(bool recursive){
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.CodeNodeBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override String GetEndId() {
            return Id;
        }

        public string discovercode(){
            String racine = GetRootNode().parentroot.Ast.Extent.Text;
            int a = 0;
            int b = 0;

            if (IsFirst && !IsLast)
            {
                // parent offsetscriptblockstart +1 && check getnextnode type
                a = parent.OffSetScriptBlockStart;
                if(GetNextNode() is ElseIfNode || GetNextNode() is ElseNode || GetNextNode() is CatchNode ) {
                    b = parent.OffSetScriptBlockEnd;
                } else {
                    b = GetNextNode().OffSetStatementStart;
                }

                // Console.WriteLine($"F: {a}, N: {b}");
                // Console.WriteLine($"IsFirst & not IstLast, F: {a}, N: {b}");
                return racine.Substring(a, b - a).Trim();
            }

            if (IsLast && !IsFirst)
            {
                // getpreviousnode offsetscriptblockend +1 && get offsetscriptblockend du parent
                //si le previous est un try, un if ou un switch, il faut taper sur le offsetglobalend
                Node previousnode = GetPreviousNode();
                if (previousnode is TryNode || previousnode is IfNode || previousnode is SwitchNode )
                {
                    a = previousnode.OffSetGlobalEnd;    
                } else {
                    a = GetPreviousNode().OffSetScriptBlockEnd;
                }
                
                b = parent.OffSetScriptBlockEnd;
                // Console.WriteLine($" not IsFirst & IstLast, F: {a}, N: {b}");
                return racine.Substring(a, b - a).Trim();
            }

            if (IsFirst && IsLast)
            {
                //
                a = parent.OffSetScriptBlockStart;
                b = parent.OffSetScriptBlockEnd;
                // Console.WriteLine($"IsFirst & IstLat, F: {a}, N: {b}");
                return racine.Substring(a, b - a).Trim();
            }

            if(!IsFirst && !IsLast)
            {
                // getprevious offsetscriptblockend+1 && getnextnode offsetstatementstart -1
                Node previousnode = GetPreviousNode();
                if (previousnode is TryNode || previousnode is IfNode || previousnode is SwitchNode )
                {
                    a = previousnode.OffSetGlobalEnd;    
                } else {
                    a = GetPreviousNode().OffSetScriptBlockEnd;
                }
                b = GetNextNode().OffSetStatementStart;
                // Console.WriteLine($"not IsFirst & not IstLast, F: {a}, N: {b}");
                return racine.Substring(a, b - a).Trim();
            }

            return null;
            
        }

    }
}