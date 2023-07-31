using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private TankUpgradesScriptableObjectScipt upgrades_references;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnResetPressed()
    {
        upgrades_references.ResetValues();
    }
}
