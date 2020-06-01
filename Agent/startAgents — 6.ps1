#PS C:\studia\semest6\project-game\Agent>
invoke-expression 'cmd /c start powershell -Command { dotnet run --project Agent -- .\Agent\Configuration\DefaultRedConfig.txt; Read-Host }';
Start-Sleep -s 1;
invoke-expression 'cmd /c start powershell -Command { dotnet run --project Agent -- .\Agent\Configuration\DefaultRedConfig.txt; Read-Host }';
Start-Sleep -s 1;
invoke-expression 'cmd /c start powershell -Command { dotnet run --project Agent -- .\Agent\Configuration\DefaultBlueConfig.txt; Read-Host }';
Start-Sleep -s 1;
invoke-expression 'cmd /c start powershell -Command { dotnet run --project Agent -- .\Agent\Configuration\DefaultBlueConfig.txt; Read-Host }';
Start-Sleep -s 1;
invoke-expression 'cmd /c start powershell -Command { dotnet run --project Agent -- .\Agent\Configuration\DefaultRedConfig.txt; Read-Host }';
Start-Sleep -s 1;
invoke-expression 'cmd /c start powershell -Command { dotnet run --project Agent -- .\Agent\Configuration\DefaultBlueConfig.txt; Read-Host }';
