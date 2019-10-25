
# Checks whether a given command has been installed
function Check-Installed($command) {
	return (Get-Command $command -ErrorAction SilentlyContinue)
}

# Install chocolatey if not installed
if (!Check-Installed("choco")) {
	# Install chocolatey and refresh path
	Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
	$env:Path = [System.Environment]::GetEnvironmentVariable("Path", "Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path", "User")
}

if (!Check-Installed("sqlite3")) {
	choco install sqlite
}


if (![System.IO.File]::Exists(".\PaymentGateway\Database\Database.db")) {
	echo "Installing database"
	sqlite3 .\PaymentGateway\Database\Database.db
	.exit
}

# Install ef if not already there.
if (!Check-Installed("dotnet-ef")) {
	dotnet tool install --global dotnet-ef
}

cd .\PaymentGateway\
echo "Updating database"
dotnet ef database update
cd ..\
