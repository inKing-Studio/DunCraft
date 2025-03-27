using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Stats base del jugador
    public float baseHP = 100f;
    public float baseMP = 50f;
    public float basePhysicalAttack = 10f;
    public float basePhysicalDefense = 5f;
    public float baseMagicalAttack = 8f;
    public float baseMagicalDefense = 5f;
    public float baseSpeed = 5f;

    // Afinidades elementales base
    public float baseWaterAffinity = 0f;
    public float baseFireAffinity = 0f;
    public float baseEnergyAffinity = 0f;
    public float baseAirAffinity = 0f;
    public float baseEarthAffinity = 0f;
    public float baseLightAffinity = 0f;
    public float baseDarkAffinity = 0f;

    // Stats actuales (modificados por items)
    private float currentHP;
    private float currentMP;
    private float currentPhysicalAttack;
    private float currentPhysicalDefense;
    private float currentMagicalAttack;
    private float currentMagicalDefense;
    private float currentSpeed;

    // Afinidades actuales
    private float currentWaterAffinity;
    private float currentFireAffinity;
    private float currentEnergyAffinity;
    private float currentAirAffinity;
    private float currentEarthAffinity;
    private float currentLightAffinity;
    private float currentDarkAffinity;

    private PlayerStatsUI statsUI;

    void Start()
    {
        ResetStats();
        statsUI = FindAnyObjectByType<PlayerStatsUI>();
        if (statsUI != null)
        {
            UpdateUI();
        }
    }

    public void ResetStats()
    {
        // Resetear stats a sus valores base
        currentHP = baseHP;
        currentMP = baseMP;
        currentPhysicalAttack = basePhysicalAttack;
        currentPhysicalDefense = basePhysicalDefense;
        currentMagicalAttack = baseMagicalAttack;
        currentMagicalDefense = baseMagicalDefense;
        currentSpeed = baseSpeed;

        // Resetear afinidades
        currentWaterAffinity = baseWaterAffinity;
        currentFireAffinity = baseFireAffinity;
        currentEnergyAffinity = baseEnergyAffinity;
        currentAirAffinity = baseAirAffinity;
        currentEarthAffinity = baseEarthAffinity;
        currentLightAffinity = baseLightAffinity;
        currentDarkAffinity = baseDarkAffinity;
    }

    public void ApplyItemModifiers(ItemData item)
    {
        if (item == null || item.baseStatModifiers == null) return;

        foreach (var modifier in item.baseStatModifiers)
        {
            ApplyStatModifier(modifier);
        }

        UpdateUI();
    }

    public void RemoveItemModifiers(ItemData item)
    {
        if (item == null || item.baseStatModifiers == null) return;

        foreach (var modifier in item.baseStatModifiers)
        {
            RemoveStatModifier(modifier);
        }

        UpdateUI();
    }

    private void ApplyStatModifier(StatModifier modifier)
    {
        switch (modifier.Stat)
        {
            case StatType.HP:
                currentHP += modifier.Value;
                break;
            case StatType.MP:
                currentMP += modifier.Value;
                break;
            case StatType.PhysicalAttack:
                currentPhysicalAttack += modifier.Value;
                break;
            case StatType.PhysicalDefense:
                currentPhysicalDefense += modifier.Value;
                break;
            case StatType.MagicalAttack:
                currentMagicalAttack += modifier.Value;
                break;
            case StatType.MagicalDefense:
                currentMagicalDefense += modifier.Value;
                break;
            case StatType.Speed:
                currentSpeed += modifier.Value;
                break;
            case StatType.WaterAffinity:
                currentWaterAffinity += modifier.Value;
                break;
            case StatType.FireAffinity:
                currentFireAffinity += modifier.Value;
                break;
            case StatType.EnergyAffinity:
                currentEnergyAffinity += modifier.Value;
                break;
            case StatType.AirAffinity:
                currentAirAffinity += modifier.Value;
                break;
            case StatType.EarthAffinity:
                currentEarthAffinity += modifier.Value;
                break;
            case StatType.LightAffinity:
                currentLightAffinity += modifier.Value;
                break;
            case StatType.DarkAffinity:
                currentDarkAffinity += modifier.Value;
                break;
        }
    }

    private void RemoveStatModifier(StatModifier modifier)
    {
        switch (modifier.Stat)
        {
            case StatType.HP:
                currentHP -= modifier.Value;
                break;
            case StatType.MP:
                currentMP -= modifier.Value;
                break;
            case StatType.PhysicalAttack:
                currentPhysicalAttack -= modifier.Value;
                break;
            case StatType.PhysicalDefense:
                currentPhysicalDefense -= modifier.Value;
                break;
            case StatType.MagicalAttack:
                currentMagicalAttack -= modifier.Value;
                break;
            case StatType.MagicalDefense:
                currentMagicalDefense -= modifier.Value;
                break;
            case StatType.Speed:
                currentSpeed -= modifier.Value;
                break;
            case StatType.WaterAffinity:
                currentWaterAffinity -= modifier.Value;
                break;
            case StatType.FireAffinity:
                currentFireAffinity -= modifier.Value;
                break;
            case StatType.EnergyAffinity:
                currentEnergyAffinity -= modifier.Value;
                break;
            case StatType.AirAffinity:
                currentAirAffinity -= modifier.Value;
                break;
            case StatType.EarthAffinity:
                currentEarthAffinity -= modifier.Value;
                break;
            case StatType.LightAffinity:
                currentLightAffinity -= modifier.Value;
                break;
            case StatType.DarkAffinity:
                currentDarkAffinity -= modifier.Value;
                break;
        }
    }

    private void UpdateUI()
    {
        if (statsUI != null)
        {
            statsUI.UpdateAllStats(
                currentHP, baseHP,
                currentMP, baseMP,
                currentPhysicalAttack, currentPhysicalDefense,
                currentMagicalAttack, currentMagicalDefense,
                currentSpeed,
                currentWaterAffinity, currentFireAffinity,
                currentEnergyAffinity, currentAirAffinity,
                currentEarthAffinity, currentLightAffinity,
                currentDarkAffinity
            );
        }
    }

    // Getters para los stats actuales
    public float GetCurrentHP() => currentHP;
    public float GetCurrentMP() => currentMP;
    public float GetCurrentPhysicalAttack() => currentPhysicalAttack;
    public float GetCurrentPhysicalDefense() => currentPhysicalDefense;
    public float GetCurrentMagicalAttack() => currentMagicalAttack;
    public float GetCurrentMagicalDefense() => currentMagicalDefense;
    public float GetCurrentSpeed() => currentSpeed;
    public float GetCurrentWaterAffinity() => currentWaterAffinity;
    public float GetCurrentFireAffinity() => currentFireAffinity;
    public float GetCurrentEnergyAffinity() => currentEnergyAffinity;
    public float GetCurrentAirAffinity() => currentAirAffinity;
    public float GetCurrentEarthAffinity() => currentEarthAffinity;
    public float GetCurrentLightAffinity() => currentLightAffinity;
    public float GetCurrentDarkAffinity() => currentDarkAffinity;
}