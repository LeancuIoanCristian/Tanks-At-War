using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, IMovableObjects, IDamageable
{
    [SerializeField] private float accuracy;
    public Ammo current_ammo;
    [SerializeField] private Ammo tank_ammo_1;
    [SerializeField] private Ammo tank_ammo_2;
    [SerializeField] private Ammo tank_ammo_3;

    [SerializeField] private int weight;

    public bool vertical_constrains;
    [SerializeField] private float gun_up_constrain;
    [SerializeField] private float gun_down_constrain;

    private void Start()
    {
        current_ammo = tank_ammo_1;
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ShellChange();
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

    public void Gun_Verticalal_Constrains(bool vertical_constrains, float up_limit, float down_limit)
    {
        if (vertical_constrains)
        {
            gun_up_constrain = up_limit;
            gun_down_constrain = down_limit;
        }
    }

    public void MovementDirection()
    {

    }

    public void GiveDamage(int damage_value, Camera player_camera)
    {
        RaycastHit hit;
        Ray ray = player_camera.ScreenPointToRay(Input.mousePosition);

        current_ammo.Shoot();

        if (Physics.Raycast(ray, out hit))
        {
            var obj = hit.transform;
            
            if (obj.GetComponent<IDamageable>() != null)
            {
                obj.GetComponent<Hull>().TakeDamage(current_ammo.damage_value);
            }
        }
    }

    public void TakeDamage(int damage_value)
    {

    }
}
