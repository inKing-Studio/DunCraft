using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats Base")]
    public float baseHP = 100f;
    public float baseMP = 50f;
    public float basePhysicalAttack = 10f;
    public float basePhysicalDefense = 5f;
    public float baseMagicalAttack = 10f;
    public float baseMagicalDefense = 5f;
    public float baseSpeed = 5f;

    [Header("Afinidades Base")]
    public float baseFireAffinity = 0f;
    public float baseWaterAffinity = 0f;
    public float baseEarthAffinity = 0f;
    public float baseWindAffinity = 0f;
    public float baseLightAffinity = 0f;
    public float baseDarkAffinity = 0f;

    [Header("Stats Actuales")]
    public float currentHP;
    public float currentMP;
    public float currentPhysicalAttack;
    public float currentPhysicalDefense;
    public float currentMagicalAttack;
    public float currentMagicalDefense;
    public float currentSpeed;

    [Header("Afinidades Actuales")]
    public float currentFireAffinity;
    public float currentWaterAffinity;
    public float currentEarthAffinity;
    public float currentWindAffinity;
    public float currentLightAffinity;
    public float currentDarkAffinity;

    private PlayerStatsUI statsUI;

    void Start()
    {
        statsUI = Object.FindAnyObjectByType<PlayerStatsUI>();
        ResetStats();
    }

    public void ResetStats()
    {
        currentHP = baseHP;
        currentMP = baseMP;
        currentPhysicalAttack = basePhysicalAttack;
        currentPhysicalDefense = basePhysicalDefense;
        currentMagicalAttack = baseMagicalAttack;
        currentMagicalDefense = baseMagicalDefense;
        currentSpeed = baseSpeed;

        currentFireAffinity = baseFireAffinity;
        currentWaterAffinity = baseWaterAffinity;
        currentEarthAffinity = baseEarthAffinity;
        currentWindAffinity = baseWindAffinity;
        currentLightAffinity = baseLightAffinity;
        currentDarkAffinity = baseDarkAffinity;

        UpdateUI();
    }

    public void ApplyItemModifiers(ItemData item)
    {
        if (item == null) return;

        foreach (var modifier in item.statModifiers)
        {
            ApplyStatModifier(modifier);
        }

        UpdateUI();
    }

    private void ApplyStatModifier(StatModifier modifier)
    {
        float value = modifier.GetModifiedValue(1f); // Usar calidad 1 por ahora
        bool isPercentage = modifier.valueType == ModifierValueType.Percentage;
        
        switch (modifier.statType)
        {
            case StatType.Health:
                currentHP += isPercentage ? baseHP * value / 100f : value;
                break;
            case StatType.Mana:
                currentMP += isPercentage ? baseMP * value / 100f : value;
                break;
            case StatType.Attack:
                currentPhysicalAttack += isPercentage ? basePhysicalAttack * value / 100f : value;
                break;
            case StatType.Defense:
                currentPhysicalDefense += isPercentage ? basePhysicalDefense * value / 100f : value;
                break;
            case StatType.Speed:
                currentSpeed += isPercentage ? baseSpeed * value / 100f : value;
                break;
            case StatType.FireResistance:
                currentFireAffinity += isPercentage ? baseFireAffinity * value / 100f : value;
                break;
            case StatType.WaterResistance:
                currentWaterAffinity += isPercentage ? baseWaterAffinity * value / 100f : value;
                break;
            case StatType.EarthResistance:
                currentEarthAffinity += isPercentage ? baseEarthAffinity * value / 100f : value;
                break;
            case StatType.WindResistance:
                currentWindAffinity += isPercentage ? baseWindAffinity * value / 100f : value;
                break;
            case StatType.LightResistance:
                currentLightAffinity += isPercentage ? baseLightAffinity * value / 100f : value;
                break;
            case StatType.DarkResistance:
                currentDarkAffinity += isPercentage ? baseDarkAffinity * value / 100f : value;
                break;
        }
    }

    private void UpdateUI()
    {
        if (statsUI != null)
        {
            statsUI.UpdateAllStats(
                currentHP, currentMP,
                currentPhysicalAttack, currentPhysicalDefense,
                currentMagicalAttack, currentMagicalDefense,
                currentSpeed,
                currentFireAffinity, currentWaterAffinity,
                currentEarthAffinity, currentWindAffinity,
                currentLightAffinity, currentDarkAffinity,
                0f, 0f, 0f // Valores adicionales requeridos por el método
            );
        }
    }
}