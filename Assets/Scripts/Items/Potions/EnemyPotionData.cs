using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Enemy Potion", menuName = "DunCraft/Potions/Enemy Potion")]
public class EnemyPotionData : PotionData
{
    public enum PotionEffect
    {
        Poison,
        Weakness,
        Slowness,
        ReduceDefense,
        ReduceMagicResistance,
        Sleep,
        Confusion,
        Paralysis
    }

    [Header("Efectos en el Enemigo")]
    public PotionEffect effectType;
    public float radius = 1f;
    public bool canSpread = false;
    public List<StatModifier> debuffs = new List<StatModifier>();

    [Header("Daño sobre Tiempo")]
    public bool hasDamageOverTime = false;
    public float damagePerTick = 0f;
    public float tickInterval = 1f;

    public override string GetEffectDescription()
    {
        string description = "Efecto: ";
        
        switch (effectType)
        {
            case PotionEffect.Poison:
                description += $"Envenena al objetivo causando {damagePerTick} de daño cada {tickInterval} segundos";
                break;
            case PotionEffect.Weakness:
                description += "Reduce la fuerza del objetivo";
                break;
            case PotionEffect.Slowness:
                description += $"Reduce la velocidad del objetivo en {potency}%";
                break;
            case PotionEffect.ReduceDefense:
                description += $"Reduce la defensa del objetivo en {potency}";
                break;
            case PotionEffect.ReduceMagicResistance:
                description += $"Reduce la resistencia mágica del objetivo en {potency}";
                break;
            case PotionEffect.Sleep:
                description += "Pone al objetivo a dormir";
                break;
            case PotionEffect.Confusion:
                description += "Confunde al objetivo";
                break;
            case PotionEffect.Paralysis:
                description += "Paraliza al objetivo";
                break;
        }

        description += $" durante {duration} segundos";
        description += $"\nRadio de efecto: {radius} metros";

        if (canSpread)
        {
            description += "\nEl efecto puede propagarse a enemigos cercanos";
        }

        if (debuffs.Count > 0)
        {
            description += "\n\nDebuffs aplicados:";
            foreach (var debuff in debuffs)
            {
                description += $"\n- {debuff.statType}: {(debuff.valueType == ModifierValueType.Percentage ? debuff.value + "%" : debuff.value.ToString())}";
            }
        }

        return description;
    }
}