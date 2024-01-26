using System.Collections;
using UnityEngine;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer spriteRenderer;
    
    [Header("PopUp Text")]
    [SerializeField] private GameObject popUpPrefab;   

    [Header("Flash FX")]
    [SerializeField] private float fxDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;


    [Header("Ailment Color")]
    [SerializeField] private Color[] frozenColor;
    [SerializeField] private Color[] burnedColor;
    [SerializeField] private Color[] shockColor;

    [Header("Ailment FX")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFxPrefab;
    [SerializeField] private GameObject critHitFxPrefab;

    private GameObject myHealthBar;

    protected virtual void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = PlayerManager.instance.player;

        originalMat = spriteRenderer.material;

        myHealthBar = GetComponentInChildren<HealthBar_UI>().gameObject;
    }
    public void CreatePopUp(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 2);

        Vector3 positionOffSet = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(popUpPrefab, transform.position + positionOffSet, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

    //Torna a entidade transparente ou não
    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
        {
            myHealthBar.SetActive(false);
            spriteRenderer.color = Color.clear;
        }
        else
        {
            myHealthBar.SetActive(true);
            spriteRenderer.color = Color.white;
        }
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

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void InvokeIgniteFx(float _seconds)
    {
        igniteFX.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }
    public void InvokeFrozenFx(float _seconds)
    {
        chillFX.Play();

        InvokeRepeating("FrozenColorFx", 0, .3f);
        Invoke("CancelColorChange", _seconds);
    }

    public void InvokeShockFx(float _seconds)
    {
        shockFX.Play();

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

    public void CreateHitFX(Transform _target, bool _critical)
    {

        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFxRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFxPrefab;

        if (_critical)
        {
            hitPrefab = critHitFxPrefab;

            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRotation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        newHitFx.transform.Rotate(hitFxRotation);

        Destroy(newHitFx, .4f);
    }

}
