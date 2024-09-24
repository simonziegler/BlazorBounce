@echo on
if exist "api" rd "api" /s /q
if exist "_site" rd "_site" /s /q
dotnet tool update -g docfx
dotnet build BlazorBounce/BlazorBounce.csproj --configuration Debug
docfx docfx.json --serve
if exist "api" rd "api" /s /q
if exist "_site" rd "_site" /s /q
