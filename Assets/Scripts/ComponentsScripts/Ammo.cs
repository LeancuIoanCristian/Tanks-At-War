using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private Playerlookscript look_reference;
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

    private void Start()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("tank"))
        {
            collision.gameObject.GetComponent<Tank>().GetHull().TakeDamage(damage_value);
        }
        OnDestroy();
    }

    public void Travel()
    {       
        body.AddForce(look_reference.ActiveCamera().transform.forward * speed_value, ForceMode.Impulse);
        body.useGravity = true;             
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
