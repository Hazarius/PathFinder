public class Directions
{
	public static Point[] directions = new Point[]
	{
		new Point( 1, 0),	// N
		new Point( 1, 1),	// NE
 		new Point( 0, 1),	// E
		new Point(-1, 1),	// SE
		new Point(-1, 0),	// S
		new Point(-1,-1),	// SW
		new Point( 0,-1),	// W
		new Point( 1,-1)		// NW
	};

	public static Point GetDirection(EDirection direction)
	{
		return directions[(int) direction];
	}
}