using System.IO;
namespace Files;

public static class FileOperations
{
    public static string CurrentDir { get { return Directory.GetCurrentDirectory(); } }
    public static string DataDir 
    { 
        get 
        { 
            string dataPath = Path.Combine(CurrentDir, DataFileName);
            if(!Directory.Exists(dataPath))
                Directory.CreateDirectory(dataPath);

            return dataPath; 
        } 
    }
    
    public const string DataFileName = "Data";

    public static void WriteToFile(string filePath, string fileName, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return;

        if (string.IsNullOrWhiteSpace(filePath))
            return;

        if(!Directory.Exists(filePath))
        {
            Console.WriteLine($"Directory '{filePath}' does not exist. Creating it.");
            Directory.CreateDirectory(filePath);
        }

        string fullPath = Path.Combine(filePath, fileName);

        using StreamWriter writer = new (fullPath);
        writer.Write(content);
    }

    public static bool TryReadFromFile(string fileDir, string fileName, out string result)
    {
        result = "";
        if (string.IsNullOrWhiteSpace(fileDir))
            return false;

        if(!Directory.Exists(fileDir))
            return false;

        string fullPath = Path.Combine(fileDir, fileName);
        if (!File.Exists(fullPath))
            return false;

        using StreamReader reader = new (fullPath);
        result = reader.ReadToEnd();

        return true;
    }
}
