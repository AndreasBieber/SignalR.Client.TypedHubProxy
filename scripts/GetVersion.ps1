Param(
	 [Parameter(Mandatory=$True)][string]$buildCounter,
	 [Parameter(Mandatory=$True)][string]$currentBranch,
	 [Parameter(Mandatory=$False)][bool]$isRelease
)

# Sometimes the branch will be a full path, e.g., 'refs/heads/master'. 
# If so we'll base our logic just on the last part.
if ($currentBranch.Contains("/")) {
  $currentBranch = $currentBranch.substring($currentBranch.lastIndexOf("/")).trim("/")
}

Write-Host "Current branch: $currentBranch"

# Load version from file
$version = Get-Content .\VERSION -Raw

# Define version numbers
$assemblyVersion = $version
$fileVersion = "$version.$buildCounter"
$informationalVersion = "$version.$buildCounter"
Write-Host "Assemblyversion: $assemblyVersion"
Write-Host "FileVersion: $fileVersion"
Write-Host "InformationalVersion: $informationalVersion"

if ($isRelease -eq $False) {
	if ($currentBranch -eq "master") {
		$informationalVersion += "-pre"
	} elseif (  $currentBranch -match "feature-.*" -or 
				$currentBranch -match "bugfix-.*") {	
		$informationalVersion += "-$currentBranch"
	}
}

# Tell teamcity the new buildnumber
Write-Host "##teamcity[buildNumber '$informationalVersion']"

# Tell teamcity the version
Write-Host "##teamcity[buildAssemblyVersion '$assemblyVersion']"
Write-Host "##teamcity[buildFileVersion '$fileVersion']"
Write-Host "##teamcity[buildInformationalVersion '$informationalVersion']"