using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesManagerObject", menuName ="Upgrades")]
public class TankUpgradesScriptableObjectScipt : ScriptableObject
{
    [SerializeField] private List<Upgrades> upgrades_list;

    public int GetUpgradesDone(UpgradeType type)
    {
        int found = 0;
        for (int index = 0 ; index < upgrades_list.Count; index++)
        {
            if (type == upgrades_list[index].GetUpgradeType())
            {
                found = index;
                break;
            }
        }

        return upgrades_list[found].GetUpgradesDone();
    }
}

