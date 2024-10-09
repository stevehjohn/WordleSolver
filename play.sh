dotnet build -c Release src/WordleSolver.sln

clear

cd src/WordleSolver.Playground/bin/Release/net8.0/

dotnet WordleSolver.Playground.dll "$@"

cd -