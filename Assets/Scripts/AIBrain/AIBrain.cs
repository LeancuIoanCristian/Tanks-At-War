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
            Vector3 direction = player_reference.transform.position - transform.position;
            float y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion target_rotation = Quaternion.Euler(0f, y_angle, 0f);
            //float temp_angle = Vector3.Angle(this.ai_tank.transform.position, player_reference.transform.position);

            ai_tank.GetTurret().transform.rotation = Quaternion.Slerp(ai_tank.GetTurret().transform.rotation, target_rotation,Time.deltaTime);
            var barrel = this.GetComponentInParent<Tank>().GetTurret().GetGun().GetBarrelEnd();
            if ((Physics.Raycast(barrel.transform.position, barrel.transform.forward, out obj_hit, 50f)))
            {
                if (obj_hit.transform.CompareTag("Player") || obj_hit.transform.CompareTag("tank"))
                {
                    Debug.LogWarning("Player seen");
                    ai_tank.GetTurret().GetGun().GiveDamage();
                }
               
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
