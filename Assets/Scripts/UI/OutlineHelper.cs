using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineHelper : MonoBehaviour
{
    [SerializeField] public SpriteRenderer SpriteRenderer;

    public void SetOutlineAmount(float outlineAmount)
    {
        SpriteRenderer.material.SetFloat("_Thickness", outlineAmount);
    }
}
