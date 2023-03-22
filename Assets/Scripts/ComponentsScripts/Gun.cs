using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IMovableObjects, IDamageable
{
    [SerializeField] private float accuracy;
    [SerializeField] private Ammo tank_ammo_1;
    [SerializeField] private Ammo tank_ammo_2;
    [SerializeField] private Ammo tank_ammo_3;

    [SerializeField] private int weight;

    public bool vertical_constrains;
    [SerializeField] private float gun_up_constrain;
    [SerializeField] private float gun_down_constrain;
   
    public void Gun_Verticalal_Constrains(bool vertical_constrains, float up_limit, float down_limit)
    {
        if (vertical_constrains)
        {
            gun_up_constrain = up_limit;
            gun_down_constrain = down_limit;
        }
    }

    public void MovementDirection()
    {

    }

    public void GiveDamage(float damage_value)
    {

    }
}
