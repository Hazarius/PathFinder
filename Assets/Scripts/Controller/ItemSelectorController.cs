using UnityEngine;
using UnityEngine.UI;

public class ItemSelectorController : MonoBehaviour
{
	public MainController mainController;
	private Image _image;

	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnSelectButtonClic);
		_image = GetComponent<Image>();
	}

	private void OnSelectButtonClic()
	{
		mainController.SetInputTexture(_image.sprite);
	}
}