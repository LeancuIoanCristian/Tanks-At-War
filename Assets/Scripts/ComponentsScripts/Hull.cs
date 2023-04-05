using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hull : MonoBehaviour, IDamageable
{
    [SerializeField] private int tank_health;
    [SerializeField] private Tracks tank_tracks;
    [SerializeField] private Engine tank_engine;
    //armor
    [SerializeField] private int armor_front;
    [SerializeField] private int armor_side;
    [SerializeField] private int armor_rear;
    [SerializeField] private int weight;

  
   
    
    private void Start()
    {
       
    }

    public void Update()
    {
        
    }

    public void GiveDamage()
    {

    }

    public void TakeDamage(int damage_value)
    {
       tank_health -= damage_value;
        Debug.Log(tank_health);
        if (tank_health <= 0)
        {
            this.GetComponentInParent<Tank>().Destroy();
        }

    }
}
