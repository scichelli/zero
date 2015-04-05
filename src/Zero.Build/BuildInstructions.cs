namespace Zero.Build
{
    using System;
    using System.IO;

    public class BuildInstructions : Nautilus.Framework.BuildInstructions
    {
        private const string PathToSolution = @".\src\Zero.sln";
        private const string OutputPath = @".\Output";
        
        public void Default()
        {
            Directory.CreateDirectory(OutputPath);
            DemonstrateFileIO();
            DemonstrateRunningACommand();
            CompileSolution(PathToSolution);
            Log("I AM RUN");
        }

        public void Other()
        {
            CompileSolution(PathToSolution);
            Log("I am the other thing");
        }

        public void Psake()
        {
            //CURRENT build.cmd CALL TO PSAKE:
            //@echo off
            //.\src\.nuget\nuget.exe install src\.nuget\packages.config -source "https://nuget.org/api/v2/" -RequireConsent -o "src\packages"
            //powershell -NoProfile -ExecutionPolicy Bypass -Command "& '%~dp0\src\packages\psake.4.3.2\tools\psake.ps1' %*; if ($psake.build_success -eq $false) { write-host "Build Failed!" -fore RED; exit 1 } else { exit 0 }"


            RunPowerShell(@".\src\packages\psake.4.3.2\tools\psake.ps1");
        }

        private static void DemonstrateFileIO()
        {
            File.WriteAllText(OutputPath + @"\CreatedByNautilus.txt", string.Format("created from Zero {0}", DateTime.Now.ToString()));
        }

        private void DemonstrateRunningACommand()
        {
            Exec("dir");
        }
    }
}
