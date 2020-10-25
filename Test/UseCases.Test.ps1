﻿[CmdletBinding()]
param()

#FlowChartCore init
$ModulePath="$PSScriptRoot\..\Src\bin\Debug\netstandard2.0"

if (-Not (Test-Path Env:GRAPHVIZ_DOT))
{ $Env:GRAPHVIZ_DOT='C:\Tools\Graphviz\bin' }

unblock-File "$ModulePath\DotNetGraph.dll"
unblock-File "$ModulePath\FlowchartCore.dll"
 #first time, close and open PS console
Import-Module "$ModulePath\FlowchartCore.dll"

Function New-CodeUseCase{
#New-PSCustomObjectFunction -Noun CodeUseCase -Parameters Name,Code -file
param(
     [Parameter(Mandatory=$True,position=0)]
    [String] $Name,

     [Parameter(Mandatory=$True,position=1)]
    [ScriptBlock] $Code
)

  @{
    Name=$Name;
    Code=$Code;
   }
}

# Build use cases
$UseCasesPath="$PSScriptRoot\..\Test\Dev"
Write-Host "`n`rAdd use cases from :`r`n$UseCasesPath"
  
$CodeUseCases=Get-Childitem $UseCasesPath |
 Where-Object {$_.Name -match '\.UseCases\.ps1'}|
 Foreach-Object {
   $Statement=$_.FullName -replace '^.*\\(.*?)\.UseCases\.ps1','$1'
   Write-Host "`t add use cases for '$Statement' statement from $_"
    . $_.Fullname
 } 

# BeforeAll { 
#     $script:Parameters=@{
#         AsText=$false
#         NoDisplay=$true
#         Strict=$true
#     }
#     #F°
# }

Describe "FlowChartCode " -Tag 'DevUseCases' {
    Context "When there is no violation" {
        #On cherche à savoir si un type de construction est géré ou pas.
        #Chacune des ces constructions doit renvoyer un résultat
       It "<Name>." -TestCases $CodeUseCases {
        param($Name,$Code)         
         $script:Result=$script:OutView=$script:OutViewAsText=$null

          #Pas d'exception
         {   [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
           $script:Result=Find-FLowChartNodes -ScriptBlock $Code } | Should -Not -Throw
          
          #Le résultat doit être renseigné
         $script:Result |Should -Not -BeNullOrEmpty
         
         {   [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
            $script:OutView=New-FLowChartGraph -Nodes $script:Result} | Should -Not -Throw
         
        $script:OutView |Should -Not -BeNullOrEmpty
         # Même chose mais avec -CodeAsText
         {   [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
           $script:OutViewAsText=New-FLowChartGraph -Nodes $script:Result -CodeAsText} | Should -Not -Throw
         
           $script:OutViewAsText|Should -Not -BeNullOrEmpty
      }
   }
}

