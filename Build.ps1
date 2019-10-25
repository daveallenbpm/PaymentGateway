dotnet test --filter Category!=Integration

if ($lastexitcode -ne 0) {
	echo 'TestsFailed! Aborting build.'
}
else {
	dotnet build
}
