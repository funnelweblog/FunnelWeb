$ErrorActionPreference = "Stop";

function Get-ScriptDirectory
{
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value
    Split-Path $Invocation.MyCommand.Path
}

$root = Get-ScriptDirectory

#Get Configuration File
if (!(Test-Path $root\AppHarbourConfig.xml))
{
    $config = [xml]'<appHarbourConfig>
    <repository></repository>
</appHarbourConfig>'
    
    $config.Save("$root\AppHarbourConfig.xml")
}
else
{
    $config = New-Object XML
    $config.Load("$root\AppHarbourConfig.xml")
}

#Get repo
if ([string]::IsNullOrEmpty($config.appHarbourConfig.repository))
{
    $repo = Read-Host "Enter AppHarbour git repository url"
    $config.appHarbourConfig.repository = [string]$repo
    $config.Save("$root\AppHarbourConfig.xml")
}
else
{
    $repo = $config.appHarbourConfig.repository
}

$x86git = "C:\Program Files (x86)\Git\bin\git.exe"
$git = "C:\Program Files (x86)\Git\bin\git.exe"
if (Test-Path "C:\Program Files (x86)\Git\bin\git.exe")
{
    $git = $x86git
}

if (Test-Path "$env:temp\FunnelWebAppHarbour")
{
    Remove-Item "$env:temp\FunnelWebAppHarbour\*" -recurse
}

if (!(Test-Path "$env:temp\FunnelWebAppHarbour"))
{
    [System.IO.Directory]::CreateDirectory("$env:temp\FunnelWebAppHarbour")
}
pushd "$env:temp\FunnelWebAppHarbour"
. $git clone $repo .
. $git checkout

# copy new stuff over
copy "$root\build\published\*" "$env:temp\FunnelWebAppHarbour" -recurse -force

if (!(Test-Path "$env:temp\FunnelWebAppHarbour\my.config"))
{
    $myConfig = New-Object XML
    $myConfig.Load("$env:temp\FunnelWebAppHarbour\my.config.sample")
    
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
     
     $myConfig.Save("$env:temp\FunnelWebAppHarbour\my.config")
}

### Compatibility Steps
#Remove Obsolete Extensions


#Update repo
& $git add "-A"
& $git commit "-m Update FunnelWeb build"
& $git push

popd

