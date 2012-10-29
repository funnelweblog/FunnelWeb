param([switch]$init, [switch]$updateConfig, [switch]$dontBuild)

$ErrorActionPreference = "Stop";

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$root = Get-ScriptDirectory

if ($dontBuild -ne $true)
{
	& $root\build.bat NOPAUSE
}

#Get Configuration File
if (!(Test-Path $root\GitRepoConfig.xml))
{
    $config = [xml]'<gitRepoConfig>
    <repository></repository>
</gitRepoConfig>'
    
    $config.Save("$root\GitRepoConfig.xml")
}
else
{
    $config = New-Object XML
    $config.Load("$root\GitRepoConfig.xml")
}

#Get repo
if ([string]::IsNullOrEmpty($config.gitRepoConfig.repository))
{
    $repo = Read-Host "Enter git repository url"
    $config.gitRepoConfig.repository = [string]$repo
    $config.Save("$root\GitRepoConfig.xml")
}
else
{
    $repo = $config.gitRepoConfig.repository
}

$x86git = "C:\Program Files (x86)\Git\bin\git.exe"
$git = "C:\Program Files (x86)\Git\bin\git.exe"
if (Test-Path "C:\Program Files (x86)\Git\bin\git.exe")
{
    $git = $x86git
}

$deployTempDir = "$env:temp\FunnelWebGitDeploy"
if (Test-Path "$deployTempDir")
{
    Remove-Item "$deployTempDir\*" -recurse -force
    Remove-Item "$deployTempDir" -force
}

if (!(Test-Path "$deployTempDir"))
{
    [System.IO.Directory]::CreateDirectory("$deployTempDir")
}

write-host "Temporary directory is $deployTempDir"
pushd "$deployTempDir"

if ($init -eq $false)
{
    & $git clone $repo .
	& $git checkout
}
else
{
    & $git init
	& $git remote add origin $repo
}
# copy new stuff over
copy "$root\build\published\*" "$deployTempDir" -recurse -force

if ($updateConfig -eq $false -and !(Test-Path "$deployTempDir\my.config"))
{
    $myConfig = New-Object XML
    $myConfig.Load("$deployTempDir\my.config.sample")
    
    $funnelWebUsername = Read-Host "SETUP: Enter your FunnelWeb Administrator Username"
    $funnelWebPassword = Read-Host "SETUP: Enter your FunnelWeb Administrator Password"
    
    foreach($setting in $myConfig.selectnodes("/funnelweb/setting"))
    {
      switch($setting.key)
      {
        "funnelweb.configuration.authentication.username" { $setting.SetAttribute("value", $funnelWebUsername) }
        "funnelweb.configuration.authentication.password" { $setting.SetAttribute("value", $funnelWebPassword) }
      } 
    }

    $myConfig.Save("$deployTempDir\my.config")
}

if ($updateConfig -eq $true)
{
	write-host "Opening my.config for editing before deployment"
	& notepad $deployTempDir\my.config | Out-Null
}

### Compatibility Steps
#Remove Obsolete Extensions


#Update repo
& $git add "-A"
& $git commit "-m Update FunnelWeb build"
& $git push origin master

popd

