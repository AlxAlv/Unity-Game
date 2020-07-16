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
    private const float DISAPPEAR_TIMER_MAX = 0.5f;
    private Vector3 moveVector;
    private static int _sortingOrder;

    private void Awake()
    {
        _text = transform.GetComponent<TextMeshPro>();
        _textColor = _text.color;

        _disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    // Start is called before the first frame update
    private void Setup(int damageAmount)
    {
        _text.SetText(damageAmount.ToString());
        moveVector = new Vector3(1, 1) * 3f;
        _sortingOrder++;
        _text.sortingOrder = _sortingOrder;
    }

    private void Update()
    {
        float moveYSpeed = 2f;

        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        _disappearTimer -= Time.deltaTime;

        if (_disappearTimer > DISAPPEAR_TIMER_MAX * .7f)
        {
            // First half of the popup
            float increaseScaleAmount = 1.5f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        if (_disappearTimer < 0)
        {
            // Start Disppearing
            float disappearSpeed = 3f;
            _textColor.a -= disappearSpeed * Time.deltaTime;

            _text.color = _textColor;

            if(_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
