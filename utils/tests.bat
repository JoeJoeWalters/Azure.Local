:: Echo %1
dotnet tool install -g dotnet-reportgenerator-globaltool
rd coverage /S /Q
cd ..

:: Clean up old test results
FOR /D /r %%G in (TestResults) DO (
	Echo We found %%G
	rd /S /Q %%G
)

:: Find any tests to run against to collect coverage
FOR /D /r %%G in ("tests/*.Tests") DO (
	Echo We found %%G
	cd %%G
	Echo Moved into directory %%G
	:: https://rjj-software.co.uk/blog/excluding-auto-generated-code-from-net-10-coverage-reports/
	dotnet test --collect:"XPlat Code Coverage" --settings:./codecoverage.runsettings
	cd ..
)

:: Retarget back to the current directory
cd /D "%~dp0"
reportgenerator "-reports:../tests/**/coverage.cobertura.xml" "-targetdir:./coverage" "-reporttypes:Html"
start ./coverage/index.html

pause