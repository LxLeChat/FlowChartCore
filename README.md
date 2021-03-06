# FlowChartCore
The main goal is to document Powershell Scripts. The Module will allow you to generate a dot graph definition. This definition can then be consumed by GraphViz to generate a nice flow chart of your script.

# Disclaimer
- It's the first time i'm writing something in c# ... jumping from Powershell... so, i'm sorry if the code is not as clean as excpected..! I know i'm missing c# tests for example..
- Since i'm discovring a lot of bugs, and strange behavior, the module is not published in the PSGallery at the moment.
This project allows me also to experiment on new stuff .. like CI.. !

### Why ?
First because i had a bad experience at a client, who asked me to add new functiunalities to a HUGE script (actually it was 3 gigantic scripts... anyway... ). Every modification i made had some impacts on other scripts... so i was tired every time to look a all these scripts... and thought ... maybe having a graph representation might make my life easier !


### Compatibility
This module works on PS v5 & v7

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
At the moment, the script will not discover what's inside nested scriptblocks. For example, if you script is a massive ``Invoke-Command``, the module will discover a simple ``CodeNode`` and the resulting graph will contain only one ``CodeBlock`` (more on nodes in the readme). Same thing for ``Foreach-Object``, ``Where-Object`` ...
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
When you parse a scriptblock or a script using ``Get-FlowChartNode``, the cmdlet will return a list of nodes.
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
PS >$x = Get-FlowChartNode -ScriptBlock $Sb
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
~~The module is cross platform, so it works on Windows Powershell, and PSCore (tested on WSL).~~
~~The Module is CrossPlatform, but is not WindowsPowershell Compatible.~~
The Module is CrossPlatform, AND WindowsPowershell Compatible.
Since it's not published in the PSGallery, either fork the project or download it as a zip.

### Importing the module
PS7:  You have to import the ``FlowChartCore.dll`` wich can be found in ``Code\src\bin\debug\netcoreapp3.1\FlowChartCore.dll``
PSV5: You have to import the ``FlowChartCore.dll`` wich can be found in ``Code\src\bin\debug\netsandard2.0\FlowChartCore.dll``

```
PS >Import-module .\Flowchartcore.dll
PS > Get-Module -Name FlowchartCore

ModuleType Version    Name                                ExportedCommands
---------- -------    ----                                ----------------
Binary     1.0.0.0    FlowchartCore                       {Get-FLowChartNode, New-FLowChartGraph}
```

### Get-FlowChartNode
The cmdlet will find nodes in a file (a ps1) or a scriptblock.
```
PS >$Sb = {
  If ( $x ) {
    Foreach ( $y in $x ) {
      "Do Something"
    }
  }
}

PS > Get-FlowChartNode -ScriptBlock $Sb
Condition : $x
Name      : IfNode
Children  : {ForeachNode}
Parent    :
Position  : 1
Depth     : 0
Id        : 01
Graph     : {}
```

Using a Path:
```
PS > Get-FlowChartNode -Path .\somescript.ps1
PS > Get-ChildItem -Path c:\temp -Filter *.ps1 | Get-FLowChartNode
```

# New-FlowChartGraph
The cmdlet will return a dot definition. It Takes a List of nodes as input.
There is a parameter ``-codeastext`` that will render discover the code for a given ``codenode`` and display it instead of just a ``codeblock``. We will this the difference in the examples.
``-codeastext`` parameter, if using the ``Formatted`` keyword, will use PSScriptAnalyzer to reformat the discovercode... if PSScriptAnalyzer is not available, a warning will be displayed.
```
PS > $x=Get-FLowChartNode -ScriptBlock $sb
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

<img src="https://github.com/LxLeChat/FlowChartCore/blob/master/sample1.png?raw=true" width="177" height="513">

With the ``-CodeAsText`` parameter, the ``codeblock`` notation in the last label will change, and will contain the actual code, here ``something else``. It's a validateSet. You can choose between ``standard`` or ``formatted``. ``formatted`` will try to use ``PSScriptAnalyzer`` to format the code nicely. If ``PSScriptAnalyzer`` is not installed, it will fallback to the ``standard`` formatting method. The code might look strange! but it works.
Also, something to take in consideration, is that using ``PSScriptanalyzer`` can slow the rendering
```
PS > $x=Find-FLowChartNodes -ScriptBlock $sb
PS > New-FLowChartGraph -Nodes $x -CodeAsText Standard
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

<img src="https://github.com/LxLeChat/FlowChartCore/blob/master/sample2.png?raw=true" width="177" height="513">

The cmdlet also support the pipeline so you can do something like
```
PS > Get-FLowChartNode -ScriptBlock $sb | New-FLowChartGraph -CodeAsText Standard
```

# Rendering the graph
If you want to render the Dot Definition, you can for example user ``Export-PSGraph`` from the ``PSGraph`` module.

You can also use https://dreampuf.github.io/GraphvizOnline/ . Just copy/paste the dot definition and it will render the graph

# Dot language
To create the dot graph, i use a library called DotNetGraph. https://github.com/vfrz/DotNetGraph

# Building the project
Clone the project.
You will need ``.Net Core SKD`` to build the solution, available here https://dotnet.microsoft.com/download/dotnet/3.1
You can use the ``build.ps1``.
Or you can use the VSCode task by pressing F1, then typing task, you should be able to use the "Tasks: Run build task"

# Contributions
Constributions are welcomed! I Speak french, and it's easier for me when dealing with a french contributor, but i'll do my best if you post in english :)
Big Thanks to @LaurentDardenne for his many contributions !
