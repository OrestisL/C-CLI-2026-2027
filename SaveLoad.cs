using System.IO;
using Newtonsoft.Json;

namespace Files;

public static class SaveLoad
{
    public static void Save<T>(string filePath, string fileName, T? data)
    {
        if (data == null)
            return;

        if (string.IsNullOrWhiteSpace(filePath))
            return;

        if(!Directory.Exists(filePath))
            return;

        string fullPath = Path.Combine(filePath, fileName);

        string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

        using StreamWriter writer = new (fullPath);
        writer.Write(jsonData);
    }

    public static bool TryLoad<T>(string fileDir, string fileName, out T? result)
    {
        result = default(T);
        if (string.IsNullOrWhiteSpace(fileDir))
            return false;

        if(!Directory.Exists(fileDir))
            return false;

        string fullPath = Path.Combine(fileDir, fileName);
        if (!File.Exists(fullPath))
            return false;

        using StreamReader reader = new (fullPath);
        string jsonData = reader.ReadToEnd();

        try
        {
            result = JsonConvert.DeserializeObject<T>(jsonData);
        }
        catch (JsonException)
        {
            Console.WriteLine($"Failed to deserialize JSON data from file '{fullPath}'.");
            return false;
        }

        return true;
    }

}
