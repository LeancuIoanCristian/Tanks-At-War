using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Tracks", menuName = "Tank Parts/ Tracks")]
[System.Serializable]
public class Tracks : MonoBehaviour
{
    [SerializeField]
    private int weight;

    [SerializeField]
    private int weight_capacity;

    [SerializeField]
    private float speed_top;

    [SerializeField]
    private float speed_back;

    [SerializeField]
    private float turning_speed;
}
