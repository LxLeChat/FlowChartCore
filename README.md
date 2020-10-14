# FlowChartCore
The main goal is to document Powershell Scripts. The Module will allow you to generate a dot graph definition. This definition can then be consumed by GraphViz to generate a nice flow chart of your script
Since i'm discovring a lot of bugs, and strange behavior, the module is not published in the PSGallery at the moment.

It's the first time i'm writing something in c# ... jumping from Powershell... so, i'm sorry if the code is not as clean as excpected..! I know i'm missing tests for example..

### Why ?
First because i had a bad experience at a client, who asked me to add new functiunalities to a HUGE script (actually it was 3 gigantics scripts... anyway... ). Every modification i made had some impacts on other scripts... so i was tired every time to look a all these scripts... and thought ... maybe having a graph representation might make my life easier !

### What it is for? What it is not for ?
Mainly for helping and documentation purpose.
The module will play nicely with pure PS1 scripts.
```
If ( $x ) {
  Foreach ( $y in $x ) {
    "Do Something"
  }
}
"SomeThing Else"
```

### Caveats
At the moment, the script will not discover what's inside scriptblocks. For example, if you script is a massive ``Invoke-Command``, the module will discover a simple ``CodeNode`` and the resulting graph will contain only one ``CodeBlock`` (more on nodes in the readme). Same thing for ``Foreach-Object``, ``Where-Object`` ...
```
Invoke-Command -ComputerName Computer1 -ScriptBlock {
  If ( $x ) {
    Foreach ( $y in $x ) {
      "Do Something"
    }
  }
}
```
Same thing for the following notations:
```
$SomeVar = If ($x) { Foreach ($y in $x) { "Do SOmething" } }
```

Also, the script will not parse function definition, so if you pass a psm1 file, with only definition this will not be great..!
Maybe at some point i'll find a way to deal with these... I have an idea but i dont know if it will render well..  

### Nodes?
When you parse a scriptblock or a script using ``Find-FlowChartNodes``, the cmdlet will return a list of nodes.
So what is a node?
A node is a class, representing a know Statement. For example, a If Statement, will return a ``IfNode``. If there is a ``Foreach`` statement in this ``If``statement, a ``ForeachNode`` will be created and added to the parent ``IfNode`` children.
So there is a notion of Parent and Children. Essentially, the module is recreating a tree from the script.
```
PS >$Sb = {
  If ( $x ) {
    Foreach ( $y in $x ) {
      "Do Something"
    }
  }
}
PS >$x = Find-FlowChartNodes -ScriptBlock $Sb
PS >$x
Condition : $x
Name      : IfNode
Children  : {ForeachNode}
Parent    :
Position  : 1
Depth     : 0
Id        : 01
Graph     : {}

PS >$x[0].Children
Label     :
Condition : $x
Name      : ForeachNode
Children  : {CodeNode}
Parent    : FlowChartCore.IfNode
Position  : 0
Depth     : 1
Id        : 0110
Graph     : {}

```

I also have implemented a method that will help you search for some type of node based on a predicate. For Example let's find all ``ForeachNode``:
```
PS > $x.FindNodes
OverloadDefinitions
-------------------
System.Collections.Generic.IEnumerable[FlowChartCore.Node] FindNodes(System.Predicate[FlowChartCore.Node] predicate, bool recurse)

PS >$x.FindNodes({$args[0] -is [FLowChartCore.ForeachNode]},$true)
Label     :
Condition : $x
Name      : ForeachNode
Children  : {CodeNode}
Parent    : FlowChartCore.IfNode
....
```


# Working with the module
The module is cross platform, so it works on Windows Powershell, and PSCore (tested on WSL).
Since it's not published in the PSGallery, either fork the project or download it as a zip.

### Importing the module
You have to import the ``FlowChartCore.dll`` wich can be found in ``Code\bin\debug\netstandard2.0\FlowChartCore.dll``
```
PS >Import-module .\Flowchartcore.dll
PS > Get-Module -Name FlowchartCore

ModuleType Version    Name                                ExportedCommands
---------- -------    ----                                ----------------
Binary     1.0.0.0    FlowchartCore                       {Find-FLowChartNodes, New-FLowChartGraph}
```

### Find-FlowChartNodes
The cmdlet will find nodes in a file (a ps1) or a scriptblock.
```
PS >$Sb = {
  If ( $x ) {
    Foreach ( $y in $x ) {
      "Do Something"
    }
  }
}

PS > Find-FlowChartNodes -ScriptBlock $Sb
Condition : $x
Name      : IfNode
Children  : {ForeachNode}
Parent    :
Position  : 1
Depth     : 0
Id        : 01
Graph     : {}
```

# New-FlowChartGraph
The cmdlet will return a dot definition. It Takes a List of nodes as input.
There is a parameter ``-codeastext`` that will render discover the code for a given ``codenode`` and display it instead of just a ``codeblock``. We will this the difference in the examples.
```
PS > $x=Find-FLowChartNodes -ScriptBlock $sb
PS > New-FLowChartGraph -Nodes $x
digraph "a" {
        "01"[label="If $x",shape=diamond];
        "end_01"[shape=mdiamond,label="End If"];
        "01" -> "0110"[label="True"];
        "end_01" -> "02";
        "0110"[label="Foreach $x"];
        "0110" -> "011020";
        "loop_0110"[shape=ellipse,label="Next item In $x"];
        "loop_0110" -> "0110";
        "loop_0110" -> "end_01"[label="Loop End"];
        "011020"[label="CodeBlock"];
        "011020" -> "loop_0110";
        "02"[label="CodeBlock"];
        "02" -> "end_of_script";
}
```
Rendering the dot will give:

![Sample1](https://github.com/LxLeChat/FlowChartCore/blob/master/sample1.png?raw=true  | width=100)

With the ``-CodeAsText`` switch, the ``codeblock`` notation in the last label will changed, and will contain the actual code, here ``something else``
```
PS > $x=Find-FLowChartNodes -ScriptBlock $sb
PS > New-FLowChartGraph -Nodes $x -CodeAsText
digraph "a" {
        "01"[label="If $x",shape=diamond];
        "end_01"[shape=mdiamond,label="End If"];
        "01" -> "0110"[label="True"];
        "end_01" -> "02";
        "0110"[label="Foreach $x"];
        "0110" -> "011020";
        "loop_0110"[shape=ellipse,label="Next item In $x"];
        "loop_0110" -> "0110";
        "loop_0110" -> "end_01"[label="Loop End"];
        "011020"[label="\"Do Something\"",shape=box];
        "011020" -> "loop_0110";
        "02"[label="\"Something Else\"\l}",shape=box];
        "02" -> "end_of_script";
}
```
Rendering the dot will give:

![Sample2](/sample2.png)

# Dot language
To create the dot graph, i use a library called DotNetGraph. https://github.com/vfrz/DotNetGraph
