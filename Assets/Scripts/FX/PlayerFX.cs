using Cinemachine;
using System.Collections;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After Image FX")]
    [SerializeField] private float afterImageCooldown;
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorAlpha;
    private float afterImageCooldownTimer;

    [Header("Screen Shake FX")]
    [SerializeField] private float shakeMultiplier;
    public Vector3 shakeSwordPower;
    public Vector3 shakeHighDamage;
    private CinemachineImpulseSource screenShake;

    [Space]
    [SerializeField] private ParticleSystem dustFX;

    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }
    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultiplier;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);

            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorAlpha, spriteRenderer.sprite);
        }
    }

    public void PlayDustFX()
    {
        if (dustFX != null)
            dustFX.Play();
    }
}
