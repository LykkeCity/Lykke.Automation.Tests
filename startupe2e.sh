#!/bin/bash
echo "start tests from cont"

echo $JAVA_HOME

dotnet test ./AFTests/AFTests.csproj --filter TestCategory=E2ESample
echo "genarate report"
cd ./AFTests/bin/Debug/netcoreapp2.0/
echo "execute allure"
bash ../../../../XUnitTestCommon/Reports/allure-cli/bin/allure generate --clean
echo "test run finished"
echo "Sleeping..."
sleep infinity