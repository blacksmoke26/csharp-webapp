################################################
#### REMOVE LOCKED FILE USING IObitUnlocker ####
################################################
### To execute this script:
### 1. Open powershell as Administrator
### 2. Paste 'Set-ExecutionPolicy RemoteSigned' without quotes.
### 3. Press enter and close, then run
### 4. powershell .\remove-locked-files.ps1
################################################

$dirMoviesApi = Resolve-Path "${PWD}/../src/Movies.Api/bin/Debug/net9.0" -ErrorAction Ignore

$files = @(
	Resolve-Path "${dirMoviesApi}/Movies.Api.dll" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Api.exe" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Api.pdb" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Application.dll" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Application.exe" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Application.pdb" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Contracts.dll" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Contracts.exe" -ErrorAction Ignore
	Resolve-Path "${dirMoviesApi}/Movies.Contracts.pdb" -ErrorAction Ignore
);

$filesPath = ('"' + ( $files -join '","') + '"');

if ('""' -eq $filesPath.Trim())
{
	return;
}

Write-Host "Removing files";
IObitUnlocker.exe /Delete /Advanced $filesPath;