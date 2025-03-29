using UnityEngine;

public static class QualitySystem
{
    // Rangos de calidad y sus porcentajes de rareza
    private static readonly (float min, float max, float rarity)[] QualityRanges = new[]
    {
        (1f, 25f, 0.10f),   // 10% chance
        (26f, 50f, 0.20f),  // 20% chance
        (51f, 75f, 0.30f),  // 30% chance
        (76f, 100f, 0.40f)  // 40% chance
    };

    public static Rarity GetRarityFromQuality(float quality)
    {
        if (quality >= 76f) return Rarity.Epic;
        if (quality >= 51f) return Rarity.Rare;
        if (quality >= 26f) return Rarity.Uncommon;
        return Rarity.Common;
    }

    public static float GenerateRandomQuality()
    {
        float random = Random.value;
        float cumulative = 0f;
        
        for (int i = 0; i < QualityRanges.Length; i++)
        {
            cumulative += QualityRanges[i].rarity;
            if (random <= cumulative)
            {
                return Random.Range(QualityRanges[i].min, QualityRanges[i].max);
            }
        }
        
        return QualityRanges[0].min;
    }

    public static float CalculateAverageQuality(float quality1, float quality2)
    {
        return (quality1 + quality2) / 2f;
    }

    public static string GetQualityDescription(float quality)
    {
        if (quality >= 76f) return "Excepcional";
        if (quality >= 51f) return "Superior";
        if (quality >= 26f) return "Decente";
        return "Común";
    }
}