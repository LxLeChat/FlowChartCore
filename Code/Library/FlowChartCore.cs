// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FlowChartCore
{
    public enum StatementType{
        Else,ElseIf,SwitchCase,SwitchDefault,Finally
    }

    public abstract class Node
    {
        protected internal string name;
        public string Name { get => name; }
        protected internal List<Node> children = new List<Node>();
        public List<Node> Children { get => children; }
        protected internal Node parent;
        public Node Parent { get=> parent; }
        protected internal int position;
        public int Position { get=> position; }
        protected internal int depth;
        public int Depth { get=> depth; }

        internal abstract void SetChildren();


        // Method to find recursively Nodes by Type
        public IEnumerable<Node> FindNodesByType (Type type) {
            List<Node> Result = new List<Node>();
            if (children.Count > 0 ) {
                foreach ( var child in children ) {
                    if (child.GetType() == type )
                    {
                        Result.Add(child);
                        Result.AddRange(child.FindNodesByType(type));
                    } else {
                        Result.AddRange(child.FindNodesByType(type));
                    }
                }
            }
            return Result;
        }
        

    }
}
