using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovescript : MonoBehaviour
{

    //Character controller Variables
    [SerializeField] private CharacterController controller;
    private float player_speed = 6.0f;
    private float gravity_value = -9.81f;

    [SerializeField] private Transform ground_checker;
    [SerializeField] private float ground_distance = 0.8f;
    [SerializeField] private LayerMask ground_mask;
    private bool grounded_player;
    private Vector3 velocity;

    [SerializeField] private Tank tank;
   

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTurnAction();
    }

    private void PlayerTurnAction()
    {
        grounded_player = Physics.CheckSphere(ground_checker.position, ground_distance, ground_mask);

        if (grounded_player && velocity.y < 0.0f)
        {
            velocity.y = -0.5f;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0.0f)
        {
            tank.transform.Rotate(0.0f, tank.GetHull().GetTracks().GetTurningSpeed() * Mathf.Sign(horizontal) * Time.deltaTime, 0.0f);

        }

        velocity.y += gravity_value;


        controller.Move(tank.transform.forward * vertical * player_speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }
}


