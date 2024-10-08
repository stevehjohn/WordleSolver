dotnet build -c Release src/WordleSolver.sln

clear

cd src/WordleSolver.Console/bin/Release/net8.0/

dotnet WordleSolver.Console.dll "$@"

cd -