using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Gun : MonoBehaviour, IMovableObjects, IDamageable
{
    [SerializeField] private float accuracy;
    [SerializeField] private Transform barrel_end;
    [SerializeField] private Ammo current_ammo;
    public Ammo GetCurrentAmmo() => current_ammo;
    [SerializeField] private Ammo tank_ammo_1;
    [SerializeField] private Ammo tank_ammo_2;
    [SerializeField] private Ammo tank_ammo_3;
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
        current_ammo = tank_ammo_1;
    }

    private void Update()
    {
        TurnAction();

    }

    private void TurnAction()
    {
        if (Input.anyKeyDown)
        {
            ShellChange();
        }

        if (reloading < reload_time)
        {
            reloading += 1f * Time.deltaTime;
        }
    }

    private void ShellChange()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && current_ammo != tank_ammo_1)
        {
            current_ammo = tank_ammo_1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && current_ammo != tank_ammo_2)
        {
            current_ammo = tank_ammo_2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && current_ammo != tank_ammo_3)
        {
            current_ammo = tank_ammo_3;
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
