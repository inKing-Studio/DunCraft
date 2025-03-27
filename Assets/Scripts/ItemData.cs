using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    
    [ReadOnly]
    [SerializeField]
    private Rarity rarity;
    public Rarity Rarity { get => rarity; set { rarity = value; UpdateDescription(); } }
    
    [ReadOnly]
    [SerializeField]
    private float quality;
    public float Quality { get => quality; set { quality = value; UpdateDescription(); } }
    
    public MaterialCategory Category;
    
    [TextArea(3, 10)]
    public string description;
    
    [Header("Properties")]
    public float cost;
    public List<StatModifier> baseStatModifiers = new List<StatModifier>();
    
    [Header("Material Properties")]
    public float durability;
    public float weight;
    public float flexibility;
    public float conductivity;
    public float resonance;
    
    private static readonly float[] RARITY_PROBABILITIES = new float[] { 0.4f, 0.3f, 0.2f, 0.1f };
    private static readonly float[] QUALITY_THRESHOLDS = new float[] { 25f, 50f, 75f, 100f };

    public void GenerateQualityAndRarity()
    {
        float randomValue = Random.value;
        float cumulativeProbability = 0f;
        
        for (int i = 0; i < RARITY_PROBABILITIES.Length; i++)
        {
            cumulativeProbability += RARITY_PROBABILITIES[i];
            if (randomValue <= cumulativeProbability)
            {
                rarity = (Rarity)i;
                // Generar calidad dentro del rango correspondiente
                float minQuality = i > 0 ? QUALITY_THRESHOLDS[i - 1] : 0f;
                float maxQuality = QUALITY_THRESHOLDS[i];
                quality = Random.Range(minQuality, maxQuality);
                break;
            }
        }
        
        UpdateDescription();
    }

    public void UpdateDescription()
    {
        string baseDesc = description;
        string qualityDesc = $"\nCalidad: {quality:F1}";
        string rarityDesc = $"\nRareza: {rarity}";
        string statsDesc = "\nModificadores:";
        
        foreach (var mod in baseStatModifiers)
        {
            statsDesc += $"\n{mod.Stat}: {(mod.IsPercentage ? mod.Value + "%" : mod.Value.ToString("F1"))}";
        }
        
        description = baseDesc + qualityDesc + rarityDesc + statsDesc;
    }

    public ItemData CreateInstance()
    {
        ItemData newInstance = Instantiate(this);
        newInstance.GenerateQualityAndRarity();
        return newInstance;
    }
}