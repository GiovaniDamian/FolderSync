using System;
using System.IO;
using System.Threading;

class Program
{
    static string sourceDirectory;
    static string replicaDirectory;
    static string logFilePath;
    static int syncInterval;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the folder synchronization program!");

        GetInputs();

        Console.WriteLine("Starting synchronization...");
        Console.WriteLine($"Source: {sourceDirectory}");
        Console.WriteLine($"Replica: {replicaDirectory}");
        Console.WriteLine($"Interval: {syncInterval} seconds");
        Console.WriteLine($"Log: {logFilePath}");

        Thread inputThread = new Thread(InputListener);
        inputThread.IsBackground = true;
        inputThread.Start();

        while (true)
        {
            int changes = SynchronizeFolders(sourceDirectory, replicaDirectory);
            if (changes > 0)
            {
                Console.WriteLine($"Synchronization completed. {changes} files were modified/added/removed.");
            }
            else
            {
                Console.WriteLine("No changes detected. Synchronization skipped.");
            }
            Thread.Sleep(syncInterval * 1000);
        }
    }

    static void GetInputs()
    {
        Console.Write("Enter the source folder path: ");
        sourceDirectory = Console.ReadLine();

        Console.Write("Enter the replica folder path: ");
        replicaDirectory = Console.ReadLine();

        Console.Write("Enter the synchronization interval (in seconds): ");
        syncInterval = int.Parse(Console.ReadLine());

        Console.Write("Enter the log file path: ");
        logFilePath = Console.ReadLine();
    }

    static int SynchronizeFolders(string sourceDir, string replicaDir)
    {
        int changesDetected = 0;
        try
        {
            foreach (string sourceFile in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDir, sourceFile);
                string replicaFile = Path.Combine(replicaDir, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(replicaFile));

                if (!File.Exists(replicaFile) || File.GetLastWriteTimeUtc(sourceFile) > File.GetLastWriteTimeUtc(replicaFile))
                {
                    File.Copy(sourceFile, replicaFile, true);
                    Log($"File copied: {sourceFile} -> {replicaFile}");
                    Console.WriteLine($"File copied: {sourceFile} -> {replicaFile}");
                    changesDetected++;
                }
            }

            foreach (string replicaFile in Directory.GetFiles(replicaDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(replicaDir, replicaFile);
                string sourceFile = Path.Combine(sourceDir, relativePath);

                if (!File.Exists(sourceFile))
                {
                    File.Delete(replicaFile);
                    Log($"File removed from replica: {replicaFile}");
                    Console.WriteLine($"File removed from replica: {replicaFile}");
                    changesDetected++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during synchronization: {ex.Message}");
        }

        foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
        {
            string relativePath = Path.GetRelativePath(sourceDir, sourceSubDir);
            string replicaSubDir = Path.Combine(replicaDir, relativePath);

            if (!Directory.Exists(replicaSubDir))
            {
                Directory.CreateDirectory(replicaSubDir);
                Log($"Folder created in replica: {replicaSubDir}");
                Console.WriteLine($"Folder created in replica: {replicaSubDir}");
                changesDetected++;
            }

            changesDetected += SynchronizeFolders(sourceSubDir, replicaSubDir);
        }
        return changesDetected;
    }

    static void InputListener()
    {
        Console.WriteLine("Press Enter to exit the program.");
        while (true)
        {
            if (Console.ReadKey().Key == ConsoleKey.Enter)
            {
                Environment.Exit(0);
            }
        }
    }

    static void Log(string message)
    {
        try
        {
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
            using (StreamWriter sw = File.AppendText(logFilePath))
            {
                sw.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to log file: {ex.Message}");
        }
    }
}

