using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField]
    private int signal_range; 
    /*variable that determines till which distance the tanks are visible
     *dependent on the tank and tank type:
     * - light and heavy tanks - lowest,
     * - medium tanks - medium,
     * - tank destroyers and artilery - longest
     */
    
    [SerializeField]
    private int weight;
    
}
