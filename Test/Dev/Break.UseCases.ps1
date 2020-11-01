    #cas sans resultat
#  New-CodeUseCase 'Break absent' {
#     $grade = 92
# }

 New-CodeUseCase 'Break IF without Break' {
   $grade = 92
   if ($grade -ge 90) { "Grade A"}
}

 New-CodeUseCase 'Break IFElse without Break' {
    $grade = 92
    if ($grade -ge 90) { "Grade A"}
    Else {"Grade B"}
}

 New-CodeUseCase 'Break ElseIF without Break' {
    $grade = 92
    if ($grade -ge 90) { "Grade A"}
    elseif ($grade -ge 80) { "Grade B"} 
}

 New-CodeUseCase 'Break at the end of code' {
    $grade = 92
    if ($grade -ge 90) { "Grade A"}
    Break
}

 New-CodeUseCase 'Break inside IF block' {
  $grade = 92
  if ($grade -ge 90) { "Grade A";Break}
}

#todo Token IF
 New-CodeUseCase 'Break IF ElseIF Else' {
    $grade = 92
    if ($grade -ge 90) { "Grade A" }
    elseif ($grade -ge 70)  { "Grade C"}
    else { "Grade D"}
}

#NOK
 New-CodeUseCase 'Break IF ElseIF' {
    $grade = 92
    if ($grade -ge 90) { "Grade A" }
    elseif ($grade -ge 70)  { "Grade C"}
 }

#NOK
 New-CodeUseCase 'Break IF inside For loop' {
    for ($i = 1; ; ++$i)
    {
        if ($i * $i -gt 50)
        {
            Get-ChildItem
        }
    }
}
