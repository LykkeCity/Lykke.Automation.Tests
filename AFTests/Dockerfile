FROM microsoft/dotnet

WORKDIR /app
COPY . .

RUN dotnet restore


# run tests on docker run
ENTRYPOINT ["dotnet", "test", "./AFTests/AFTests.csproj", "--filter", "TestCategory=E2E"]