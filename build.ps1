param (
    [Parameter(Mandatory=$false)]
    [Switch]
    $OfflineBuild,
    [Parameter(Mandatory=$False)]
    [ValidateSet]
    $TestPlateform = ("PS7","PS5")
)


$Messages = @{
    DotNetBuild = "Building Project..."
    BuildSuccessfull = "Build Successfull.."
    BuildFailed = "Build Failed.."
    ImportingModule = "Importing Newly Build Module.."
    RunningTests = "Running Pester Tests..."
}

Write-Information -MessageData $Messages.DotNetBuild -InformationAction Continue


## build offline, configuration offline packages & config file
Set-Location -Path .\Src
remove-item .\Packages\* -Recurse
Expand-Archive .\resources\Packages.zip -DestinationPath .\Packages
[xml]$NugetConfig = Get-Content .\packages\nuget.config
$NugetConfig.configuration.packageSources.add.value = "$($(Get-Item .\packages\nuget.config).Directory)"
$NugetConfig.Save($(get-item .\packages\nuget.config).FullName)

## cleaning dotnet output directories
# dotnet clean

## cleaning Module directory
Gci ..\Module -Recurse | Remove-Item

## dotnet build
$Build = dotnet build
set-location ..

## was build successfull ?
If ( $? ) {
    Write-Information $Messages.BuildSuccessfull -InformationAction Continue

    Write-Information $Messages.ImportingModule -InformationAction Continue
    Import-Module .\src\bin\debug\netcoreapp3.1\FlowchartCore.dll -ErrorAction Stop

    Write-Information $Messages.RunningTests -InformationAction Continue
    # Set-Location -Path ..\Test

    Import-Module -Name Pester -ErrorAction Stop
    Set-Location .\Test
    # Invoke-Pester -path .\test\UsesCases.Tests.ps1 -Output Detailed -ErrorAction stop
    Invoke-Pester
    
} Else {
    Write-Information $Messages.BuildFailed -InformationAction Continue
    Throw "Build Failed..."
}