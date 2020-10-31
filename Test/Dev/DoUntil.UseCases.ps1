New-CodeUseCase 'DoUntil simple' {
    $x = 1,2,78,0
    do { $count++; $a++; } until ($x[$a] -eq 0)
    $count
}

New-CodeUseCase 'DoUntil simple and break' {
do {
    if (-not $foreach.MoveNext()) {
      break 
    }
    $token = $foreach.Current
  } until ($token.Kind -in @('Generic', 'Identifier'))
}