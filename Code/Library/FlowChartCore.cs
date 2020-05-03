﻿// using System.Management.Automation.Language;
using System.Collections.Generic;
using System.Linq;
using System;
using DotNetGraph.Core;

namespace FlowChartCore
{
    public enum StatementType{
        Else,ElseIf,SwitchCase,SwitchDefault,Finally
    }

    public enum GraphType{
        Dot,Mmd
    }

    public class Tree {
        public List<Node> Nodes { get; private set; }

        public Tree (List<Node> nodes) {
            Nodes = nodes;
        }

        // Method to find recursively Nodes by Type
        public virtual IEnumerable<Node> FindNodesByType (Type type, bool recurse) {
            List<Node> Result = new List<Node>();
            if (Nodes.Count > 0 ) {
                foreach ( var child in Nodes ) {
                    if (child.GetType() == type )
                    {
                        Result.Add(child);

                        if (recurse)
                        {
                            Result.AddRange(child.FindNodesByType(type,recurse));
                        }

                    } else {
                        if (recurse)
                        {
                            Result.AddRange(child.FindNodesByType(type,recurse));
                        }
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
        // public Tree ParentRoot { get=> parentroot; }
        protected internal string label;
        public String Id { get=> GetId(); }
        public bool IsLast { get=> GetIsLast(); }
        public bool IsFirst { get=> GetIsFirst(); }

        // public List<string> Graph { get; set; }
        public List<IDotElement> Graph = new List<IDotElement>();
        
        // Method to find children..
        // Must be overriden
        internal virtual void SetChildren() {}

        // Method to find recursively Nodes by Type
        public virtual IEnumerable<Node> FindNodesByType (Type type, bool recurse) {
            List<Node> Result = new List<Node>();
            if (children.Count > 0 ) {
                foreach ( var child in children ) {
                    if (child.GetType() == type )
                    {
                        Result.Add(child);

                        if (recurse)
                        {
                            Result.AddRange(child.FindNodesByType(type,recurse));
                        }

                    } else {
                        if (recurse)
                        {
                            Result.AddRange(child.FindNodesByType(type,recurse));
                        }
                    }
                }
            }
            return Result;
        }
        
        // Method to find a node by type UpWard.
        // Stops When a corresponding node is found
        public virtual Node FindNodesByTypeUp (Type type) {
            
            if ( this.parent != null ) {
                if (this.Parent.GetType() == type)
                {
                    return this.Parent;
                } else {
                    return this.Parent.FindNodesByTypeUp(type);
                }
            }
            return null;
        }
        
        // Method to find a node by type & label UpWard.
        // Stops When a corresponding node is found
        public virtual Node FindNodesByLabelUp (String Label) {
            
            if ( this.parent != null ) {
                if (this.parent.label == Label)
                {
                    return this.Parent;
                } else {
                    return this.Parent.FindNodesByLabelUp(Label);
                }
            }
            return null;
        }
        
        // method to find index of node in parent
        // parent can be parent property, or ParentRoot if
        // the current node depth is 0
        public virtual int FindIndex(){
            
            if (depth == 0)
            {
                return parentroot.Nodes.FindIndex(x => x == this);
            } else
            {
                return parent.Children.FindIndex(x => x == this);    
            }
        }

        // Method to return the next node object from the parent
        public virtual Node GetNextNode(){
            
            int CurrIndex = FindIndex();
            if (depth == 0)
            {
                try {
                    Node NextNode = parentroot.Nodes[CurrIndex+1];
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
            if (depth == 0)
            {
                try {
                    Node NextNode = parentroot.Nodes[CurrIndex-1];
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

        // Method to create the node Id
        internal string GetId() {
            string id = depth.ToString("D2") + position.ToString("D2");
            return id;
        }

        // Method for IsLast property
        internal bool GetIsLast() {
            if (GetNextNode() == null) {
                return true;
            } else {
                return false;
            }
        }

        // Method for IsFirst property
        internal bool GetIsFirst() {
            if (GetPreviousNode() == null) {
                return true;
            } else {
                return false;
            }
        }

        public virtual String GetEndId() {
            return $"end_{Id}";
        }

        internal void CreateCodeNode(){
            if ( children.Count == 0 ) {
                children.Add(new CodeNode(depth+1,0,this,null));
            }
        }

        public virtual void GenerateGraph(bool recursive){}

    }

}

