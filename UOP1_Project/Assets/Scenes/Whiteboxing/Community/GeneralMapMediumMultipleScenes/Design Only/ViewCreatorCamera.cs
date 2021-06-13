using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class ViewCreatorCamera : MonoBehaviour
{
	public string targetTexturePath;//path in Assets folder where image is to be stored

	public void captureView()
	{
		Camera camera = gameObject.GetComponent<Camera>();
		RenderTexture renderTexture = camera.targetTexture;
		Texture2D view2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
		camera.Render();
		RenderTexture.active = renderTexture;
		view2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		byte[] bytes = view2D.EncodeToPNG();
		System.IO.File.WriteAllBytes(Application.dataPath + targetTexturePath + ".png", bytes);
	}
}
