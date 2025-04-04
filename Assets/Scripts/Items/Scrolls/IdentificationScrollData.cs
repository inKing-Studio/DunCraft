using UnityEngine;

[CreateAssetMenu(fileName = "New Identification Scroll", menuName = "DunCraft/Scrolls/Identification Scroll")]
public class IdentificationScrollData : ConsumableData
{
    [Header("Identificación")]
    [Range(0f, 1f)]
    public float identificationPercentage = 0.25f;

    public override string GetTooltipText()
    {
        string text = base.GetTooltipText();
        text += $"\nPorcentaje de identificación: {identificationPercentage:P0}";
        return text;
    }
}