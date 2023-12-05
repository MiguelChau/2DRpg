using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    [Header("Flash FX")]
    [SerializeField] private float fxDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;

        yield return new WaitForSeconds(fxDuration);

        spriteRenderer.material = originalMat;

    }

    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
