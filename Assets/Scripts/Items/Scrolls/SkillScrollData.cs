using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Scroll", menuName = "DunCraft/Scrolls/Skill Scroll")]
public class SkillScrollData : ConsumableData
{
    [Header("Habilidad")]
    public int skillId;
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;

    [Header("Requisitos")]
    public int requiredLevel;
    public StatType primaryAttribute;
    public int requiredAttributeValue;

    public override string GetTooltipText()
    {
        string text = base.GetTooltipText();
        text += $"\n\nHabilidad: {skillName}";
        text += $"\n{skillDescription}";
        text += $"\n\nRequisitos:";
        text += $"\nNivel: {requiredLevel}";
        text += $"\n{primaryAttribute}: {requiredAttributeValue}";
        return text;
    }
}