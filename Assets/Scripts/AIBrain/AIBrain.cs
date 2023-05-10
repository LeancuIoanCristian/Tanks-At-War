using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBrain : MonoBehaviour
{
    [SerializeField] private NavMeshAgent ai_mesh_agent;
    [SerializeField] private GameObject player_reference;
    [SerializeField] private float ai_view_distance;
    [SerializeField] private Tank ai_tank;
    [SerializeField] private float reload_time;

    private void Start()
    {
      
    }
    private void Update()
    {
        RaycastHit obj_hit;
        if (Vector3.Distance(this.transform.position, player_reference.transform.position) < ai_view_distance)
        {
            if ((Physics.Raycast(transform.position, transform.forward, out obj_hit, 300f, 7)))
            {
                Debug.LogWarning("Player seen");
                ai_tank.GetTurret().GetGun().GiveDamage();
            }
            Pursue();
        }
        
    }

    private void Pursue()
    {
        ai_mesh_agent.destination = player_reference.transform.position;
    }
    private void Attack()
    {
      
    }
}
