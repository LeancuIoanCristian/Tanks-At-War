using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hull : MonoBehaviour
{
    [SerializeField] private int tank_health;
    [SerializeField] private Tracks tank_tracks;
    [SerializeField] private Engine tank_engine;
    //armor
    [SerializeField] private int armor_front;
    [SerializeField] private int armor_side;
    [SerializeField] private int armor_rear;
    [SerializeField] private int weight;
    
}