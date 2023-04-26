using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hull : MonoBehaviour, IDamageable
{
    [SerializeField] private int tank_health;
    [SerializeField] private Tracks tank_tracks;
    public Tracks GetTracks() => tank_tracks;
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

    private void OnCollisionEnter(Collision collision)
    {
        var rigid_body = collision.gameObject.GetComponent<Ammo>().last_velocity;
        Vector3 collision_face_normal = collision.contacts[0].normal;
        Vector3 bullet_direction = rigid_body;

        float impact_angle = Vector3.Angle(collision_face_normal, -bullet_direction);

        Debug.DrawRay(collision.contacts[0].point, collision_face_normal, Color.red, 10f);
        Debug.DrawRay(collision.contacts[0].point, -bullet_direction, Color.green, 10f);

        print(impact_angle);

        

    }
}
