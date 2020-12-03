---
external help file: FlowChartCore.dll-Help.xml
Module Name: FlowChartCore
online version:
schema: 2.0.0
---

# Find-FLowChartNodes

## SYNOPSIS
Construit un graph à partir des instructions contenues dans un script Powershell.

Voici la liste instructions gérées :

Structures de contrôle : If/Else/ElseIf, Switch, Try/Catch/Finally

Boucles : Do Until, Do While, For, Foreach, Foreach-Object, While (Foreach -Parallel -> Pas implémenté).

Commandes de sortie de boucle : Break, Continue, Exit, Return, Throw.

Opérateur ternaire:  ?:   -> Pas implémenté.

Opérateur d'affectation conditionnelle null : ?=   -> Pas implémenté.

Note: l'analyse des conteneurs de code n'est pas implémenté. Par exemple l'affectation d'un scriptblock à une variable, le code d'une fonction ou le scriptblock associé au cmdlet Foreach-Object

## SYNTAX

### Script
```
Find-FLowChartNodes [-ScriptBlock] <ScriptBlock> [<CommonParameters>]
```

### Path
```
Find-FLowChartNodes [-Path] <String[]> [[-LiteralPath] <String[]>] [<CommonParameters>]
```

## DESCRIPTION
Lorsque vous analysez un scriptblock ou un script à l'aide de Find-FlowChartNodes, la cmdlet renvoie une liste de nœuds.
Par exemple, pour une instruction If le cmdlet renvoit un objet IfNode et ainsi de suite.

## EXAMPLES

### Analyser un scriptblock
```
$Sb={
 If ( $x ) 
 {
    Foreach ( $y in $x ) 
    { "Do Something" }
 }
}
$List=Find-FlowChartNodes -Scriptblock $Sb
$List

#Condition : $x
#Name      : IfNode
#Children  : {ForeachNode}
#Parent    :
#Position  : 1
#Depth     : 0
#Id        : 01
#Graph     : {}

```

Description

Le cmdlet analyse le scriptblock et renvoi une liste de noeud.
Le résultat peut être vide si le code ne contient pas d'instruction reconnue.

.
### Afficher tous les noeuds
```
$Sb={
 If ( $x ) 
 {
    Foreach ( $y in $x ) 
    { "Do Something" }
 }
}
$List=Find-FlowChartNodes -Scriptblock $Sb
$List[0].FindNodes({$args[0] -is [FlowChartCore.Node]},$true)|Select-Object Name

#Name
#----
#ForeachNode
#CodeNode

```

Description

Le cmdlet analyse le scriptblock et renvoi une liste de noeud. On recherche et affiche tous les noeuds de la liste.

.
### Analyser un script
```
Find-FlowChartNodes -Path .\somescript.ps1
Get-ChildItem -Path c:\temp -Filter *.ps1 | Find-FLowChartNodes
```

Description

Le cmdlet analyse le ou les fichiers et renvoi une liste de noeud. On recherche et affiche tous les noeuds de la liste.

.

### Afficher tous les noms de classes dérivés de la classse 'FlowChartCore.Node'
```
[FlowChartCore.Node].Assembly.ExportedTypes|
 Where-Object {$_.IsSubclassOf([FlowChartCore.Node])}|
 Select-Object Name

#Name
#----
#CodeNode
#ElseIfNode
#...

```

Description

Le code précédent affiche la liste des classes représentant un type d'instruction.


## PARAMETERS

### -ScriptBlock
Code à analyser. Contient une suite d'instructions Powershell.

```yaml
Type: ScriptBlock
Parameter Sets: Script
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -Path
Nom du chemin d'un script Powershell à analyser.

```yaml
Type: String[]
Parameter Sets: Path
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### -LiteralPath
Nom du chemin littéral d'un script Powershell à analyser

```yaml
Type: String[]
Parameter Sets: Path
Aliases: PSPath

Required: False
Position: 0
Default value: None
Accept pipeline input: True (ByPropertyName, ByValue)
Accept wildcard characters: False
```

### CommonParameters
Ce cmdlet supporte les paramètres communs suivant : -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. Pour plus d'information, voir [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Scriptblock
## OUTPUTS

### FlowChartCore.Node
## NOTES

## RELATED LINKS
