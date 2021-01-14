using System.Collections;
using UnityEditor;
using UnityEngine;

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
}
