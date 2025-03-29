[System.Serializable]
public class StatModifier
{
    public StatType statType;
    public ModifierValueType valueType;
    public float value;

    public StatModifier(StatType statType, ModifierValueType valueType, float value)
    {
        this.statType = statType;
        this.valueType = valueType;
        this.value = value;
    }

    public float GetModifiedValue(float quality)
    {
        return value * quality;
    }
}