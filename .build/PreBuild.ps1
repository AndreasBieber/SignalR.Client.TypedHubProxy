[CmdLetBinding()]
Param(
	[Parameter(Mandatory=$True)][string]$TeamCityUrl,
	[Parameter(Mandatory=$True)][string]$TeamCityBuildTypeId,
	[Parameter(Mandatory=$True)][string]$TeamCityUser,
	[Parameter(Mandatory=$True)][string]$TeamCityPass,
	[Parameter(Mandatory=$True)][string]$AssemblyInfoPath,
	[Parameter(Mandatory=$True)][string]$BuildCounter,
	[Parameter(Mandatory=$True)][string]$CurrentBranch,
	[Parameter(Mandatory=$False)][bool]$IsRelease
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path $AssemblyInfoPath)) {
	Throw "Could not find file $AssemblyInfoPath"
}

# Include scripts
. .\build\Utils.ps1
. .\build\Teamcity-Functions.ps1

# Sometimes the branch will be a full path, e.g., 'refs/heads/master'. 
# If so we'll base our logic just on the last part.
if ($currentBranch.Contains("/")) {
  $currentBranch = $currentBranch.substring($currentBranch.lastIndexOf("/")).trim("/")
}

# Build credential header for upcoming Rest-Calls
$creds = Get-CredentialHeader -User $TeamCityUser -Pass $TeamCityPass

# Try to get assembly version
$assemblyVersion = [version] (Get-AssemblyVersionFromAssemblyInfo -File $AssemblyInfoPath)

# Derive fileVersion and informationalVersion from assembly version
$derivedVersions = Generate-FileAndInformationalVersion -AssemblyVersion $assemblyVersion -BuildCounter $BuildCounter -Branch $CurrentBranch -IsRelease $IsRelease
$fileVersion = $derivedVersions[0]
$informationalVersion = $derivedVersions[1]

# Try to get last built version from teamcity
$lastBuiltVersion = [version] (Get-TeamcityBuildTypeParameter -BaseUrl $TeamCityUrl -Credentials $creds -BuildTypeId $TeamCityBuildTypeId -Parameter "system.lastBuiltVersion")

# Check whether assemblyinfo version is greater than last built version.
# If so, we have to reset the TeamCity buildCounter.
if ($assemblyVersion -gt $lastBuiltVersion) {
	Write-Host "New version detected! (New version: $assemblyVersion; Old version: $lastBuiltVersion)"

	# Persist assemblyInfoVerion to TeamCity
	Write-Host "Persisting new version number to TeamCity..."
	Set-TeamcityBuildTypeParameter -BaseUrl $TeamCityUrl -Credentials $creds -BuildTypeId $TeamCityBuildTypeId -Parameter "system.lastBuiltVersion" -Value $assemblyVersion.ToString()

	# Reset buildCounter
	Write-Host "Resetting build counter ..."
	Reset-BuildCounterForBuildType -BaseUrl $TeamCityUrl -Credentials $creds -BuildTypeId $TeamCityBuildTypeId 

	# Update informational version with new buildCounter
	$informationalVersion -match '([0-9]+\.[0-9]+\.[0-9]+)\.([0-9]+)(-.*)?'
	$informationalVersion = "$($Matches[1]).1$($Matches[3])"
}

# Patch AssemblyInfo.cs
Write-Host "Patching $AssemblyInfoPath ..."
Patch-AssemblyInfoVersions -File $AssemblyInfoPath -AssemblyVersion $assemblyVersion -FileVersion $fileVersion -InformationalVersion $informationalVersion

# Announce version numbers to TeamCity
Set-TeamcityParameter -Parameter "build.assemblyVersion" -Value $assemblyVersion
Set-TeamcityParameter -Parameter "env.fileVersion" -Value $fileVersion
Set-TeamcityParameter -Parameter "env.informationalVersion" -Value $informationalVersion

# Set buildNumber
Write-Host "##teamcity[buildNumber '$informationalVersion']"
