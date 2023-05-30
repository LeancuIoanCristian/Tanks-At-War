using UnityEngine;  

[System.Serializable]
public struct Upgrades
{
    [SerializeField] private string upgrade_name;
    [SerializeField] private int saved_upgrade_value;
    [SerializeField] private int upgrades_done;
    [SerializeField] private float upgrades_multiplier;
    [SerializeField] private UpgradeType upgrade_type;

    public int GetUpgrade() => saved_upgrade_value;
    public void SetStartValueUpgrades(int player_upgrades_done)
    {
        upgrades_done = player_upgrades_done;
    }

    public void IncreaseUpgradeValue()
    {
        upgrades_done++;
    }

    public void SaveUpgrades(UpgradeType type, int upgrade_value)
    {

    }

    public UpgradeType GetUpgradeType() => upgrade_type;
}

public enum UpgradeType
{
    Health,
    Damage,
    CurrencyMultiplier
}