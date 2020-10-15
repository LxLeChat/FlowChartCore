using System.Management.Automation.Language;
using System.Collections.Generic;
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
        private Ast _ast {get;set;}
        public Ast Ast {get=> _ast;}

        public Tree (List<Node> nodes,Ast ast) {
            Nodes = nodes;
            _ast = ast;
        }

                // Method to find a node by predicate.
        // Pwsh: FinNodes({$args[0] -is [FlowChartCore.IfNode]},$True)
        // will search all nodes of types Ifnode, recursively
        public IEnumerable<Node> FindNodes (Predicate<Node> predicate,bool recurse) {
            
            List<Node> Result = new List<Node>();
            if (Nodes.Count > 0 ) {
                foreach ( var child in Nodes ) {
                    if (predicate(child))
                    {
                        Result.Add(child);

                        if (recurse)
                        {
                            Result.AddRange(child.FindNodes(predicate,recurse));
                        }

                    } else {
                        if (recurse)
                        {
                            Result.AddRange(child.FindNodes(predicate,recurse));
                        }
                    }
                }
            }
            return Result;
        }
        
        
    }

    public class Node
    {
        // public Ast ast;
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
        internal Tree ParentRoot { get=> parentroot; }
        protected internal string label;
        public String Id { get=> GetId(); }
        internal bool IsLast { get=> GetIsLast(); }
        internal bool IsFirst { get=> GetIsFirst(); }
        internal virtual int OffSetStatementStart {get;set;}
        internal virtual int OffSetScriptBlockStart {get;set;}
        internal virtual int OffSetScriptBlockEnd {get;set;}
        internal virtual int OffSetGlobalEnd {get;set;}
        internal int OffSetToRemove { get; set;}


        // igraphElement
        public List<IDotElement> Graph = new List<IDotElement>();
        
        // Method to populate various offsets values
        protected virtual void SetOffToRemove () {
            // Set OffSetToRemove
            if (depth == 0 )
            {
                OffSetToRemove = parentroot.Ast.Extent.StartOffset;
            } else {
                OffSetToRemove = parent.OffSetToRemove;
            }
        }


        // Method to find children..
        // Must be overriden
        internal virtual void SetChildren() {}

        // Find Depth 0 Node
        public Node GetRootNode() {
            // Fix: Bug when first node at position 1 & depth 0
            if (depth == 0)
            {
                return null;
            }

            if (parent.Depth == 0)
            {
                return parent;
            } else {
                return parent.GetRootNode();
            }
        }
        

        // Method to find a node by predicate UpWard.
        // Stops When a corresponding node is found
        // predicate example: x => x.Label == "lol
        public Node FindNodesUp (Predicate<Node> predicate) {
            
            if ( this.parent != null ) {
                if (predicate(this.Parent))
                {
                    return this.Parent;
                } else {
                    return this.Parent.FindNodesUp(predicate);
                }
            }
            return null;
        }

        // Method to find a node by predicate.
        // Pwsh: FinNodes({$args[0] -is [FlowChartCore.IfNode]},$True)
        // will search all nodes of types Ifnode, recursively
        public IEnumerable<Node> FindNodes (Predicate<Node> predicate,bool recurse) {
            
            List<Node> Result = new List<Node>();

            if ( this.children.Count > 0 ) {
                Result.AddRange(this.children.FindAll(predicate));
                if(recurse)
                {
                    foreach (Node item in this.children)
                    {
                        Result.AddRange(item.FindNodes(predicate,recurse));
                    }
                }
            }
            return Result;
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

        // Return the id of the next node
        // if next node is else, elseif or catch, then the parent node id is returned
        public virtual string GetNextId(){
            
            int CurrIndex = FindIndex();

            if (depth == 0)
            {
                try {
                    Node NextNode = parentroot.Nodes[CurrIndex+1];
                    return NextNode.Id;
                }
                catch {
                    return "end_of_script";
                }
                
            } else
            {
                if ( IsLast ) {
                    return parent.GetEndId();
                } else {
                    // Fix issue #25
                    if ( GetNextNode() is FinallyNode & parent is TryNode) {
                        return parent.GetEndId();
                    }

                    switch (GetNextNode())
                    {
                        case ElseNode elsenode:
                            return parent.GetEndId();
                        case ElseIfNode elseIfnode:
                            return parent.GetEndId();
                        case CatchNode catchNode:
                            return parent.GetEndId();
                        default:
                            return GetNextNode().Id;
                    }
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
            
            if(parent !=null ) {
                return parent.Id + depth.ToString("D0") + position.ToString("D0");
            } else {
                return depth.ToString("D0") + position.ToString("D0");
            }
        }

        // Method for IsLast property
        // Return true if node is last
        internal bool GetIsLast() {
            if (GetNextNode() == null) {
                return true;
            } else {
                return false;
            }
        }

        // Method for IsFirst property
        // Return true if node is first
        internal bool GetIsFirst() {
            if (GetPreviousNode() == null) {
                return true;
            } else {
                return false;
            }
        }

        // Return an end_id
        public virtual String GetEndId() {
            return $"end_{Id}";
        }

        internal void CreateCodeNode(int position){
            if ( children.Count == 0 ) {
                children.Add(new CodeNode(depth+1,position,this,null));
            }
        }

        public virtual void GenerateGraph(bool recursive){
        }
        public virtual void GenerateGraph(bool recursive,bool codeBlockAsText){}
        internal virtual void SetCondition(){}
        

    }

}

