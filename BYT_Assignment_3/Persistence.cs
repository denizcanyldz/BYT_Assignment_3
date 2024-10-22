using System.Text.Json;
public abstract class Persistence<T> where T : class
{
    public static void Save(string filePath, List<T> data)
    {
        string json = JsonSerializer.Serialize(data);
        File.WriteAllText(filePath, json);
    }

    public static List<T> Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(json);
        }
        return new List<T>();
    }
}
