using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Turret", menuName = "Tank Parts/ Turret")]
[System.Serializable]
public class Turret : MonoBehaviour
{
    [SerializeField]
    private int armor_front;

    [SerializeField]
    private int armor_side;

    [SerializeField]
    private int armor_rear;

    [SerializeField]
    private float turret_rotation;

    [SerializeField]
    private int view_range; //variable that determines until which distance the tank can spot/see enemies

    [SerializeField]
    private int weight;

    public bool horizontal_constrains;

    [SerializeField]
    private float turret_left_constrain;

    [SerializeField]
    private float turret_right_constrain;
    public void Gun_Horizontal_Constrains (bool horizontal_constrains, float left_limit, float right_limit)
    {
        if (horizontal_constrains)
        {
            turret_left_constrain = left_limit;
            turret_right_constrain = right_limit;
        }
    }
}
