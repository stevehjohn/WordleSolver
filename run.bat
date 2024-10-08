dotnet build -c Release src/WordleSolver.sln

cls

pushd

cd src/WordleSolver.Console/bin/Release/net8.0/

dotnet WordleSolver.Console.dll "$@"

popd