Param(
[string]$projectPath,
[string]$outputPath,
[switch]$concatenateFiles=$false
)
$currentPath = Split-Path $PSScriptRoot
$outputDefaultPath = $PSScriptRoot
$all_scripts_filename = "allscripts.sql"
if ($projectPath -eq '') { $projectPath = $currentPath + '\Sticos.Backend\' }
if ($outputPath -eq '') { $outputPath = $outputDefaultPath }
cd $projectPath\Timereg\Timereg.Api.Repositories;
dotnet ef migrations script --idempotent --output "$outputPath\timeregScript.sql" --context  TimeregDbContext;
cd $projectPath\Integrations\Integrations.Api.Repositories;
dotnet ef migrations script --idempotent --output "$outputPath\integrationScript.sql" --context  IntegrationDbContext;
cd  $projectPath\Altinn\Altinn.Api.Repositories;
dotnet ef migrations script --idempotent --output "$outputPath\altinnScript.sql" --context  AltinnDbcontext;
cd $projectPath\Common\Common.Api.Repositories;
dotnet ef migrations script --idempotent --output "$outputPath\sticoswidgetScript.sql" --context  SticosWidgetDbContext;
cd $PSScriptRoot
if($concatenateFiles -eq $true){ Get-Content *.sql -Exclude $all_scripts_filename | Set-Content $all_scripts_filename; Remove-Item * -Include *.sql -Exclude  $all_scripts_filename;}
