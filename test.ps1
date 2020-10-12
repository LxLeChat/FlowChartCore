$sb={
	try{
		faitqqchose
		$PSDrive = $global:conf
		
		if((Get-PSDrive -Name $PSDrive -ea Ignore) -eq $null)
        {
			writeLog "Mount PSDrive: $PSDrive"
			if((Test-Connection $SCCMServer -Count 2 -Quiet) -eq $false)
            { 
                writeLog ("{0} is unreachable" -f $SCCMServer)
            }
            else
            { 
                writeLog ("server respond")
				if($remoteAuth -eq $false)
                {
                    writeLog ("no credential")
					if((New-PsDrive -Name $PSDrive -PSProvider CMSite -Root $SCCMServer -Scope Global) -eq $null)
                    { $errorMessage = $CannotMountPSDrive}
				}
                else
                { 
                     writeLog ("with credential")
					if((New-PsDrive -Name $PSDrive -PSProvider CMSite -Root $SCCMServer -Scope Global -Credential $credentials.$env) -eq $null)
                    { $errorMessage = $CannotMountPSDrive }
				}
			}
		}



		if($processStatus -eq $false){
			throw $errorMessage
        }
        


		if($processStatus -eq $true){
			Push-Location "$($PSDrive):"
		}
	}catch{
		throw $_
	}
}