#
# Manifeste de module pour le module « FlowChartCore »
#
# Généré par : LxLeChat
#
# Généré le : 31/10/2020
#

@{

# Module de script ou fichier de module binaire associé à ce manifeste
RootModule = '.\FlowchartCore.dll'

# Numéro de version de ce module.
ModuleVersion = '0.1.0'

# Éditions PS prises en charge
CompatiblePSEditions = 'Core' 

# ID utilisé pour identifier de manière unique ce module
GUID = '7ec5cdcb-6d15-44df-8985-754c9b54e482'

# Auteur de ce module
Author = 'LxLeChat'

# Société ou fournisseur de ce module
CompanyName = 'LxLeChat'

# Déclaration de copyright pour ce module
Copyright = 'Copyleft'

# Description de la fonctionnalité fournie par ce module
Description = 'The main goal is to document Powershell scripts. The module will allow you to generate a dot graph definition.'

# Version minimale du moteur Windows PowerShell requise par ce module
PowerShellVersion = '7.0'

# Architecture de processeur (None, X86, Amd64) requise par ce module
ProcessorArchitecture = 'None'

# Modules qui doivent être importés dans l'environnement global préalablement à l'importation de ce module
# RequiredModules = @()

# Assemblys qui doivent être chargés préalablement à l'importation de ce module
RequiredAssemblies = '.\DotNetGraph.dll'

# Fichiers de script (.ps1) exécutés dans l’environnement de l’appelant préalablement à l’importation de ce module
# ScriptsToProcess = @()

# Fichiers de types (.ps1xml) à charger lors de l'importation de ce module
# TypesToProcess = @()

# Fichiers de format (.ps1xml) à charger lors de l'importation de ce module
# FormatsToProcess = @()

# Fonctions à exporter à partir de ce module. Pour de meilleures performances, n’utilisez pas de caractères génériques et ne supprimez pas l’entrée. Utilisez un tableau vide si vous n’avez aucune fonction à exporter.
FunctionsToExport = @()

# Applets de commande à exporter à partir de ce module. Pour de meilleures performances, n’utilisez pas de caractères génériques et ne supprimez pas l’entrée. Utilisez un tableau vide si vous n’avez aucune applet de commande à exporter.
CmdletsToExport = 'Find-FLowChartNodes', 'New-FLowChartGraph'

# Variables à exporter à partir de ce module
VariablesToExport = @()

# Alias à exporter à partir de ce module. Pour de meilleures performances, n’utilisez pas de caractères génériques et ne supprimez pas l’entrée. Utilisez un tableau vide si vous n’avez aucun alias à exporter.
AliasesToExport = @()

# Liste de tous les modules empaquetés avec ce module
ModuleList = @('FlowChartCore')

# Liste de tous les fichiers empaquetés avec ce module
FileList = 'FlowchartCore.dll', 'DotNetGraph.dll'

# Données privées à transmettre au module spécifié dans RootModule/ModuleToProcess. Cela peut également inclure une table de hachage PSData avec des métadonnées de modules supplémentaires utilisées par PowerShell.
PrivateData = @{

    PSData = @{

        # Des balises ont été appliquées à ce module. Elles facilitent la découverte des modules dans les galeries en ligne.
        Tags = @('PSEdition_Core','graph','graphviz','diagram','AST','AbstractSyntaxTree')

        # URL vers la licence de ce module.
        # LicenseUri = ''

        # URL vers le site web principal de ce projet.
        ProjectUri = 'https://github.com/LxLeChat/FlowChartCore'

        # URL vers une icône représentant ce module.
        # IconUri = ''

        # Propriété ReleaseNotes de ce module
        ReleaseNotes = 'Alpha version'

    } # Fin de la table de hachage PSData

} # Fin de la table de hachage PrivateData

# URI HelpInfo de ce module
# HelpInfoURI = ''
}
