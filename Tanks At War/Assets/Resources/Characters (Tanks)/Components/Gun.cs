using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IMovableObjects
{
    [SerializeField]
    private float accuracy;

    [SerializeField]
    private int weight;

    public bool horizontal_constrains;

    [SerializeField]
    private float turret_left_constrain;

    [SerializeField]
    private float turret_right_constrain;
    public void Gun_Horizontal_Constrains(bool horizontal_constrains, float left_limit, float right_limit)
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
