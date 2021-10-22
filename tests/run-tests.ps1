<#
    Based on https://github.com/bUnit-dev/bUnit/blob/main/tests/run-tests.ps1 by Egil Hansen https://github.com/egil

    Sample usage:
    - will run 5 times in Release configuration for net6
	.\run-tests.ps1 5 Release net6

    - will run once in Debug configuration for default dotnet version
	.\run-tests.ps1 1 Release 
#>

$maxRuns = $args[0]
$mode = $args[1]
$framework = $args[2]
if ($framework.Length.Equals(0)) 
{
    $framework = "net5"
}

$filter = $args[3]

dotnet build ..\AntDesign.sln -c $mode --nologo

for ($num = 1 ; $num -le $maxRuns ; $num++)
{
    Write-Output "### TEST RUN $num ###"
	
	if($filter)
	{
		dotnet test .\AntDesign.Tests\AntDesign.Tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f $framework --nologo --filter $filter
	}
	else
	{
		dotnet test .\AntDesign.Tests\AntDesign.Tests.csproj -c $mode --no-restore --no-build --blame-hang --blame-hang-timeout 100s -f $framework --nologo
	}
}