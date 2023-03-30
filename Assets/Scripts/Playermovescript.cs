using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovescript : MonoBehaviour
{

    //Character controller Variables
    [SerializeField] private CharacterController controller;
    private Vector3 player_velocity;
    private bool grounded_player;
    private float player_speed = 2.0f;
    private float gravity_value = -9.81f;
    private float turn_smoother = 0.1f;
    private float smooth_velocity;

    [SerializeField] private Transform main_tactical_camera;
    [SerializeField] private Tank tank;
    [SerializeField] private Playerlookscript look_script;

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smooth_velocity, turn_smoother);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            Vector3 move_direction = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            controller.Move(move_direction.normalized * player_speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            tank.GetTurret().GetGun().GiveDamage(tank.GetTurret().GetGun().GetCurrentAmmo().GetDamage(), look_script.ActiveCamera());
        }
    }
}
