using UnityEngine;
using UnityEngine.UI;

public class RawImageSelectController : MonoBehaviour
{
	public MainController _mainController;
	private RawImage _image;

	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnSelectButtonClic);
		_image = GetComponent<RawImage>();
	}

	private void OnSelectButtonClic()
	{
		_mainController.SetInputTexture(_image.texture);
	}
}