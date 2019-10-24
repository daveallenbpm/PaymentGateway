# Install chocolatey if not installed
Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

choco install sqlite

if (![System.IO.File]::Exists(".\PaymentGateway\Database\Database.db")) {
	sqlite3 .\PaymentGateway\Database\Database.db
}

# Install ef if not already there.
dotnet tool install --global dotnet-ef

dotnet ef database update
