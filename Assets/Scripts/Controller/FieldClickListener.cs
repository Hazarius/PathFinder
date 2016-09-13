using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldClickListener : MonoBehaviour, IPointerClickHandler
{
	public MainController mainController;
	public Canvas mainCanvas;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (mainController != null)
		{					
			var rect = GetComponent<RectTransform>();
			
			var image = GetComponent<RawImage>();
			if (image != null && image.texture != null)
			{
				var pos = eventData.position;
				pos.x /= mainCanvas.scaleFactor;
				pos.y /= mainCanvas.scaleFactor;

				pos.x -= rect.localPosition.x;
				pos.y -= rect.localPosition.y;

				var w = pos.x / rect.rect.width * image.texture.width;
				var h = pos.y / rect.rect.height * image.texture.height;
				mainController.OnSetPoint((int)w, (int)h);
			}			
		}		
	}
}