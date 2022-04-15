param([String]$Server)

# --- Test external exe is available
$Msdeploy  = "C:/Program Files/IIS/Microsoft Web Deploy V3/msdeploy.exe"
$ServiceName = "GoogleSearchQueryDataDownload"
$CoreVersion = "3.1"
$USER = "Administrator"
$PASSWORD = "KakUph_Ba2ra"
$env,$Server = $Server.split('|');

# Build the expression, cater for spaces in exe path

$DestinationPath = "D:/live/WinServices/dotnet-core-services/$($ServiceName)Service"
$ResolvedPath = Invoke-Expression "Resolve-Path ."
$ResolvedPath = $ResolvedPath.ToString().Replace("\", "/")
$SourcePath = "$($ResolvedPath)/$($ServiceName)Service/bin/Release/netcoreapp$($CoreVersion)/win10-x64/publish"
$PostSyncPath = "$($ResolvedPath)/postsync.bat"
$Expression = "& '$($MsDeploy)' --% -verb:sync -source:dirPath='$($SourcePath)' -dest:dirPath='$($DestinationPath)',computerName=http://$($Server)/MSDeployAgentService,username='$($USER)',password='$($PASSWORD)' -allowUntrusted "
#Write-Host "cmd called:$($Expression)" -ForeGroundColor "Yellow"

# --- invoke the expression and check return code
Invoke-Expression $Expression
if ($LASTEXITCODE -ne 0)
{
     Write-Host "An error occurred during invocation of msdeploy, please review log : C:\temp\msdeploy.log" -ForeGroundColor "Red"
}
else
{
     Write-Host "Success" -ForeGroundColor "Green"
}