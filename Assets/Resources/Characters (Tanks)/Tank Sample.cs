using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankSample : ScriptableObject
{
    public GameObject Tank_Prefab;
    public Tank Tank_Object;
}

[CreateAssetMenu(fileName = "Tank", menuName = "ScriptableObjects/SpawnManagerScriptableObject/Tank")]
public class Tank : ScriptableObject
{
    [SerializeField]
    private string tank_name;

    [SerializeField]
    private int tank_health;

    [SerializeField]
    private Hull tank_hull;

    [SerializeField]
    private Radio tank_radio;

    [SerializeField]
    private Engine tank_engine;

    [SerializeField]
    private Ammo tank_ammo_1;

    [SerializeField]
    private Ammo tank_ammo_2;

    [SerializeField]
    private Ammo tank_ammo_3;

    [SerializeField]
    private Gun tank_gun;

    [SerializeField]
    private Tracks tank_tracks;

    [SerializeField]
    private Turret tank_turret;

    public Tank (TankType type)
    {
        if(type == TankType.Destroyer)
        {
            tank_turret.horizontal_constrains = true;
        }
    }
}

public enum TankType
{
    Light,
    Medium,
    Heavy,
    Destroyer,
    Artilery
}