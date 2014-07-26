namespace Zero.Build
{
    using System;
    using System.IO;
    using Nautilus.Framework;

    public class BuildInstructions : TaskRunner
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
