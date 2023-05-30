using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradesManagerObject", menuName ="Upgrades")]
public class TankUpgradesScriptableObjectScipt : ScriptableObject
{
    [SerializeField] private List<Upgrades> upgrades_list;
}

