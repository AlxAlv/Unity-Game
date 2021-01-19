using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemNumberPopups : MonoBehaviour
{
    public static ItemNumberPopups Create(Vector3 position, int damageAmount, ItemRewardType rewardType)
    {
        GameObject damageNumbersPrefab = Instantiate((Resources.Load("Prefabs/NumberPopups/ItemNumbers") as GameObject), position, Quaternion.identity);
        ItemNumberPopups damageNumbers = damageNumbersPrefab.GetComponent<ItemNumberPopups>();
        damageNumbers.Setup(damageAmount, rewardType);

        return damageNumbers;
    }

    private TextMeshPro _text;
    private SpriteRenderer _sprite;
    private Color _textColor;
    private float _disappearTimer;
    private Vector3 moveVector;
    private static int _sortingOrder;

    // Game Feel Numbers
    // Scale
    private float _enlargeAmount = 2.0f;
    private float _reduceAmount = (1.5f);

    // Direction
    private float _directionSpeed = 8.0f;

    // Time
    private float _precentSpentEnlarging = 0.7f;
    private float _dissapearSpeed = 20.0f;
    private float _timeAlive = (1.0f);

    private void Awake()
    {
        _text = transform.GetComponent<TextMeshPro>();
        _sprite = GetComponentInChildren<SpriteRenderer>();
        _textColor = _text.color;

        _disappearTimer = _timeAlive;
    }

    // Start is called before the first frame update
    private void Setup(int damageAmount, ItemRewardType reward)
    {
        _text.SetText("x" + damageAmount.ToString());
        moveVector = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(3.0f, 6.0f)) * 3f;
        _sortingOrder++;
        _text.sortingOrder = _sortingOrder;

        switch (reward)
        {
            case ItemRewardType.JarDust:
				_sprite.sprite = Resources.Load<Sprite>("Sprites/Items/JarDust");
                break;

            case ItemRewardType.MagicPowder:
	            _sprite.sprite = Resources.Load<Sprite>("Sprites/Items/MagicPowder");
                break;

            case ItemRewardType.Sawdust:
	            _sprite.sprite = Resources.Load<Sprite>("Sprites/Items/Sawdust");
                break;
        }
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * _directionSpeed * Time.deltaTime;

        _disappearTimer -= Time.deltaTime;

        if (_disappearTimer > (_timeAlive * _precentSpentEnlarging))
        {
            // First half of the popup
            transform.localScale += Vector3.one * _enlargeAmount * Time.deltaTime;
        }
        else
        {
            // Second half
            transform.localScale -= Vector3.one * _reduceAmount * Time.deltaTime;
        }

        if (_disappearTimer < 0)
        {
            // Start Disppearing
            _textColor.a -= _dissapearSpeed * Time.deltaTime;

            _text.color = _textColor;

            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
