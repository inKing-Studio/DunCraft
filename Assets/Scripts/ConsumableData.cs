using UnityEngine;

public class ConsumableData : ItemData
{
    [Header("Consumible")]
    public ConsumableType consumableType;
    public float duration = 0f; // Para efectos temporales de pociones
    public float effectValue = 0f; // Cantidad de curación, daño, etc.
    public bool isInstant = true; // Si el efecto es instantáneo o por tiempo
    public string targetSkill; // Para scrolls de skill, el nombre de la skill que enseña

    public override string GetTooltipText()
    {
        string text = $"{itemName}\n";
        text += $"Rareza: {rarity}\n";
        
        if (!string.IsNullOrEmpty(description))
        {
            text += $"\n{description}\n";
        }

        switch (consumableType)
        {
            case ConsumableType.HealthPotion:
            case ConsumableType.ManaPotion:
            case ConsumableType.PoisonPotion:
                text += $"\nEfecto: {effectValue:F0}";
                if (!isInstant)
                    text += $" durante {duration:F1} segundos";
                break;

            case ConsumableType.SkillScroll:
                text += $"\nEnseña: {targetSkill}";
                break;

            case ConsumableType.RecipeScroll:
            case ConsumableType.IdentificationScroll:
                // Estos no necesitan información adicional
                break;
        }

        return text;
    }
}