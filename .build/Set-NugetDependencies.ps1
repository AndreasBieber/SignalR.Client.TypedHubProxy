[CmdLetBinding()]
param(
    [Parameter(Mandatory=$True)][string]$NuspecFile,
    [Parameter(Mandatory=$True)][string]$PackagesConfig
)

$ErrorActionPreference = "Stop"

try {
	if (-Not (Test-Path $NuspecFile)) {
		Throw "Could not find file $NuspecFile"
	}

	if (-Not (Test-Path $PackagesConfig)) {
		Throw "Could not find file $PackagesConfig"
	}

	# Load nuspec file
	[xml] $nuspec = Get-Content $NuspecFile

	# Ensure dependencies-node exist
	if ($nuspec.SelectSingleNode('//package/metadata/dependencies') -eq $null) {
		$nuspec.package.metadata.AppendChild($nuspec.CreateElement('dependencies'))
	}
	#if ($nuspec.package.metadata.dependencies -eq $null) {
	#	$nuspec.package.metadata.AppendChild($nuspec.CreateElement('dependencies'))
	#}

	# Clear dependencies from nuspec
	if (($node = $nuspec.SelectSingleNode('//package/metadata/dependencies')).HasChildNodes) {
		$node.RemoveAll()
	}

	# Load packages.config
	[xml] $dependencies = Get-Content $PackagesConfig

	foreach ($package in $dependencies.packages.package) {
		Write-Host "Adding package dependency $($package.id) (Version: $($package.version))"
		
		$newNuspecDependency = $nuspec.CreateElement('dependency')
		$dependencyId = $nuspec.CreateAttribute('id')
		$dependencyId.set_value($package.id)
		
		$dependencyVersion = $nuspec.CreateAttribute('version')
		$dependencyVersion.set_value($package.version)
		
		$newNuspecDependency.setAttributeNode($dependencyId) | Out-Null
		$newNuspecDependency.setAttributeNode($dependencyVersion) | Out-Null
		
		$nuspec.SelectSingleNode('//package/metadata/dependencies').AppendChild($newNuspecDependency) | Out-Null
	}

	$nuspec.Save($NuspecFile)
} catch {
	Write-Error $_
	##teamcity[buildStatus status='FAILURE' ]
    exit 1
}