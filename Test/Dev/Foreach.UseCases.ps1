New-CodeUseCase 'Foreach simple' {
    $T=1..10
    Foreach ($I in $T)
    {
        Write-host "Foreach"
    }
    $letterArray = "a","b","c","d"
    foreach ($letter in $letterArray)
    {
      Write-Host $letter
    }
}

New-CodeUseCase 'Foreach simple 2' {
    Foreach ($I in 1..10)
    {
        Write-host "Foreach"
    }
}

New-CodeUseCase 'Foreach Pipeline' {
    Foreach ($S in Get-Process|Select-Object Name)
    {
        Write-host "Foreach"
    }
}

New-CodeUseCase 'Foreach Pipeline and automatic enum' {
    Foreach ($S in (Get-Process).Name)
    {
        Write-host "Foreach"
    }
}

New-CodeUseCase 'Foreach Nested Foreach' {
    Foreach ($I in 1..10)
    {
        Write-host "Foreach"
        Foreach ($I in 1..10)
        {
            Write-host "Nested Foreach"
        }
    }
}

New-CodeUseCase 'Foreach with array' {
    $i = 0
    foreach ($num in "one","two","three")
    {
        "Iteration: $i"
        $i++
        "`tNum: $num"
        "`tCurrent: $($foreach.Current)"

        if ($foreach.Current -eq "two")
        {
            "Before MoveNext (Current): $($foreach.Current)"
            $foreach.MoveNext() | Out-Null
            "After MoveNext (Current): $($foreach.Current)"
            "Num has not changed: $num"
        }
    } 
}

New-CodeUseCase 'Foreach enhanced' {
#function Get-FunctionPosition {
    [CmdletBinding()]
    [OutputType('FunctionPosition')]
    param(
      [Parameter(Position = 0, Mandatory,
        ValueFromPipeline, ValueFromPipelineByPropertyName)]
      [ValidateNotNullOrEmpty()]
      [Alias('PSPath')]
      [System.String[]]
      $Path
    )

    process {
      try {
        $filesToProcess = if ($_ -is [System.IO.FileSystemInfo]) {
          Write-Verbose "From pipeline"
          $_
        } else {
          Write-Verbose "From parameter, $Path"
          Get-Item -Path $Path
        }
        $parser = [System.Management.Automation.Language.Parser]
        Write-Verbose "lets start the foreach loop on `$filesToProcess with $($filesToProcess.count) as count"
        foreach ($item in $filesToProcess) {
          Write-Verbose "$item"
          if ($item.PSIsContainer -or
              $item.Extension -notin @('.ps1', '.psm1')) {
            continue
          }
          $tokens = $errors = $null
          $parser::ParseFile($item.FullName, ([REF]$tokens),
            ([REF]$errors)) | Out-Null
          if ($errors) {
            $msg = "File '{0}' has {1} parser errors." -f $item.FullName,
              $errors.Count
            Write-Warning $msg
          }
          :tokenLoop foreach ($token in $tokens) {
            if ($token.Kind -ne 'Function') {
              continue
            }
            $position = $token.Extent.StartLineNumber
            do {
              if (-not $foreach.MoveNext()) {
                break tokenLoop
              }
              $token = $foreach.Current
            } until ($token.Kind -in @('Generic', 'Identifier'))
            $functionPosition = [pscustomobject]@{
              Name       = $token.Text
              LineNumber = $position
              Path       = $item.FullName
            }
            Add-Member -InputObject $functionPosition `
              -TypeName FunctionPosition -PassThru
          }
        }
      }
      catch {
        throw
      }
    }
  }
