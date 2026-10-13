using GeometryLib;
using Files;
using System.Collections.Generic;

public class Program
{
    private static List<Point2d> points2d = [];
    public static void Main(string[] args)
    {      
        string fileName = "line.txt";
        Line2d line = new Line2d(new Point2d(0, 0), new Point2d(3, 4));
        string saveDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        SaveLoad.Save(saveDir, fileName, line);

        if(SaveLoad.TryLoad(FileOperations.DataDir, fileName, out Line2d? loadedLine))
        {
            Console.WriteLine($"Loaded line: {loadedLine}");
        }
        else
        {
            Console.WriteLine("Failed to load the line from file.");
        }

        points2d.Add(new Point2d(0, 0));
        points2d.Add(new Point2d(3, 4));
        points2d.Add(new Point2d(6, 8));
        points2d.Add(new Point2d(12, 16));
        points2d.Add(new Point2d(24, 32));

        Polyline2d polyline = Polyline2d.CopyPoints(points2d);
        Console.WriteLine($"Polyline length: {polyline.Length()}");
        polyline.Points.Add(new Point2d(48, 64));
        Console.WriteLine($"Updated polyline length: {polyline.Length()}");

        SaveLoad.Save(FileOperations.DataDir, "polyline.json", polyline);
    }

    private static void Old(string[] args)
    {
            foreach (var arg in args)
        {
            Console.WriteLine($"Argument: {arg}");
            if(double.TryParse(arg, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("en-US"), out double value))
            {
                Console.WriteLine($"Parsed value: {value}");
            }
            else
            {
                //string newArg = arg.Replace(',', '.');
                if(double.TryParse(arg, System.Globalization.NumberStyles.Float, new System.Globalization.CultureInfo("fr-FR"), out double newValue))
                {
                    Console.WriteLine($"Parsed value after replacing comma: {newValue}");
                }
                else
                {
                    Console.WriteLine($"Could not parse '{arg}' as a double.");
                }
            }
        }
        try
        {
            Console.WriteLine("Please enter your name:");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Guest";
            }

            Console.WriteLine("Please enter your age:");
            string? ageInput = Console.ReadLine();
            int age = ParseAge(ageInput);

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"Enter coordinates for point {i + 1} (format: x,y):");
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var parts = input.Split(',');
                    if (parts.Length == 2 &&
                        double.TryParse(parts[0], out double x) &&
                        double.TryParse(parts[1], out double y))
                    {
                        points2d.Add(new Point2d(x, y));
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter coordinates in the format x,y.");
                        i--; // Decrement to repeat this iteration
                    }
                }
                else
                {
                    Console.WriteLine("Input cannot be empty. Please enter coordinates.");
                    i--; // Decrement to repeat this iteration
                }
            }

            List<Vector2d> vectors = new List<Vector2d>();
            for (int i = 0; i < points2d.Count - 1; i++)
            {
                Vector2d vector = points2d[i + 1] - points2d[i];
                Console.WriteLine($"Vector from Point {i + 1} to Point {i + 2}: {vector}, Length: {vector.Length:F3}");
                vectors.Add(vector);
            }

            for (int i = 0; i < vectors.Count - 1; i++)
            {
                double angle = Vector2d.AngleBetween(vectors[i], vectors[i + 1]);
                Console.WriteLine($"Angle between Vector {i + 1} and Vector {i + 2}: {angle:F3} degrees");
            }

            Console.WriteLine("Removing duplicate points from list...");
            int countBefore = points2d.Count;
            points2d = [.. points2d.Distinct()];
            int countAfter = points2d.Count;
            Console.WriteLine($"Points before removal: {countBefore}, Points after removal: {countAfter}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Caught an exception: {e.Message}. Stacktrace: {e.StackTrace}");
        }
    }
    private static int ParseAge(string? ageInput)
    {
        if (int.TryParse(ageInput, out int age))
        {
            return age;
        }
        else
        {
            Console.WriteLine("Invalid age input. Defaulting to 20.");
            return 20;
        }
    }
}

public class Student
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Student(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return $"{Name}, Age: {Age}";
    }
}