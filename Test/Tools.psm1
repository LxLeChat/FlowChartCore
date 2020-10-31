
#https://github.com/pester/Pester/issues/1713
[string[]] $script:_ExcludeCase=@('None')

function Set-ExcludeCase {
param(
    [ValidateSet('None','Break','While','Switch','Foreach','DoWhile','DoUntil')]
    [string[]] $ExcludeCase
  )
  $script:_ExcludeCase=Write-Output $ExcludeCase
}
function Get-ExcludeCase {
    $script:_ExcludeCase
}

Export-ModuleMember -Function 'Set-ExcludeCase','Get-ExcludeCase'
