using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("Basic Info")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    [Header("HP")]
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI hpValueText;
    public Slider hpSlider;

    [Header("MP")]
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI mpValueText;
    public Slider mpSlider;

    [Header("Combat Stats")]
    public TextMeshProUGUI physicalAttackText;
    public TextMeshProUGUI physicalDefenseText;
    public TextMeshProUGUI magicalAttackText;
    public TextMeshProUGUI magicalDefenseText;
    public TextMeshProUGUI speedText;

    [Header("Affinities")]
    public TextMeshProUGUI waterAffinityText;
    public TextMeshProUGUI fireAffinityText;
    public TextMeshProUGUI energyAffinityText;
    public TextMeshProUGUI airAffinityText;
    public TextMeshProUGUI earthAffinityText;
    public TextMeshProUGUI lightAffinityText;
    public TextMeshProUGUI darkAffinityText;

    public void UpdateAllStats(
        float hp, float maxHp,
        float mp, float maxMp,
        float physicalAttack, float physicalDefense,
        float magicalAttack, float magicalDefense,
        float speed,
        float waterAffinity, float fireAffinity,
        float energyAffinity, float airAffinity,
        float earthAffinity, float lightAffinity,
        float darkAffinity)
    {
        // HP
        if (hpText != null)
            hpText.text = "HP";
        if (hpValueText != null)
            hpValueText.text = $"{hp:F0}/{maxHp:F0}";
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHp;
            hpSlider.value = hp;
        }

        // MP
        if (mpText != null)
            mpText.text = "MP";
        if (mpValueText != null)
            mpValueText.text = $"{mp:F0}/{maxMp:F0}";
        if (mpSlider != null)
        {
            mpSlider.maxValue = maxMp;
            mpSlider.value = mp;
        }

        // Combat Stats
        if (physicalAttackText != null)
            physicalAttackText.text = $"Ataque Físico: {physicalAttack:F1}";
        if (physicalDefenseText != null)
            physicalDefenseText.text = $"Defensa Física: {physicalDefense:F1}";
        if (magicalAttackText != null)
            magicalAttackText.text = $"Ataque Mágico: {magicalAttack:F1}";
        if (magicalDefenseText != null)
            magicalDefenseText.text = $"Defensa Mágica: {magicalDefense:F1}";
        if (speedText != null)
            speedText.text = $"Velocidad: {speed:F1}";

        // Affinities
        if (waterAffinityText != null)
            waterAffinityText.text = $"Agua: {waterAffinity:F1}";
        if (fireAffinityText != null)
            fireAffinityText.text = $"Fuego: {fireAffinity:F1}";
        if (energyAffinityText != null)
            energyAffinityText.text = $"Energía: {energyAffinity:F1}";
        if (airAffinityText != null)
            airAffinityText.text = $"Aire: {airAffinity:F1}";
        if (earthAffinityText != null)
            earthAffinityText.text = $"Tierra: {earthAffinity:F1}";
        if (lightAffinityText != null)
            lightAffinityText.text = $"Luz: {lightAffinity:F1}";
        if (darkAffinityText != null)
            darkAffinityText.text = $"Oscuridad: {darkAffinity:F1}";
    }
}