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


    [Header("Ailment Color")]
    [SerializeField] private Color[] frozenColor;
    [SerializeField] private Color[] burnedColor;
    [SerializeField] private Color[] shockColor;
    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMat = spriteRenderer.material;
    }

    //Torna a entidade transparente ou não
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            spriteRenderer.color = Color.clear;
        else
            spriteRenderer.color = Color.white;
    }

    IEnumerator FlashFX()
    {
        //Armazena o material atual e aplica o material no Hit
        spriteRenderer.material = hitMat;
        Color _currentColor = spriteRenderer.color;
        spriteRenderer.color = Color.white;

        yield return new WaitForSeconds(fxDuration);

        //Restaura a cor original
        spriteRenderer.color = _currentColor;
        spriteRenderer.material = originalMat;

    }

    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
            spriteRenderer.color = Color.white;
        else
            spriteRenderer.color = Color.red;
    }

    //Cancela a mudança de cor programada
    private void CancelColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }

    public void InvokeIgniteFx(float _seconds)
    {
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void InvokeFrozenFx(float _seconds)
    {
        InvokeRepeating("FrozenColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void InvokeShockFx(float _seconds)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (spriteRenderer.color != burnedColor[0])
            spriteRenderer.color = burnedColor[0];
        else
            spriteRenderer.color = burnedColor[1];
    }

    private void FrozenColorFx()
    {
        if (spriteRenderer.color != frozenColor[0])
            spriteRenderer.color = frozenColor[0];
        else
            spriteRenderer.color = frozenColor[1];
    }

    private void ShockColorFx()
    {
        if (spriteRenderer.color != shockColor[0])
            spriteRenderer.color = shockColor[0];
        else
            spriteRenderer.color = shockColor[1];
    }
}
