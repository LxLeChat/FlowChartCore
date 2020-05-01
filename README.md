# FlowChartCore
 FlowChart c# version
 
 First time writing c# ... Jump from PS to C# ...
 
 This will be used mainly for PowerShell Core. (trying to make it compatible with 5.1 but no succes at the moment)
 
 The main goal is to generate a dot graph, then it will be consumed by GraphViz, or any tool accepting dot language.
 The result of the dot being consumed, will be a PowerShell Script FlowChart.

# Steps
 - [x] Create Node structure referencing all types for, foreach, if etc..
 - [x] Add properties & methods for each types of nodes (like linkednodelist, ability to find a specific node by id/type up/down...)
 - [ ] Implement Graph Method to generate a dot file
 - [ ] Create PS Cmdlet with C#

## Importing the dll
``` powershell
Import-module .\Flowchartcore.dll
```

## Utility Method Implemented:
- ParseScriptBlock
Given the following PowerShell scriptblock:
``` powershell
$a = {
  foreach ($item in $collection) {
      for ($i = 0; $i -lt $array.Count; $i++) {}
      for ($i = 0; $i -lt $array.Count; $i++) {}
      for ($i = 0; $i -lt $array.Count; $i++) {}
  }
}
```
You can do this:
``` powershell
PS \> $x = [FlowChartCore.Utility]::ParseScriptBlock($a)
```
Will return the following:
``` powershell
PS \>  $x
Label    : testlabel
Name     : ForNode
Children : {IfNode, ForNode, ForNode}
Parent   :
Position : 1
Depth    : 0
Id       : 0001
IsLast   : False
IsFirst  : True

Label    :
Name     : ForNode
Children : {ForNode, ForNode, ForNode}
...

Label    :
Name     : ForNode
Children : {}
Parent   :
Position : 10
Depth    : 0
Id       : 0010
IsLast   : True
IsFirst  : False
```

ParseScriptBlock actually build a Tree object. And we only return the Nodes property of this object wich is a List of Node object.


# Done & ToDo:
- [x] Discovering & Implementing Foreach
- [x] Discovering & Implementing For
- [x] Discovering & Implementing If / ElseIf / Else
- [x] Discovering & Implementing While
- [x] Discovering & Implementing DoWhile
- [x] Discovering & Implementing DoUntil
- [x] Discovering & Implementing Switch / Switch Case / Switch Default
- [x] Discovering & Implementing Try / Catch / Finally
- [x] Discovering & Implementing Loops Labels
- [x] Discovering & Implementing Keyword: Break, Continue
- [ ] Discovering & Implementing Keyword: Exit, Return
- [x] Implement Method FindNodeByType
- [x] Implement Method FindNodeByTypeUp
- [x] Implement Method FindNodeByLabelUp
