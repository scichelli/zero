Framework '4.0'

properties {
    $name = "Zero"
    $birthYear = 2014
    $maintainers = "Patrick Lioi"
    $description = ""

    $configuration = 'Debug'
    $src = resolve-path '.\src'
    $build = if ($env:build_number -ne $NULL) { $env:build_number } else { '0' }
    $version = [IO.File]::ReadAllText('.\VERSION.txt') + '.' + $build
    $projects = @(gci $src -rec -filter *.csproj)
}

task default -depends Test

task Test -depends Compile {
    $fixieRunners = @(gci $src\packages -rec -filter Fixie.Console.exe)

    if ($fixieRunners.Length -ne 1) {
        throw "Expected to find 1 Fixie.Console.exe, but found $($fixieRunners.Length)."
    }

    $fixieRunner = $fixieRunners[0].FullName

    foreach ($project in $projects) {
        $projectName = [System.IO.Path]::GetFileNameWithoutExtension($project)

        if ($projectName.EndsWith("Tests")) {
            $testAssembly = "$($project.Directory)\bin\$configuration\$projectName.dll"
            exec { & $fixieRunner $testAssembly }
        }
    }
}

task Compile -depends AssemblyInfo {
  exec { msbuild /t:clean /v:q /nologo /p:Configuration=$configuration $src\$name.sln }
  exec { msbuild /t:build /v:q /nologo /p:Configuration=$configuration $src\$name.sln }
}

task AssemblyInfo {
    $date = Get-Date
    $year = $date.Year
    $copyrightSpan = if ($year -eq $birthYear) { $year } else { "$birthYear-$year" }

    foreach ($project in $projects) {
        $title = [System.IO.Path]::GetFileNameWithoutExtension($project)

"using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: AssemblyProduct(""$name"")]
[assembly: AssemblyTitle(""$title"")]
[assembly: AssemblyConfiguration(""$configuration"")]

[assembly: AssemblyCopyright(""Copyright © $copyrightSpan $maintainers"")]
[assembly: AssemblyVersion(""$version"")]
[assembly: AssemblyDescription(""$description"")]
[assembly: AssemblyFileVersion(""$version"")]" | out-file "$($project.DirectoryName)\Properties\AssemblyInfo.cs" -encoding "UTF8"
    }
}