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
    private float x_axis_rotation = 0.0f;

   
    private void Update()
    {
        TurnAction();

    }

    private void TurnAction()
    {
        Vector3 direction = player_reference.transform.position - transform.position;
        float y_angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        if (Vector3.Distance(this.transform.position, player_reference.transform.position) < ai_view_distance)
        {
            RotateTurret(y_angle);

            AngleGun(y_angle);
            ShootAtPlayerInRange();
            Pursue();
        }
    }

    private void ShootAtPlayerInRange()
    {
        RaycastHit obj_hit;
        var barrel = this.GetComponentInParent<Tank>().GetTurret().GetGun().GetBarrelEnd();
        if ((Physics.Raycast(barrel.transform.position, barrel.transform.forward, out obj_hit, 50f)))
        {
            if (obj_hit.transform.CompareTag("Player") || obj_hit.transform.CompareTag("tank"))
            {
                ai_tank.GetTurret().GetGun().GiveDamage();
            }

        }
    }

    private void AngleGun(float y_angle)
    {
        Quaternion target_gun_level;
        x_axis_rotation = transform.position.y - player_reference.transform.position.y;
        x_axis_rotation = Mathf.Clamp(x_axis_rotation, ai_tank.GetTurret().GetGun().GetGunDownConstrain(), ai_tank.GetTurret().GetGun().GetGunUpConstrain());



        target_gun_level = Quaternion.Euler(x_axis_rotation, y_angle, 0f);
        ai_tank.GetTurret().GetGun().transform.rotation = Quaternion.Slerp(ai_tank.GetTurret().GetGun().transform.rotation, target_gun_level, Time.deltaTime);
    }

    private void RotateTurret(float y_angle)
    {
        Quaternion target_rotation = Quaternion.Euler(0f, y_angle, 0f);
        ai_tank.GetTurret().transform.rotation = Quaternion.Slerp(ai_tank.GetTurret().transform.rotation, target_rotation, Time.deltaTime);
    }

    private void Pursue()
    {
        ai_mesh_agent.destination = player_reference.transform.position;
    }
    private void Attack()
    {
      
    }
}
