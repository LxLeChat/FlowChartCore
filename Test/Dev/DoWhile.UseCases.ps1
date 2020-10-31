New-CodeUseCase 'DoWhile simple' {
    do {
        if ($x[$a] -lt 0) { continue }
        Write-Host $x[$a]
    }
    while (++$a -lt 10)
}