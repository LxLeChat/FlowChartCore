---
external help file: FlowChartCore.dll-Help.xml
Module Name: FlowChartCore
online version:
schema: 2.0.0
---

# Find-FLowChartNodes

## SYNOPSIS
Construit un graph des instructions contenant dans un script Powershell.

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
détail

## EXAMPLES

### Basic
@{paragraph=PS C:\\\>}

```
Find-FlowChartNodes -Scriptblock { Get-Childitem }
```

Description

Sortie attendue

## PARAMETERS

### -ScriptBlock
Code à analyser.

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
Nom du chemin d'un script à analyser

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
Nom du chemin littéral d'un script à analyser

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
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### Scriptblock
## OUTPUTS

### FlowChartCore.Node
## NOTES

## RELATED LINKS
