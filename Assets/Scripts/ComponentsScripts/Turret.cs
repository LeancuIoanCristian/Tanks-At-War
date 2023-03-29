using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IMovableObjects
{

    //armor
    [SerializeField] private int armor_front;
    [SerializeField] private int armor_side;
    [SerializeField] private int armor_rear;

    //rotation constrains
    [SerializeField] private float turret_rotation;
    public bool horizontal_constrains;
    [SerializeField] private float turret_left_constrain;
    [SerializeField] private float turret_right_constrain;

    //firepower
    [SerializeField] private Gun tank_gun;
    
    [SerializeField] private int view_range; //variable that determines until which distance the tank can spot/see enemies
    [SerializeField] private int weight;
    [SerializeField] private Radio tank_radio;

    private void Update()
    {
        
    }

    public void Gun_Horizontal_Constrains (bool horizontal_constrains, float left_limit, float right_limit)
    {
        if (horizontal_constrains)
        {
            turret_left_constrain = left_limit;
            turret_right_constrain = right_limit;
        }
    }

    public void MovementDirection()
    {

    }
}
