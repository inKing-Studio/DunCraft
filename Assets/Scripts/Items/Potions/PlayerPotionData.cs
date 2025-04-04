using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Player Potion", menuName = "DunCraft/Potions/Player Potion")]
public class PlayerPotionData : PotionData
{
    public enum PotionEffect
    {
        RestoreHealth,
        RestoreMana,
        IncreaseDefense,
        IncreaseMagicResistance,
        IncreaseStrength,
        IncreaseDexterity,
        IncreaseIntelligence,
        Invisibility,
        SpeedBoost
    }

    [Header("Efectos en el Jugador")]
    public PotionEffect effectType;
    public List<StatModifier> additionalModifiers = new List<StatModifier>();
    public bool isTemporary = true;

    public override string GetEffectDescription()
    {
        string description = "Efecto: ";
        
        switch (effectType)
        {
            case PotionEffect.RestoreHealth:
                description += $"Restaura {potency} puntos de salud";
                break;
            case PotionEffect.RestoreMana:
                description += $"Restaura {potency} puntos de maná";
                break;
            case PotionEffect.IncreaseDefense:
                description += $"Aumenta la defensa en {potency}";
                break;
            case PotionEffect.IncreaseMagicResistance:
                description += $"Aumenta la resistencia mágica en {potency}";
                break;
            case PotionEffect.IncreaseStrength:
                description += $"Aumenta la fuerza en {potency}";
                break;
            case PotionEffect.IncreaseDexterity:
                description += $"Aumenta la destreza en {potency}";
                break;
            case PotionEffect.IncreaseIntelligence:
                description += $"Aumenta la inteligencia en {potency}";
                break;
            case PotionEffect.Invisibility:
                description += "Vuelve al jugador invisible";
                break;
            case PotionEffect.SpeedBoost:
                description += $"Aumenta la velocidad en {potency}%";
                break;
        }

        if (isTemporary)
        {
            description += $" durante {duration} segundos";
        }

        if (additionalModifiers.Count > 0)
        {
            description += "\n\nEfectos adicionales:";
            foreach (var modifier in additionalModifiers)
            {
                description += $"\n- {modifier.statType}: {(modifier.valueType == ModifierValueType.Percentage ? modifier.value + "%" : modifier.value.ToString())}";
            }
        }

        return description;
    }
}