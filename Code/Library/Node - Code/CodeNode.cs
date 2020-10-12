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
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.CodeNodeBuilder(this);
            Graph.AddRange(x.DotDefinition);
        }

        public override void GenerateGraph(bool recursive, bool codeAsText){
            Graph.Clear();
            FlowChartCore.Graph.IBuilder x = new FlowChartCore.Graph.CodeNodeBuilder(this,true);
            Graph.AddRange(x.DotDefinition);
        }

        public override String GetEndId() {
            return Id;
        }


        public string discovercode(){
            // Fix: Bug when first node at position 1 & depth 0
            Node rootNode = GetRootNode() ?? this;
            String scriptText = rootNode.parentroot.Ast.Extent.Text;
            
            int a = 0;
            int b = 0;

            if (IsFirst && !IsLast)
            {
                // parent offsetscriptblockstart +1 && check getnextnode type
                if ( position == 1 )
                {
                    a = OffSetToRemove;
                } else {
                    a = parent.OffSetScriptBlockStart;
                }
                
                if(GetNextNode() is ElseIfNode || GetNextNode() is ElseNode || GetNextNode() is CatchNode ) {
                    b = parent.OffSetScriptBlockEnd;
                } else {
                    b = GetNextNode().OffSetStatementStart;
                }
                
                return scriptText.Substring(a, b - a).Trim();
                
            }

            if (IsLast && !IsFirst)
            {
                // getpreviousnode offsetscriptblockend +1 && get offsetscriptblockend du parent
                // si le previous est un try, un if ou un switch, il faut taper sur le offsetglobalend
                Node previousnode = GetPreviousNode();
                if (previousnode is TryNode || previousnode is IfNode || previousnode is SwitchNode )
                {
                    a = previousnode.OffSetGlobalEnd;    
                } else {
                    a = GetPreviousNode().OffSetScriptBlockEnd;
                }
                
                b = parent.OffSetScriptBlockEnd;
                return scriptText.Substring(a, b - a).Trim();
            }

            if (IsFirst && IsLast)
            {
                //
                a = parent.OffSetScriptBlockStart;
                b = parent.OffSetScriptBlockEnd;
                return scriptText.Substring(a, b - a).Trim();
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

                return scriptText.Substring(a, b - a).Trim();
            }

            return null;
            
        }

    }
}