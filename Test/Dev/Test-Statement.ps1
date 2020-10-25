Function Test-Statement{
<#
.SYNOPSIS
  Analyze a scriptblock, compile it to a .dot file and displaying the generated graph
.EXAMPLE

$DefaultParameters=@{
  AsText=$true
  NoDisplay=$false
  Strict=$false
}

$sb={
  If ($i -eq 0) {'ok'}
}

$Parameters = $DefaultParameters+@{
   ScriptBlock=$sb
   FileName='Testflow'
}

Test-Statement @Parameters
#>
    [cmdletbinding()]
 param(
   [ScriptBlock] $ScriptBlock,
 
   [string] $Path=$Env:Temp,

   [string] $FileName,

   [switch] $AsText,

   [switch] $NoDisplay,

   [switch] $Strict
)

 try {
   if (Test-Path Variable:E)
   { Remove-Variable -Name E -Scope 1 -ErrorAction Ignore}
   
   $FullFileName=Join-Path $Path $FileName
   Write-verbose @"
Path : $FileName
Filename : $FileName
Code=$ScriptBlock
"@

   $Result=Find-FLowChartNodes -ScriptBlock $ScriptBlock|New-FLowChartGraph -CodeAsText:$AsText
   if ($null -ne $Result) 
   { 
       $GraphFilename="$FullFileName.graph"
       $Result|Set-Content $GraphFilename
       if ((Test-path $GraphFilename) -and ((Get-Item $GraphFilename).length -ne 0))
       {
         &"$Env:GRAPHVIZ_DOT\dot.exe" -Tpng $GraphFilename -o "$FullFileName.png"
         if (! $NoDisplay.IsPresent)
         { Invoke-Item "$FullFileName.png" }
       }
       else 
       { Write-Warning "No file or zero size file." }
   }
   else
   { Write-Warning "No result."  }
 } catch {
     if ($Strict)
      { $PSCmdlet.ThrowTerminatingError($_) }

      Set-Variable -Name E -Value $Error[0] -Scope 1
      Write-Warning "Bug. View `$E variable."  
      $E|Select-Object *
      $Stacktrace
      Throw $_
 }
}