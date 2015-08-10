function Get-AssemblyVersionFromAssemblyInfo {
	param(
		[Parameter(Mandatory=$True)][string] $File
	)

	# Load version from AssemblyInfo.cs
	$fileContent = Get-Content $file

	$regex = [regex] "^\[assembly\: AssemblyVersion\(`"([0-9]+\.[0-9]+\.[0-9]+)(\.[0-9]+)?`"\)\]"
	foreach($line in $fileContent) {
		if ($line -imatch $regex) {
			$version = $matches[1]
			$found = $True
			break
		}
	}

	if (-Not $found){
		Throw "Could not find [assembly: AssemblyVersion(`"`") attribute inside of $File."
	}

	return $version
}

function Patch-AssemblyInfoVersions {
	[CmdLetBinding()]
	param(
		[Parameter(Mandatory=$True)][string] $File,
		[Parameter(Mandatory=$True)][string] $AssemblyVersion,
		[Parameter(Mandatory=$True)][string] $FileVersion,
		[Parameter(Mandatory=$True)][string] $InformationalVersion
	)

	$fileContent = Get-Content $File -Raw
	$fileContent = [Regex]::Replace($fileContent, 'AssemblyVersion\("[^"]*"\)', "AssemblyVersion(`"$AssemblyVersion`")")
	$fileContent = [Regex]::Replace($fileContent, 'AssemblyFileVersion\("[^"]*"\)', "AssemblyFileVersion(`"$FileVersion`")")
	$fileContent = [Regex]::Replace($fileContent, 'AssemblyInformationalVersion\("[^"]*"\)', "AssemblyInformationalVersion(`"$InformationalVersion`")")

	Set-Content $File $fileContent
}

function Generate-FileAndInformationalVersion {
	[CmdLetBinding()]
	param(
		[Parameter(Mandatory=$True)][string] $AssemblyVersion,
		[Parameter(Mandatory=$True)][string] $BuildCounter,
		[Parameter(Mandatory=$True)][string] $Branch,
		[Parameter(Mandatory=$False)][bool] $IsRelease
	)

	$fileVersion = "$AssemblyVersion.$BuildCounter"
	$informationalVersion = "$AssemblyVersion.$BuildCounter"
	
	if ($IsRelease -eq $False) {
		if ($Branch -eq "master") {
			$informationalVersion += "-dev"
		} elseif (  $Branch -match "feature-.*" -or 
					$Branch -match "bugfix-.*") {	
			$informationalVersion += "-$Branch"
		}
	}

	return $fileVersion,$informationalVersion
}