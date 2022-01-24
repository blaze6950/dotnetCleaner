Console.WriteLine("Hello, .Net developer!");

string firstArgument;

if (args.Length == 0)
{
    var input = Console.ReadLine()!;
    if (input.Contains("\""))
    {
        firstArgument = input.Split(" ")[0];
        var indexOfArgumentValue = input.IndexOf("\"");
        var secondArgument = input[indexOfArgumentValue..];
        args = new[]
        {
            firstArgument,
            secondArgument.Trim('\"')
        };
    }
    else
    {
        args = input.Split(" ");
    }
}

firstArgument = args[0];

switch (firstArgument)
{
    case     "-h":
    case "--help":
        DisplayHelp();
        break;
    case     "-p":
    case "--path":
        var rootPathArgument = args[1];
        Clean(rootPathArgument);
        break;
    default:
        Console.WriteLine("Unknown argument. Use help '-h' or '--help'.");
        break;
}

Console.WriteLine("For closing the app press any key");
Console.ReadKey();

static void Clean(string rootPath)
{
    if (!Directory.Exists(rootPath))
    {
        Console.WriteLine($"The specified path is wrong: {rootPath}");
        return;
    }

    var listOfTempDirectoriesToDelete = GetTempDirectories(rootPath).ToList();

    Console.WriteLine("Do you approve the deletion of these directories? yes to all - [Y], no - [N], ask on every directory - [A]");
    var approve = Console.ReadLine();

    if (approve.Contains("N", StringComparison.OrdinalIgnoreCase))
    {
        return;
    }

    foreach (var tempDirectory in listOfTempDirectoriesToDelete)
    {
        try
        {
            if (approve.Contains("A", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Delete \"{tempDirectory}\"? yes - [Y], no - [N]");
                if (Console.ReadLine().Contains("N", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"\"{tempDirectory}\" was skipped");
                    continue;
                }
            }
            Directory.Delete(tempDirectory, true);
            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("Successfully removed: ");
            Console.ResetColor();
            Console.WriteLine(tempDirectory);
        }
        catch (Exception e)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("Failed Deleting: ");
            Console.ResetColor();
            Console.WriteLine(e.Message);
        }
    }
}

static IEnumerable<string> GetTempDirectories(string rootPath)
{
    var nodeModules = Directory.EnumerateDirectories(rootPath, "node_modules", new EnumerationOptions
    {
        RecurseSubdirectories = true,
        ReturnSpecialDirectories = true,
        AttributesToSkip = FileAttributes.ReadOnly
    }).Where(p => p.IndexOf("node_modules", StringComparison.OrdinalIgnoreCase) == p.LastIndexOf("node_modules", StringComparison.OrdinalIgnoreCase));

    var vsFolders = Directory.EnumerateDirectories(rootPath, ".vs", new EnumerationOptions
    {
        RecurseSubdirectories = true,
        ReturnSpecialDirectories = true,
        AttributesToSkip = FileAttributes.ReadOnly
    });

    var binFolders = Directory.EnumerateDirectories(rootPath, "bin", new EnumerationOptions
    {
        RecurseSubdirectories = true,
        ReturnSpecialDirectories = true,
        AttributesToSkip = FileAttributes.ReadOnly
    });

    var objFolders = Directory.EnumerateDirectories(rootPath, "obj", new EnumerationOptions
    {
        RecurseSubdirectories = true,
        ReturnSpecialDirectories = true,
        AttributesToSkip = FileAttributes.ReadOnly
    });

    var buildFolders = Directory.EnumerateDirectories(rootPath, "build", new EnumerationOptions
    {
        RecurseSubdirectories = true,
        ReturnSpecialDirectories = true,
        AttributesToSkip = FileAttributes.ReadOnly
    }).Where(p => !p.Contains("node_modules"));

    Console.WriteLine("The list of found temp directories:");

    return nodeModules.Concat(vsFolders).Concat(binFolders).Concat(objFolders).Concat(buildFolders).Select(p =>
    {
        var indexOfLastSlash = p.LastIndexOf('\\');

        var firstSegment = p[..indexOfLastSlash];
        var secondSegment = p[(indexOfLastSlash + 1)..];

        Console.Write(firstSegment + "\\");
        Console.BackgroundColor = ConsoleColor.Blue;
        Console.Write(secondSegment);
        Console.ResetColor();
        Console.WriteLine();
        return p;
    });
}

static void DisplayHelp()
{
    Console.WriteLine("This is the CLI of the dotnetCleaner tool. Here is the list of supported arguments for it:");
    Console.WriteLine("\t-p {path of the root directory}");
    Console.WriteLine("\t\t The specified directory will be recursively cleaned up from temp files and folders.");
    Console.WriteLine("\t\t Temp folders are: '/node_modules/', '/build/', '/bin/', '/obj/', '/.vs/'.");
}