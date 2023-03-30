using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[CreateAssetMenu(fileName = "Tank", menuName = "ScriptableObjects/SpawnManagerScriptableObject/Tank")]
//public class TankSample : ScriptableObject
//{
//    public GameObject Tank_Prefab;
//    public Tank Tank_Object;
//}


public class Tank : MonoBehaviour,  IDestroyable
{
    [SerializeField] private string tank_name;
    [SerializeField] private Hull tank_hull;
    public Hull GetHull() => tank_hull;
    [SerializeField] private Turret tank_turret;
    public Turret GetTurret() => tank_turret;
    public Tank (TankType type)
    {
        if(type == TankType.Destroyer)
        {
            tank_turret.horizontal_constrains = true;
        }
    }

    public void  Destroy()
    {

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
