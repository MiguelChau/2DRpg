using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float colorLooseRate;

    public void SetupAfterImage(float _loosingSpeed, Sprite _spriteImage)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = _spriteImage;

        colorLooseRate = _loosingSpeed;
    }

    private void Update()
    {
        float alpha = spriteRenderer.color.a - colorLooseRate * Time.deltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

        if (spriteRenderer.color.a <= 0)
            Destroy(gameObject);
    }
}
