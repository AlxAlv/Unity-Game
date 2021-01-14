using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DamageNumbers : MonoBehaviour
{
    public static DamageNumbers Create(Vector3 position, int damageAmount)
    {
        GameObject damageNumbersPrefab = Instantiate((Resources.Load("Prefabs/NumberPopups/DamageNumbers") as GameObject), position, Quaternion.identity);
        DamageNumbers damageNumbers = damageNumbersPrefab.GetComponent<DamageNumbers>();
        damageNumbers.Setup(damageAmount);

        return damageNumbers;
    }

    private TextMeshPro _text;
    private Color _textColor;
    private float _disappearTimer;
    private Vector3 moveVector;
    private static int _sortingOrder;

    // Game Feel Numbers
    // Scale
    private float _enlargeAmount = 16.0f;
    private float _reduceAmount = 8.0f;

    // Direction
    private float _directionSpeed = 8.0f;

    // Time
    private float _precentSpentEnlarging = 0.7f;
    private float _dissapearSpeed = 20.0f;
    private float _timeAlive = 0.5f;

    private void Awake()
    {
        _text = transform.GetComponent<TextMeshPro>();
        _textColor = _text.color;

        _disappearTimer = _timeAlive;
    }

    // Start is called before the first frame update
    private void Setup(int damageAmount)
    {
        _text.SetText("-" + damageAmount.ToString());
        moveVector = new Vector3(Random.Range(-2.5f, 2.5f), Random.Range(3.0f, 6.0f)) * 3f;
        _sortingOrder++;
        _text.sortingOrder = _sortingOrder;
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

            if(_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
