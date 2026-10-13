namespace GeometryLib;

using System.Drawing;
using Newtonsoft.Json;

public static class Constants
{
    public const double ToDegrees = 180.0 / Math.PI;
    public const double ToRadians = Math.PI / 180.0;
    public const double AbsoluteZeroTolerance = 1e-12; // units
    private static double _tolerance = 1e-3; // units
    public static double Tolerance {
        get { return _tolerance; } 
        set
        {
            _tolerance = Math.Max(value, AbsoluteZeroTolerance);
        }
    }
}

public class Point2d(double x, double y)
{
    public static Point2d Origin { get { return new Point2d(0, 0); } }
    public double X { get; private set; } = x;
    public double Y { get; private set; } = y;

    public override string ToString()
    {
        return $"({X:F3}, {Y:F3})";
    }

    public static Vector2d operator- (Point2d end, Point2d start)
    {
        return new Vector2d(end.X - start.X, end.Y - start.Y);
    }

    public static Vector2d operator+ (Point2d end, Point2d start)
    {
        return new Vector2d(end.X + start.X, end.Y + start.Y);
    }

    public static bool operator== (Point2d point1, Point2d point2)
    {
        if (point1 is null && point2 is null) return true;
        if (point1 is null || point2 is null) return false;
        
        return Math.Abs(point1.X - point2.X) < Constants.Tolerance &&
               Math.Abs(point1.Y - point2.Y) < Constants.Tolerance;
    }

    public static bool operator!= (Point2d point1, Point2d point2)
    {
        return !(point1 == point2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Point2d otherPoint)
        {
            return this == otherPoint;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}

public class Vector2d(double x, double y)
{
    public static Vector2d Zero { get { return new Vector2d(0, 0); } }
    public static Vector2d One { get { return new Vector2d(1, 1); } }
    public static Vector2d XAxis { get { return new Vector2d(1, 0); } }
    public static Vector2d YAxis { get { return new Vector2d(0, 1); } }

    public double X { get; private set; } = x;
    public double Y { get; private set; } = y;
    public double Length { get { return Math.Sqrt(X * X + Y * Y); } }

    public Vector2d Normalize()
    {
        if (Length == 0)
        {
            throw new InvalidOperationException("Cannot normalize a zero-length vector.");
        }
        return new Vector2d(X / Length, Y / Length);
    }

    public override string ToString()
    {
        return $"<{X:F3}, {Y:F3}>";
    }

    public static double AngleBetween(Vector2d? vector1, Vector2d? vector2)
    {
        ArgumentNullException.ThrowIfNull(vector1);
        ArgumentNullException.ThrowIfNull(vector2);

        double dotProduct = vector1 * vector2;
        double lengthsProduct = vector1.Length * vector2.Length;

        if (lengthsProduct == 0)
        {
            throw new InvalidOperationException("Cannot calculate angle with a zero-length vector.");
        }

        double cosTheta = dotProduct / lengthsProduct;
        return Math.Acos(cosTheta) * Constants.ToDegrees; // Convert to degrees
    }

// operators
    public static Vector2d operator- (Vector2d end, Vector2d start)
    {
        return new Vector2d(end.X - start.X, end.Y - start.Y);
    }

    public static Vector2d operator+ (Vector2d end, Vector2d start)
    {
        return new Vector2d(end.X + start.X, end.Y + start.Y);
    }

    public static Vector2d operator* (Vector2d vector, double scalar)
    {
        return new Vector2d(vector.X * scalar, vector.Y * scalar);
    }

    public static Vector2d operator* (double scalar, Vector2d vector)
    {
        return new Vector2d(vector.X * scalar, vector.Y * scalar);
    }

    public static Vector2d operator/ (Vector2d vector, double scalar)
    {
        if (scalar == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        return new Vector2d(vector.X / scalar, vector.Y / scalar);
    }

    public static Vector2d operator/ (double scalar, Vector2d vector)
    {
        if (scalar == 0)
        {
            throw new DivideByZeroException("Cannot divide by zero.");
        }

        return new Vector2d(vector.X / scalar, vector.Y / scalar);
    }

    public static double operator* (Vector2d vector1, Vector2d vector2)
    {
        return vector1.X * vector2.X + vector1.Y * vector2.Y;
    }

    public static bool operator== (Vector2d vec1, Vector2d vec2)
    {
        if (vec1 is null && vec2 is null) return true;
        if (vec1 is null || vec2 is null) return false;
        
        return Math.Abs(vec1.X - vec2.X) < Constants.Tolerance &&
               Math.Abs(vec1.Y - vec2.Y) < Constants.Tolerance;
    }

    public static bool operator!= (Vector2d vec1, Vector2d vec2)
    {
        return !(vec1 == vec2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Vector2d otherVector)
        {
            return this == otherVector;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}

public class Line2d(Point2d startPoint, Point2d endPoint)
{
    [JsonRequired]
    public Point2d StartPoint { get; private set; } = startPoint;
    [JsonRequired]
    public Point2d EndPoint { get; private set; } = endPoint;

    public double Length()
    {
        Vector2d vector = EndPoint - StartPoint;
        return vector.Length;        
    }

    public override string ToString()
    {
        return $"Line from {StartPoint} to {EndPoint}";
    }
}

/// <summary>
/// Default constructor does not copy input points
/// </summary>
public class Polyline2d(List<Point2d> points)
{
    public List<Point2d> Points { get; set; } = points;

    /// <summary>
    /// Implicit conversion from List<Point2d> copies the input list of points
    /// </summary>
    public static implicit operator Polyline2d(List<Point2d> points)
    {
        List<Point2d> copy = [];
        foreach (var point in points)
        {
            copy.Add(new Point2d(point.X, point.Y));
        }
        return new Polyline2d(copy);
    }

    /// <summary>
    /// Copies input points to create a polyline2d.
    /// </summary>
    public static Polyline2d CopyPoints(List<Point2d> points)
    {
        List<Point2d> copy = [];
        foreach (var point in points)
        {
            copy.Add(new Point2d(point.X, point.Y));
        }
        return new Polyline2d(copy);
    }

    public double Length()
    {
        double totalLength = 0.0;
        for (int i = 0; i < Points.Count - 1; i++)
        {
            Vector2d vector = Points[i + 1] - Points[i];
            totalLength += vector.Length;
        }
        return totalLength;
    }
}