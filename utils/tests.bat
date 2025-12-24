:: Echo %1
dotnet tool install -g dotnet-reportgenerator-globaltool
rd coverage /S /Q
cd ..
FOR /D /r %%G in ("tests/*.Tests") DO (
	Echo We found %%~nxG
	cd %%~nxG
	dir /b /a-d
	rd TestResults /S /Q
	dotnet test --collect:"XPlat Code Coverage"
	cd ..
)

:: Retarget back to the current directory
cd /D "%~dp0"

reportgenerator "-reports:../tests/**/coverage.cobertura.xml" "-targetdir:./coverage" "-reporttypes:Html"
start ./coverage/index.html

pause