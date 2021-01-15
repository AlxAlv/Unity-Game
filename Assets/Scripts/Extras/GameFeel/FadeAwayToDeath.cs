using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FadeAwayToDeath : Singleton<FadeAwayToDeath>
{
	[SerializeField] private float _deathSpeed = 2.5f;

	public void InitializeFadeAway(SpriteRenderer spriteRenderer)
	{
		//Spawn object
		GameObject spriteObject = Instantiate((Resources.Load("Prefabs/Enemies/FadeAwayObject") as GameObject), spriteRenderer.transform.position, Quaternion.identity);

		//Add Components
		SpriteRenderer renderer = spriteObject.GetComponent<SpriteRenderer>();

		// Get Path And Copy Values
		renderer.color = spriteRenderer.color;

		if (spriteRenderer.gameObject.transform.parent)
			renderer.transform.localScale = spriteRenderer.gameObject.transform.parent.transform.localScale;
		else
			renderer.transform.localScale = spriteRenderer.gameObject.transform.localScale;

		// Look Through The SpriteSheet For The Specific Sprite We Need
		int indexToUse = 0;
		Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Characters/");

		for (int i = 0; i < sprites.Length; ++i)
			if (sprites[i].name == spriteRenderer.sprite.name)
			{
				indexToUse = i;
				i = sprites.Length;
			}

		renderer.sprite = sprites[indexToUse];

		StartCoroutine(FadeAway(renderer));
	}

	public void InitializeFadeAway(Image image, float imageScale = 1.0005f)
	{
		//Spawn object
		GameObject imageObject = Instantiate((Resources.Load("Prefabs/UI/FadeAwayImage") as GameObject), image.transform.position, Quaternion.identity);

		//Add Components
		Image newImage = imageObject.GetComponent<Image>();

		// Get Path And Copy Values
		newImage.color = image.color;

		newImage.transform.parent = image.gameObject.transform;
		newImage.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

		// Assign The Correct Image
		newImage.sprite = image.sprite;

		StartCoroutine(FadeAway(newImage, imageScale));
	}

	public void InitializeFadeAway(TextMeshProUGUI text, float imageScale = 1.0005f)
	{
		//Spawn object
 		GameObject textObject = Instantiate((Resources.Load("Prefabs/UI/FadeAwayText") as GameObject), text.transform.position, Quaternion.identity);

		//Add Components
		TextMeshProUGUI newText = textObject.GetComponent<TextMeshProUGUI>();

		// Get Path And Copy Values
		newText.color = text.color;
		newText.font = text.font;
		newText.fontMaterial = text.fontMaterial;
		newText.fontSize = text.fontSize;
		newText.alignment = text.alignment;
		newText.enableWordWrapping = text.enableWordWrapping;

		RectTransform rt = newText.GetComponent<RectTransform>();
		rt.sizeDelta = text.GetComponent<RectTransform>().sizeDelta;

		newText.transform.parent = text.gameObject.transform;
		newText.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

		// Assign The Correct Text
		newText.text = text.text;

		StartCoroutine(FadeAway(newText, imageScale));
	}

	private IEnumerator FadeAway(SpriteRenderer spriteRenderer)
	{
		Color color = spriteRenderer.color;

		while (color.a > 0)
		{
			color.a -= _deathSpeed * Time.deltaTime;
			spriteRenderer.transform.localScale *= 1.01f;

			spriteRenderer.color = color;
			yield return null;
		}

		Destroy(spriteRenderer.gameObject);
	}

	private IEnumerator FadeAway(Image image, float imageScale = 1.0005f)
	{
		Color color = image.color;

		while (color.a > 0)
		{
			color.a -= _deathSpeed * Time.deltaTime;
			image.transform.localScale *= imageScale;

			image.color = color;
			yield return null;
		}

		Destroy(image.gameObject);
	}

	private IEnumerator FadeAway(TextMeshProUGUI text, float _imageScale = 1.05f)
	{
		Color color = text.color;

		while (color.a > 0)
		{
			color.a -= _deathSpeed * Time.deltaTime;
			text.transform.localScale *= _imageScale;

			text.color = color;
			yield return null;
		}

		Destroy(text.gameObject);
	}
}
