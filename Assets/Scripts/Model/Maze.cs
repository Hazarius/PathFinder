using System;
using UnityEngine;

public class Maze
{	
	private Node[,] _field;
	public int Width { get; private set; }
	public int Height { get; private set; }

	public Node[,] Field
	{
		get { return _field; }
	}

	public Maze(int width, int height)
	{
		Width = width;
		Height = height;
		_field = new Node[width, height];
	}

	public Maze(Node[,] field)
	{
		Width = field.GetLength(0);
		Height = field.GetLength(1);
		_field = field;
	}

	public Maze(Texture texture)
	{
		var raw = ImageScanHelper.Scan(texture);
		Load(raw);
	}

	public Maze(Sprite sprite)
	{
		var raw = ImageScanHelper.Scan(sprite);
		Load(raw);
	}

	private void Load(byte[,] raw)
	{
		Width = raw.GetLength(0);
		Height = raw.GetLength(1);

		_field = new Node[Width, Height];
		for (var w = 0; w < Width; ++w)
		{
			for (var h = 0; h < Height; ++h)
			{
				_field[w, h] = new Node(new Point(w, h));
				_field[w, h].weight = raw[w, h];
			}
		}
	}

	public Node this[int w, int h]
	{
		get
		{
			if (w >= 0 && h >= 0 && w < Width && h < Height)
			{
				return _field[w, h];
			}
			throw new ArgumentOutOfRangeException();
		}
	}

	public void Reset()
	{
		for (var w = 0; w < Width; ++w)
		{
			for (var h = 0; h < Height; ++h)
			{
				_field[w, h].distance = 0;
				_field[w, h].cost = 0;
				_field[w, h].step = 0;
				_field[w, h].Parent = null;
			}
		}	
	}
}