using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Ammo", menuName = "Tank Parts/ Ammo")]
[System.Serializable]
public class Ammo : MonoBehaviour
{
    [SerializeField]
    private int penetration_value;

    [SerializeField]
    private int damage_value;

    [SerializeField]
    private int falloff_value;

    [SerializeField]
    private int speed_value;

    [SerializeField]
    private AmmoType ammo_type;
}

public enum AmmoType
{
    Armor_Piercing,
    High_Explosive,
    High_Explosive_Anti_Tank,
    Armor_Piercing_Composite_Rigid

}
