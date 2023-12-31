using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIRenderTexture : MonoBehaviour
{
	[Header("Settings")]
	public Vector2 size = new Vector2(1920, 1080);
	public bool useScreenSize = true;

	[Header("Components")]
	public Camera UICamera;
	public RawImage UIImage;
	RenderTexture newTexture;
	private float width;
	private float height;

	private void Update()
	{
		if (UICamera == null || UIImage == null)
			return;

		if (newTexture == null)
			newTexture = new RenderTexture(useScreenSize ? Screen.width : (int)size.x, useScreenSize ? Screen.height : (int)size.y, 24) { format = RenderTextureFormat.ARGB32 };

		width = useScreenSize ? Screen.width : (int)size.x;
		height = useScreenSize ? Screen.height : (int)size.y;

		if (newTexture.width != width || newTexture.height != height)
			newTexture = new RenderTexture(useScreenSize ? Screen.width : (int)size.x, useScreenSize ? Screen.height : (int)size.y, 24) { format = RenderTextureFormat.ARGB32 };


		UICamera.targetTexture = newTexture;
		UIImage.texture = newTexture;

		if (UICamera.targetTexture != null)
			UICamera.targetTexture.Release();
	}
}