using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Gun : MonoBehaviour, IMovableObjects, IDamageable
{
    [SerializeField] private float accuracy;
    [SerializeField] private Transform barrel_end;
    [SerializeField] private Ammo current_ammo;
    public Ammo GetCurrentAmmo() => current_ammo;
    public void SetAmmo(Ammo ammo_reference) => current_ammo = ammo_reference;
    
    
    [SerializeField] private float reload_time = 3f;
    [SerializeField] private float reloading = 3f;
    public float GetReload() => reloading;
    [SerializeField] private int weight;
   
    public bool vertical_constrains;
    [SerializeField] private float gun_up_constrain;
    public float GetGunUpConstrain() => gun_up_constrain;

    [SerializeField] private float gun_down_constrain;
    public float GetGunDownConstrain() => gun_down_constrain;

    public Transform GetBarrelEnd() => barrel_end;
    private void Start()
    {
       
    }

    private void Update()
    {
        TurnAction();

    }

    private void TurnAction()
    {
        if (reloading < reload_time)
        {
            reloading += 1f * Time.deltaTime;
        }
    }
    
    public void MovementDirection()
    {

    }

    public void GiveDamage()
    {
        if (reloading >= reload_time)
        {
            var bullet = Instantiate(current_ammo.GetBullet(), barrel_end.position, barrel_end.rotation);

            bullet.GetComponent<Ammo>().Travel(transform);

            reloading = 0.0f;
        }
    }

    public void GiveDamagePulse(Transform camera)
    {
        if (reloading >= reload_time)
        {
            var bullet = Instantiate(current_ammo.GetBullet(), barrel_end.position, barrel_end.rotation);

            bullet.GetComponent<Ammo>().Travel(camera);

            reloading = 0.0f;
        }
    }



    public bool CanShoot()
    {
        return reloading >= reload_time;
    }

    public void TakeDamage(int damage_value)
    {
        
    }
}
