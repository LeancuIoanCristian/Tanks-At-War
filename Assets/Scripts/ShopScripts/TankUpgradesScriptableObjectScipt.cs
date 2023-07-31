using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesManagerObject", menuName ="Upgrades")]
public class TankUpgradesScriptableObjectScipt : ScriptableObject
{
    [SerializeField] private List<Upgrades> upgrades_list;
    private const int base_health_up = 10;
    private const int base_damage_up = 100;
    private const float base_currency_up = 0.5f;

    public int GetHealthUp() => base_health_up;
    public int GetDamageUp() => base_damage_up;
    public float GetCurrencyUp() => base_currency_up;

    public void IncrementUpgradesDone(UpgradeType type)
    {
        for (int index = 0 ; index < upgrades_list.Count; index++)
        {
            if (type == upgrades_list[index].GetUpgradeType())
            {
                upgrades_list[index].IncreaseUpgradeValue(1);
                break;
            }
        }

       
    }

    public Upgrades GetUpgradesReferenceFromSaveFile(int index) => upgrades_list[index]; 

    public int NumberOfUpgradesDone(UpgradeType type)
    {
        int found = 0;
        for (int index = 0; index < upgrades_list.Count; index++, found++)
        {
            if (type == upgrades_list[index].GetUpgradeType())
            {
                break;
            }
        }

        return upgrades_list[found].GetUpgradesDone();
    }

    public int RetriveUpgrade(UpgradeType upgrade_type_reference)
    {
        int found = 0;
        for (int index = 0; index < upgrades_list.Count; index++, found++)
        {
            if (upgrade_type_reference == upgrades_list[index].GetUpgradeType())
            {
                break;
            }
        }

        return 1 + upgrades_list[found].GetUpgradesDone();
    }

    public void ResetValues()
    {
        for (int index = 0; index < upgrades_list.Count; index++)
        {           
            upgrades_list[index].SetUpgradesDone(1);            
        }
    }
}

