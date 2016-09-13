using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathFinderView : MonoBehaviour
{
	private static readonly Color Clear = new Color(0f, 0f, 0f, 0f);

	public RawImage output;
	public RawImage path;
	public RawImage pointsImage;

	public Text infoText;
	public Text euristicValue;

	private Texture2D _pathTexture;
	private Texture2D _pointTexture;

	public void ResetTextures()
	{
		_pathTexture = new Texture2D(output.texture.width, output.texture.height);
		_pointTexture = new Texture2D(output.texture.width, output.texture.height);

		var outputWidth = output.texture.width;
		var outputHeight = output.texture.height;

		for (var w = 0; w < outputWidth; ++w)
		{
			for (var h = 0; h < outputHeight; ++h)
			{
				_pathTexture.SetPixel(w, h, Clear);
				_pointTexture.SetPixel(w, h, Clear);
			}
		}
		_pathTexture.filterMode = FilterMode.Point;
		_pointTexture.filterMode = FilterMode.Point;
		_pointTexture.Apply();
		_pathTexture.Apply();
		path.texture = _pathTexture;
		pointsImage.texture = _pointTexture;
	}

	public void DrawCircle(int w, int h, int radius, Color color)
	{
		for (int x = -radius / 2; x < radius / 2; x++)
		{
			for (int y = -radius / 2; y < radius / 2; y++)
				_pointTexture.SetPixel(x + w, y + h, color * new Color(1f, 1f, 1f, 0.5f));
		}
		_pointTexture.Apply();
	}

	public void DrawProgress(List<Node> closedNodes)
	{
		if (closedNodes != null)
		{
			for (var i = 0; i < closedNodes.Count; ++i)
			{
				var point = closedNodes[i];				
				_pathTexture.SetPixel(point.position.x, point.position.y, Color.yellow);
			}
			closedNodes.Clear();

			_pathTexture.Apply();
		}
	}

	public void DrawPath(Node[] extimatedPath)
	{
		if (extimatedPath != null)
		{
			for (var i = 0; i < extimatedPath.Length; ++i)
			{
				_pathTexture.SetPixel(extimatedPath[i].position.x, extimatedPath[i].position.y, Color.green);
			}
			_pathTexture.Apply();
		}
	}

	public void SetMaze(Maze maze)
	{
		output.texture = ImageScanHelper.Refine(maze);
	}

	public void SetInfo(string value, EMessageType messageType)
	{
		switch (messageType)
		{
			case EMessageType.Normal:
				infoText.color = Color.black;
				break;
			case EMessageType.Warning:
				infoText.color = Color.yellow;
				break;
			case EMessageType.Critical:
				infoText.color = Color.red;
				break;
		}
		infoText.text = value;
	}
}