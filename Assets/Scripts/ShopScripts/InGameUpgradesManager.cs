using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUpgradesManager : MonoBehaviour
{
    [SerializeField] private TankUpgradesScriptableObjectScipt upgrades_reference;
    [SerializeField] private List<Upgrades> shops;
    [SerializeField] private int max_number_of_shops = 20;

    // Start is called before the first frame update
    void Start()
    {
        shops.Capacity = max_number_of_shops;
    }

   
}
