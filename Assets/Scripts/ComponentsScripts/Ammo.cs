using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int weight;
    [SerializeField] private int penetration_value;
    [SerializeField] private int damage_value;
    public int GetDamage() => damage_value;
    [SerializeField] private int falloff_value;
    
    [SerializeField] private float speed_value;
    public float GetSpeed() => speed_value;
    [SerializeField] private AmmoType ammo_type;
    public GameObject GetBullet() => bullet;
     [SerializeField] private GameObject bullet;
    [SerializeField] private Rigidbody body;

    [SerializeField] private bool collided = false;

    private void Start()
    {
        //this.gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
        if (collision.gameObject.CompareTag("tank"))
        {
            collision.gameObject.GetComponent<Tank>().GetHull().TakeDamage(damage_value);
        }
        OnDestroy();
    }

    private Vector3 Physics()
    {
        float angle = this.gameObject.transform.rotation.y;
        float force_up = this.weight * this.SpeedUpdate(angle);
        float down_force = this.weight * 9.81f;
        

        force_up = force_up * Mathf.Sin(angle);
        down_force = down_force * Mathf.Cos(angle);

        return new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - (force_up - down_force), this.gameObject.transform.position.z);
    }

    private float SpeedUpdate(float angle)
    {
        this.speed_value = speed_value - weight * (speed_value * Mathf.Cos(angle) - 9.81f * Mathf.Sin(angle));
        return speed_value;
    }

    public void Travel()
    {
        /*if (!collided)
        {
            this.gameObject.transform.position -= Physics();
            Travel();
        }*/

        body.AddForce(transform.forward * speed_value, ForceMode.Impulse);
        body.useGravity = true;
             
    }

    public void Shoot()
    {

    }


    private void OnDestroy()
    {
        this.gameObject.SetActive(false);
    }
}


public enum AmmoType
{
    Armor_Piercing,
    High_Explosive,
    High_Explosive_Anti_Tank,
    Armor_Piercing_Composite_Rigid

}
