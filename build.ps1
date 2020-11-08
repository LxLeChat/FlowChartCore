$Messages = @{
    DotNetBuild = "Building Project..."
    BuildSuccessfull = "Build Successfull.."
    BuildFailed = "Build Failed.."
    ImportingModule = "Importing Newly Build Module.."
    RunningTests = "Running Pester Tests..."
}

Write-Information -MessageData $Messages.DotNetBuild -InformationAction Continue
Set-Location -Path .\Src
$Build = dotnet build

## was build successfull ?
If ( $? ) {
    Write-Information $Messages.BuildSuccessfull -InformationAction Continue

    Write-Information $Messages.ImportingModule -InformationAction Continue
    Set-Location -Path .\bin\debug\netcoreapp3.1\
    Import-Module .\FlowchartCore.dll -ErrorAction Stop

    Write-Information $Messages.RunningTests -InformationAction Continue
    Set-Location -Path ..\..\..\..\Test
    Import-Module -Name Pester -ErrorAction Stop
    Invoke-Pester -Path  .\UsesCases.Tests.ps1 -Output Detailed
} Else {
    Write-Information $Messages.BuildFailed -InformationAction Continue
    Throw "Build Failed..."
}