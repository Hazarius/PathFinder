using Random = UnityEngine.Random;

public static class MazeGenerator
{	
	public class Properties
	{
		public int width;
		public int height;
		public int wallThikness = 1;
		public int passThikness = 1;
		public float wallThrowChance;
	}

	public static Maze GenerateRandomMaze(Properties props)
	{
		var width = props.width;
		var height = props.height;
		if (width < 3 || height < 3)
		{
			UnityEngine.Debug.LogError("Not enough size for maze");
			return null;
		}
		if ((width & 2) == 0)
		{
			width -= 1;
		}
		if ((height & 2) > 0)
		{
			height -= 1;
		}
		var field = new Node[width, height];
		var wallOffset = 1;
		for (var w = 0; w < width; ++w)
		{
			for (var h = 0; h < height; ++h)
			{
				var node = new Node(new Point(w,h));

				if ((w%2 != 0 && h%2 != 0) &&
				    (w < width - 1 && h < height - 1))
				{
					node.weight = 255;
				}
				else
				{
					node.weight = 0; 
				}

				field[w, h] = node;
			}
		}
		var startNode = field[Random.Range(1, width - 1), Random.Range(1, height - 1)];		
		if ((startNode.position.x % 2) == 0)
		{
			var newPoint = startNode.position + Point.down;
			startNode = field[newPoint.x, newPoint.y];
		}

		if ((startNode.position.y % 2) == 0)
		{
			var newPoint = startNode.position + Point.left;
			startNode = field[newPoint.x, newPoint.y];
		}		
 		Heap<Node> heap = new MinHeap<Node>();
 		heap.Add(startNode);
		var neibhors = new Point[4];
		int counter = 0;
		int maxCount = width*height*2;
		for (int i = 0; i < neibhors.Length; i++)
		{
			neibhors[i]	= Directions.directions[i * 2];			
		}
		var currentNode = startNode;
		do
		{
			if (currentNode == null)
			{
				currentNode = heap.ExtractDominating();
			}

			var isGenerated = false;
			for (int i = 0; i < neibhors.Length; i++)
			{
				var randomIndex = Random.Range(0, 4);
				var direction = neibhors[randomIndex];
				var pos = currentNode.position + direction * 2;
				if (pos.x >= width - 1 || pos.x <= 0 || pos.y >= height - 1 || pos.y <= 0)
				{
					continue;
				}
				var node = field[pos.x, pos.y];

				if (node.Parent == null)
				{
					if (Random.value > props.wallThrowChance)
					{
						node.Parent = currentNode;
					}				
					heap.Add(node);
					var wallPos = currentNode.position + direction;
					field[wallPos.x, wallPos.y].weight = 255;
					currentNode = node;
					isGenerated = true;
					break;
				}						
			}
			if (!isGenerated)
			{
				currentNode = null;
			}
			counter++;
		}
		while (heap.Count > 0 || currentNode!=null && counter < maxCount);
		

		var maze = new Maze(field);
		return maze;
	}
}