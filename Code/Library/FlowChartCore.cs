using System.Management.Automation.Language;
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
        private Ast _ast {get;set;}
        public Ast Ast {get=> _ast;}

        public Tree (List<Node> nodes,Ast ast) {
            Nodes = nodes;
            _ast = ast;
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
        public Tree ParentRoot { get=> parentroot; }
        protected internal string label;
        public String Id { get=> GetId(); }
        public bool IsLast { get=> GetIsLast(); }
        public bool IsFirst { get=> GetIsFirst(); }
        public virtual int OffSetStatementStart {get;set;}
        public virtual int OffSetScriptBlockStart {get;set;}
        public virtual int OffSetScriptBlockEnd {get;set;}
        public virtual int OffSetGlobalEnd {get;set;}
        public int OffSetToRemove { get; set;}

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

        // Method to find recursively Nodes by Id
        public virtual Node FindNodesById (String id, bool recurse) {
            Node result = new Node();
            if (children.Count > 0 ) {
                foreach ( var child in children ) {
                    if (child.Id == id )
                    {
                        return child;

                    } else {
                        if (recurse)
                        {
                            return child.FindNodesById(id,recurse);
                        }
                    }
                }
            }
            return result ;
        }
    

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
        
        // Find Depth 0 Node
        public Node GetRootNode() {
            if (this.Parent.Depth == 0)
            {
                return this.Parent;
            } else {
                return this.Parent.GetRootNode();
            }
        }
        
        // faut refaire toutes les autres ... et du coup on aura juste 2 méthodes
        // FindNodes(Predicate<Node> predicate, bool recurse) & FindNodesUp(Predicate<Node> predicate, bool recurse)
        // et on pourra effacer toutes les autres méthodes .. ! EXCELLENT !

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

        // Method to find a node by predicate DownWard.
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
            return null;
        }

        // Method to find a node by type UpWard.
        // Stops When a corresponding node is found
        internal Node FindNodesByTypeUp (Type type) {
            
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

        internal void CreateCodeNode(int position){
            if ( children.Count == 0 ) {
                children.Add(new CodeNode(depth+1,position,this,null));
            }
        }

        public virtual void GenerateGraph(bool recursive){}

        internal virtual void SetCondition(){}
        

    }

}

