using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Hull", menuName = "Tank Parts/Hull")]
[System.Serializable]
public class Hull : MonoBehaviour
{
    [SerializeField]
    private int armor_front;

    [SerializeField]
    private int armor_side;

    [SerializeField]
    private int armor_rear;

    [SerializeField]
    private int weight;
    
}
