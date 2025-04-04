using UnityEngine;

public abstract class PotionData : ConsumableData
{
    [Header("Poción Base")]
    public new float duration;
    public float potency;
    public Color potionColor = Color.white;
    
    [Header("Efectos Visuales")]
    public ParticleSystem useEffect;
    public AudioClip useSound;

    public abstract string GetEffectDescription();

    public override string GetTooltipText()
    {
        string text = base.GetTooltipText();
        text += $"\n\nDuración: {duration} segundos";
        text += $"\nPotencia: {potency}";
        text += $"\n\n{GetEffectDescription()}";
        return text;
    }
}