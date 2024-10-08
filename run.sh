dotnet build -c Release src/WordleSolver.sln

dotnet src/WordleSolver.Console/bin/Release/net8.0/WordleSolver.Console.dll "$@"