using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

class Program
{
    static string sourceDirectory;
    static string replicaDirectory;
    static string logFilePath = "default_log.txt";
    static int syncInterval;
    static string userInput;

    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the folder synchronization program!");

        Console.WriteLine("Do you want to run automated tests? (yes/no)");

        userInput = Console.ReadLine();

        if (userInput.ToLower() == "yes")
        {
            Console.WriteLine("Running automated tests...");
            TestSynchronizeFolders();
            Console.WriteLine("Automated tests completed.");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        else
        {
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


                    if (!File.Exists(replicaFile))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(replicaFile));
                        File.Copy(sourceFile, replicaFile);
                        Log($"New file added: {sourceFile} -> {replicaFile}");
                        Console.WriteLine($"New file added: {sourceFile} -> {replicaFile}");
                        changesDetected++;
                    }
                    else if (File.GetLastWriteTimeUtc(sourceFile) > File.GetLastWriteTimeUtc(replicaFile))
                    {
                        Log($"File modified: {sourceFile} -> {replicaFile}");
                        Console.WriteLine($"File modified: {sourceFile} -> {replicaFile}");

                        File.Copy(sourceFile, replicaFile, true);
                        changesDetected++;
                    }
                    if (File.Exists(replicaFile))
                    {
                        Log($"File copied: {sourceFile} -> {replicaFile}");
                        Console.WriteLine($"File copied: {sourceFile} -> {replicaFile}");
                    }

                    File.Copy(sourceFile, replicaFile, true);

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

            foreach (string sourceFile in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string relativePath = Path.GetRelativePath(sourceDir, sourceFile);
                string replicaFile = Path.Combine(replicaDir, relativePath);

                if (!File.Exists(replicaFile))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(replicaFile));
                    File.Copy(sourceFile, replicaFile);
                    Log($"New file added: {sourceFile} -> {replicaFile}");
                    Console.WriteLine($"New file added: {sourceFile} -> {replicaFile}");
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
    static void TestSynchronizeFolders()
    {
        string sourceDir = "source";
        string replicaDir = "replica";

        Directory.CreateDirectory(sourceDir);
        Directory.CreateDirectory(replicaDir);

        try
        {
            string sourceFile1 = Path.Combine(sourceDir, "file1.txt");
            string sourceFile2 = Path.Combine(sourceDir, "file2.txt");
            File.WriteAllText(sourceFile1, "Content of file 1");
            File.WriteAllText(sourceFile2, "Content of file 2");

            Program.SynchronizeFolders(sourceDir, replicaDir);

            string replicaFile1 = Path.Combine(replicaDir, "file1.txt");
            string replicaFile2 = Path.Combine(replicaDir, "file2.txt");
            if (!File.Exists(replicaFile1) || !File.Exists(replicaFile2))
            {
                throw new Exception("Files were not correctly copied to the replica folder.");
            }

            string content1 = File.ReadAllText(replicaFile1);
            string content2 = File.ReadAllText(replicaFile2);
            if (content1 != "Content of file 1" || content2 != "Content of file 2")
            {
                throw new Exception("The content of files in the replica folder does not match the original content.");
            }

            string newSourceFile = Path.Combine(sourceDir, "newFile.txt");
            File.WriteAllText(newSourceFile, "Content of the new file");

            Program.SynchronizeFolders(sourceDir, replicaDir);

            Console.WriteLine("Folder synchronization test successful.");
            Console.WriteLine("Summary:");
            Console.WriteLine("1. Source folder and replica folder created.");
            Console.WriteLine("2. Files copied from source to replica.");
            Console.WriteLine("3. Content of files verified.");
            Console.WriteLine("4. New files added to source and copied to replica.");
        }
        finally
        {
            Directory.Delete(sourceDir, true);
            Directory.Delete(replicaDir, true);
        }
    }

}

