using System;
using UnityEngine;

public enum SwordType
{
    REGULAR,
    BOUNCE,
    PIERCE,
    SPIN
}
public class Sword_Skill : Skill
{
    public SwordType swordType = SwordType.REGULAR;

    [Header("Bounce Info")]
    [SerializeField] private int bounceAmount;
    [SerializeField] private float bounceGravity;
    [SerializeField] private float bounceSpeed;

    [Header("Piece Info")]
    [SerializeField] private int pierceAmount;
    [SerializeField] private float pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private float maxTravelDis = 7;
    [SerializeField] private float spinDur = 2;
    [SerializeField] private float spinGravity = 1;
    [SerializeField] private float hitCooldown = .35f;


    [Header("Skill Info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimerDur;
    [SerializeField] private float returnSpeed;


    private Vector2 finalDir;

    [Header("Aim Dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();
        
    }

    private float SetupGravity()
    {
        float swordGravity = 0;

        if (swordType == SwordType.BOUNCE)
            swordGravity = bounceGravity;
        else if(swordType == SwordType.PIERCE)
            swordGravity = pierceGravity;
        else if(swordType == SwordType.SPIN)
            swordGravity = spinGravity;

        return swordGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.BOUNCE)
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed);
        else if (swordType == SwordType.PIERCE)
            newSwordScript.SetupPierce(pierceAmount);
        else if (swordType == SwordType.SPIN)
            newSwordScript.SetupSpin(true, maxTravelDis, spinDur, hitCooldown);


        float swordGravity = SetupGravity();
        newSwordScript.SetupSword(finalDir, swordGravity, player, freezeTimerDur, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Aim
    public Vector2 AimDirection()
    {
        Vector2 playerPos = player.transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - playerPos;

        return direction;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t) //direçao usando e multiplicando pela gravidade 
    {
        Vector2 pos = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * launchForce.x,
            AimDirection().normalized.y * launchForce.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return pos;
    }
    #endregion
}
