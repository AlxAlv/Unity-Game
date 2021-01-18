using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TintHelper : MonoBehaviour
{
    [SerializeField] public SpriteRenderer SpriteRenderer;

    public SpriteRenderer ArmRenderer { get; set; }
    public SpriteRenderer OffArmRenderer { get; set; }

    private Material _material;

    private Color _materialTintcolor;
    private Color _invincibleColor = new Color(1, 1, 0, .5f);
    private float _tintFadeSpeed;

    void Awake()
    {
        _materialTintcolor = new Color(1, 0, 0, 0);

        _material = SpriteRenderer.material;

        _tintFadeSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_materialTintcolor.a > 0)
        {
            _materialTintcolor.a = Mathf.Clamp01(_materialTintcolor.a - _tintFadeSpeed * Time.deltaTime);
            _material.SetColor("_Tint", _materialTintcolor);
        }

        EntityStunGuage stunGuage = GetComponent<EntityStunGuage>();

        if (stunGuage && stunGuage.Invincible)
            _materialTintcolor = _invincibleColor;
    }

    public void SetTintColor(Color color)
    {
        _materialTintcolor = color;
        _material.SetColor("_Tint", _materialTintcolor);
    }

    public void SetTintFadeSpeed(float tintFadeSpeed)
    {
        _tintFadeSpeed = tintFadeSpeed;
    }
}
