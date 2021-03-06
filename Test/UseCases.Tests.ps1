﻿<#
To parameterize the test :
 $container = New-PesterContainer -Path 'G:\PS\FlowChartCore\Test\UseCases.Tests.ps1' -Data @{ExcludeCase=@('Break','While','Switch','Foreach')}
 Invoke-Pester -Container $container 
#or
 Invoke-Pester -Container $container -PassThru #Renvoi un objet de type 'Pester.Run'

To run the test :
 $container = New-PesterContainer -Path 'G:\PS\FlowChartCore\Test\UseCases.Tests.ps1' 
 Invoke-Pester -Container $Container -Output Detailed 

Require Pester v5.1.0 rc1 ou >

#>
[CmdletBinding()]
param(
     # To exclude statements
     [Parameter(Mandatory=$False)]
    [string[]] $ExcludeCase
  )

# Delay the validity check otherwise, when binding, Pester
# fails because of the presence of validateset on the parameter.
If ( $null -eq $ExcludeCase)
{ $ExcludeCase=@('None') }

[ValidateSet('None','Break','While','Switch','Foreach','DoWhile','DoUntil')] 
[string[]] $ExcludeCase=$ExcludeCase

#FlowChartCore init
$ModulePath="$PSScriptRoot\..\Src\bin\Debug\netcoreapp3.1"

If ( !$IsLinux ) {
  UnBlock-File "$ModulePath\DotNetGraph.dll"
  UnBlock-File "$ModulePath\FlowchartCore.dll"
}

#first time, close and open PS console
Import-Module "$ModulePath\FlowChartCore.dll"

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
   if ($Statement -notIn $ExcludeCase)
   {
     Write-Host "`t add use cases for '$Statement' statement from $_"
     . $_.Fullname
   }
   else
   { Write-Host "`t exclude use cases for '$Statement' statement." -ForegroundColor Yellow}
 } 

# BeforeAll { 
#     $script:Parameters=@{
#         AsText=$false
#         NoDisplay=$true
#         Strict=$true
#     }
#     #F°
# }

if ($Null -eq $CodeUseCases)
{
  Write-Host "No use case defined." -ForegroundColor Yellow
  Return
}

Describe "FlowChartCode " -Tag 'DevUseCases' {
    Context "When there is no violation" {
        #On cherche à savoir si un type de construction est géré ou pas.
        #Chacune des ces constructions doit renvoyer un résultat
       It "<Name>." -TestCases $CodeUseCases {
        param($Name,$Code)         
         $script:Result=$script:OutView=$script:OutViewAsText=$null

          #Pas d'exception
         {   [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
           $script:Result=Get-FLowChartNode -ScriptBlock $Code } | Should -Not -Throw
          
          #Le résultat doit être renseigné
         $script:Result |Should -Not -BeNullOrEmpty
         
         {   [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
            $script:OutView=New-FLowChartGraph -Nodes $script:Result} | Should -Not -Throw
         
        $script:OutView |Should -Not -BeNullOrEmpty
         # Même chose mais avec -CodeAsText
         {   [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssigments', '')]
           $script:OutViewAsText=New-FLowChartGraph -Nodes $script:Result -CodeAsText Standard} | Should -Not -Throw
         
           $script:OutViewAsText|Should -Not -BeNullOrEmpty
      }
   }
}
