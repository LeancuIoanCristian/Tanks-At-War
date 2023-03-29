using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int penetration_value;
    [SerializeField] public int damage_value;
    [SerializeField] private int falloff_value;
    [SerializeField] private int speed_value;
    [SerializeField] private AmmoType ammo_type;

    private void Start()
    {
        this.gameObject.AddComponent<CharacterController>();
        this.gameObject.SetActive(false);
    }

    public void Shoot()
    {
        this.gameObject.SetActive(true);
        this.GetComponent<CharacterController>().Move(this.transform.position * speed_value * Time.deltaTime);
    }
}


public enum AmmoType
{
    Armor_Piercing,
    High_Explosive,
    High_Explosive_Anti_Tank,
    Armor_Piercing_Composite_Rigid

}
