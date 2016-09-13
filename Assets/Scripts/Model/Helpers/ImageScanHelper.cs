using UnityEngine;

public enum EPointType
{
	None = 0,
	Wall = 1,
	Start = 2,
	End = 3,
}

public class ImageScanHelper
{
	public static Color WallColor = Color.black;
	public static Color NoneColor = Color.white;
	public static Color StartColor = Color.blue;
	public static Color EndColor = Color.red;

	public static byte[,] Scan(Sprite input)
	{
		return Scan(input.texture);
	}

	public static byte[,] Scan(Texture texture)
	{
		var input = texture as Texture2D;
		if (input != null)
		{
			return Scan(input);
		}
		return new byte[0, 0];
	}

	public static byte[,] Scan(Texture2D input)
	{		
		var width = input.width;
		var height = input.height;
		var field = new byte[width, height];
		Color pixel;	
		for (var w = 0; w < width; w++)
		{
			for (var h = 0; h < height; h++)
			{
				pixel = input.GetPixel(w, h);
				field[w, h] = (byte)(pixel.grayscale * 255);
			}
		}
		return field;
	}

	#region Obsolete
	// 	public static Maze Scan(Texture2D texture, float minTollerance, float maxTollerance, int step = 1, bool useGrayScale = false)
// 	{
// 		var result = new Maze();
// 		var width = texture.width;
// 		var height = texture.height;
// 		result.field = new byte[width, height];
// 		var servicePixel = false;
// 		Color pixel;
// 		byte pixelType;
// 		for (var w = 0; w < width; w+=step)
// 		{
// 			for (var h = 0; h < height; h += step)
// 			{
// 				pixel = texture.GetPixel(w, h);
// 				pixelType = (byte)EPointType.None;
// 				servicePixel = false;
// 
// 				if (TestColor(pixel, Color.blue))
// 				{
// 					if (result.start == Point.zero)
// 					{
// 						//UnityEngine.Debug.Log("Catch start " + w + " " + h);
// 						result.start = new Point(w, h);
// 						pixelType = (byte)EPointType.Start;
// 					}
// 					else
// 					{
// 						pixelType = (byte)EPointType.None;
// 					}
// 					servicePixel = true;
// 				}
// 
// 				if (TestColor(pixel, Color.red))
// 				{
// 					if (result.end == Point.zero)
// 					{
// 						//UnityEngine.Debug.Log("Catch end " + w + " " + h);
// 						result.end = new Point(w, h);
// 						pixelType = (byte)EPointType.End;						
// 					}
// 					else
// 					{
// 						pixelType = (byte)EPointType.None;
// 					}
// 					servicePixel = true;
// 				}
// 
// 				if (useGrayScale)
// 				{
// 					var gray = pixel.grayscale;
// 					if (gray < minTollerance && !servicePixel)
// 					{
// 						pixelType = (byte)EPointType.Wall;
// 					}
// 				}
// 				else
// 				{
// 					if (TestColor(pixel, Color.black))
// 					{
// 						pixelType = (byte)EPointType.Wall;
// 					}
// 				}
// 		
// 				for (var wr = w-step/2; wr < (w + step); wr++)
// 				{
// 					if (wr < width && wr > 0)
// 						for (var hr = h - step / 2; hr < h + step; hr++)
// 					{
// 						if (hr < height && hr >0)
// 						result.field[wr, hr] = pixelType;
// 					}
// 				}
// 			}
// 		}
// 		return result;
// 	}
//
// 	private static bool TestColor(Color pixel, Color expected, float deviation = 0.1f)
// 	{
// 		var dr = Mathf.Abs(expected.r - pixel.r);
// 		var dg = Mathf.Abs(expected.g - pixel.g);
// 		var db = Mathf.Abs(expected.b - pixel.b);
// 
// 		return dr < deviation && dg < deviation && db < deviation;
// 	}
	#endregion

	public static Texture2D Refine(Maze maze)
	{
		if (maze != null)
		{
			var width = maze.Width;
			var height = maze.Height;
			var result = new Texture2D(width, height);
			for (var w = 0; w < width; ++w)
			{
				for (var h = 0; h < height; ++h)
				{
					var g = maze[w, h].weight;
					var colorVal = g / 255f;
					var color = new Color(colorVal, colorVal, colorVal, 1f);
					result.SetPixel(w, h, color);
				}
			}
			result.filterMode = FilterMode.Point;
			result.anisoLevel = 0;
			result.name = "Refined maze";
			result.Apply();
			return result;
		}
		return null;
	}
}