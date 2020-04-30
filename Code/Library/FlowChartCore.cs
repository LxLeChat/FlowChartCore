// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FlowChartCore
{
    public enum StatementType{
        Else,ElseIf,SwitchCase,SwitchDefault,Finally
    }

    public class Tree {
        public List<Node> Nodes { get; private set; }

        public Tree (List<Node> nodes) {
            Nodes = nodes;
        }

// Method to find recursively Nodes by Type
        public virtual IEnumerable<Node> FindNodesByType (Type type) {
            List<Node> Result = new List<Node>();
            if (Nodes.Count > 0 ) {
                foreach ( var child in Nodes ) {
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

    public class Node
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
        protected internal Tree parentroot;
        public Tree ParentRoot { get=> parentroot; }

        // public LinkedList<Node> LList { get; set; }

        internal virtual void SetChildren() {}


        // Method to find recursively Nodes by Type
        public virtual IEnumerable<Node> FindNodesByType (Type type) {
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
        
        // method to find index of node in parent
        // parent can be parent property, or ParentRoot if
        // the current node depth is 0
        public virtual int FindIndex(){
            
            if (Depth == 0)
            {
                return ParentRoot.Nodes.FindIndex(x => x == this);
            } else
            {
                return parent.Children.FindIndex(x => x == this);    
            }
        }

        // Method to return the next node object from the parent
        public virtual Node GetNextNode(){
            
            int CurrIndex = FindIndex();
            if (Depth == 0)
            {
                try {
                    Node NextNode = ParentRoot.Nodes[CurrIndex+1];
                    return NextNode;
                }
                catch {
                    return null;
                }
                
            } else
            {
                try {
                    Node NextNode = parent.Children[CurrIndex+1];
                    return NextNode;
                }
                catch {
                    return null;
                }
            }
        }

        // Method to return the next node object from the parent
        public virtual Node GetPreviousNode(){
            
            int CurrIndex = FindIndex();
            if (Depth == 0)
            {
                try {
                    Node NextNode = ParentRoot.Nodes[CurrIndex-1];
                    return NextNode;
                }
                catch {
                    return null;
                }
                
            } else
            {
                try {
                    Node NextNode = parent.Children[CurrIndex-1];
                    return NextNode;
                }
                catch {
                    return null;
                }
            }
        }

    }
}

