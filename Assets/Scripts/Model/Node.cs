using System;
public class Node : IComparable<Node>
{
	public readonly Point position;
	public Node Parent;
	public float cost;
	public int weight;
	public float step;
	public float distance;

	public Node(int w, int h)
	{
		position = new Point(w, h);
	}
	public Node(Point nodePosition)
	{
		position = new Point(nodePosition);
	}

	public static bool operator == (Node lhs, Node rhs)
	{
		if (ReferenceEquals(lhs, rhs))
		{
			return true;
		}

		if (((object)lhs == null) || ((object)rhs == null))
		{
			return false;
		}
		return lhs.position == rhs.position;
	}

	public static bool operator != (Node lhs, Node rhs)
	{
		return !(lhs== rhs);
	}

	public override int GetHashCode()
	{
		return position.GetHashCode();
	}

	public int CompareTo(Node other)
	{
		if (cost < other.cost) return -1;
		if (cost > other.cost) return 1;
		return 0;
	}
}
