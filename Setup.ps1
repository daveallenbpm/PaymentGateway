# Checks whether a given command has been installed
function Check-Installed($command) {
	return (Get-Command $command -ErrorAction SilentlyContinue)
}

if (!(Check-Installed("sqlite3"))) {
	if (!(Check-Installed("choco"))) {
		# Install chocolatey and refresh path
		Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
		$env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
	}

	choco install sqlite
}

if (!([System.IO.File]::Exists('.\PaymentGateway\Database\Database.db'))) {
	echo "Creating database"
	sqlite3 .\PaymentGateway\Database\Database.db "VACUUM;"
}

if (!(Check-Installed("dotnet-ef"))) {
	dotnet tool install --global dotnet-ef
}

cd .\PaymentGateway\
echo "Updating database"
dotnet ef database update
cd ..\
