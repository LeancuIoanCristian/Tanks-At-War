using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tank : MonoBehaviour, IDestroyable
{
    [SerializeField] private string tank_name;
    [SerializeField] private Hull tank_hull;
    public Hull GetHull() => tank_hull;
    [SerializeField] private Turret tank_turret;
    public Turret GetTurret() => tank_turret;
    public void SetHull(Hull hull_reference) => tank_hull = hull_reference;
    public void SetTurret(Turret turret_reference) => tank_turret = turret_reference;

    public Tank()
    {
    }
    public Tank (TankType type)
    {
        if(type == TankType.Destroyer)
        {
            tank_turret.horizontal_constrains = true;
        }
    }

    public void  IDestroy()
    {
        this.gameObject.SetActive(false);
        
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
