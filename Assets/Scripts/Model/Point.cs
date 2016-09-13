using UnityEngine;

public struct Point
{
	public readonly int x;
	public readonly int y;
	
	public static Point zero = new Point(0, 0);
	public static Point one = new Point(1, 1);
	public static Point up = new Point(1, 0);
	public static Point down = new Point(-1, 0);
	public static Point left = new Point(0, -1);
	public static Point right = new Point(0, 1);

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public Point(Vector2 vecor)
	{
		x = (int)vecor.x;
		y = (int)vecor.y;
	}

	public Point(Point point)
	{
		x = point.x;
		y = point.y;
	}

	public static Point operator +(Point lhs, Point rhs)
	{
		return new Point(lhs.x + rhs.x, lhs.y + rhs.y);
	}

	public static Point operator -(Point lhs, Point rhs)
	{
		return new Point(lhs.x - rhs.x, lhs.y - rhs.y);
	}

	public static bool operator ==(Point lhs, Point rhs)
	{
		return lhs.x == rhs.x && lhs.y == rhs.y;
	}

	public static bool operator !=(Point lhs, Point rhs)
	{
		return lhs.x != rhs.x || lhs.y != rhs.y;
	}

	public static Point operator *(Point lhs, int scale)
	{
		return new Point(lhs.x * scale, lhs.y * scale);
	}

	public override string ToString()
	{
		return "["+x + ", " + y+"]";
	}

	public override int GetHashCode()
	{
		return 10000 * x + y;
	}
}