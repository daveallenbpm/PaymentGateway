dotnet test

if ($lastexitcode -ne 0) {
	echo 'TestsFailed! Aborting build.'
}
else {
	dotnet build
	# dotnet run --project .\PaymentGateway\PaymentGateway.csproj
}
