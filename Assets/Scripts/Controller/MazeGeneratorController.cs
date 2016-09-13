using UnityEngine;
using UnityEngine.UI;

public class MazeGeneratorController : MonoBehaviour
{
	public InputField widthField;
	public InputField heightField;
	public Slider wallCoefficientSlider;
	// to view

	public RawImage _resultImage;

	#region UI actions implemetation

	public void OnGenerateButtonClick()
	{
		int w = 0;
		int h = 0;
		if (widthField.text.Length > 0)
		{
			w = int.Parse(widthField.text);
		}

		if (heightField.text.Length > 0)
		{
			h = int.Parse(heightField.text);
		}
		if (w > 0 && h > 0)
		{
			var props = new MazeGenerator.Properties();
			props.width = w;
			props.height = h;
			props.wallThrowChance = Mathf.Clamp01(wallCoefficientSlider.value/100f);
			var maze = MazeGenerator.GenerateRandomMaze(props);
			_resultImage.texture = ImageScanHelper.Refine(maze);
		}
	}

	#endregion

}