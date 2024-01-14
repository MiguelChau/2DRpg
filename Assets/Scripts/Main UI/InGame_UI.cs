using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGame_UI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordThrowImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image healthPotImage;

    [SerializeField] private TextMeshProUGUI currentExp;
    private SkillManager skills;

    [Header("Experience Info")]
    [SerializeField] private TextMeshProUGUI currentExperience;
    [SerializeField] private float expAmount;
    [SerializeField] private float increaseRate = 100;

    private void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChange += UpdateHealthUI;

        skills = SkillManager.instance;
    }
    private void Update()
    {
        UpdateExpUI();

        if (Input.GetKeyDown(KeyCode.Z) && skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Alpha2) && skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
            SetCooldownOf(swordThrowImage);

        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackholeUnlocked)
            SetCooldownOf(blackHoleImage);

        if (Input.GetKeyDown(KeyCode.B) && Inventory.Instance.GetEquipment(EquipementType.Flask) != null)
            SetCooldownOf(healthPotImage);


        CheckCooldownOf(dashImage, skills.dash.cooldown);
        CheckCooldownOf(parryImage, skills.parry.cooldown);
        CheckCooldownOf(crystalImage, skills.crystal.cooldown);
        CheckCooldownOf(swordThrowImage, skills.sword.cooldown);
        CheckCooldownOf(blackHoleImage, skills.blackhole.cooldown);
        CheckCooldownOf(healthPotImage, Inventory.Instance.flaskCooldown);
    }

    private void UpdateExpUI()
    {
        if (expAmount < PlayerManager.instance.CurrentExp())
            expAmount += Time.deltaTime * increaseRate;
        else
            expAmount = PlayerManager.instance.CurrentExp();

        currentExp.text = ((int)expAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats._currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
