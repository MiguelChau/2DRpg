using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    private RectTransform rectTransform;
    private Slider slider;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats._currentHealth;
    }

    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChange += UpdateHealthUI;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
            myStats.onHealthChange -= UpdateHealthUI;
    }
    private void FlipUI() => rectTransform.Rotate(0, 180, 0);


}
