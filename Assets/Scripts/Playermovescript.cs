using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Playermovescript : MonoBehaviour
{

    //Character controller Variables
    [SerializeField] private CharacterController controller;
    private float player_speed = 6.0f;
    private float gravity_value = -9.81f;

    Level_Manager level_manager;
    [SerializeField] private Transform ground_checker;
    [SerializeField] private float ground_distance = 0.8f;
    [SerializeField] private LayerMask ground_mask;
    private bool grounded_player;
    private Vector3 velocity;

    [SerializeField] private Tank tank;
    [SerializeField] private TextMeshProUGUI text_reference_tactical;
    [SerializeField] private TextMeshProUGUI text_reference_sniper;

    [SerializeField] GameState game_state_ref;

    public TextMeshProUGUI GetTextReference() => GetActiveText();

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        text_reference_sniper.text = "";
        text_reference_tactical.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTurnAction();
    }

    private void PlayerTurnAction()
    {
        if (game_state_ref.GetGameState())
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FinishPoint")
        {
            level_manager.LoadNextLevel();
        }
        if (other.gameObject.tag == "Platform")
        {
            GetActiveText().text = other.GetComponentInChildren<MiniShopFunctionality>().ShowText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GetActiveText().text = "";
        if (other.GetComponentInChildren<MiniShopFunctionality>().IsUpgradeDone())
        {
            other.gameObject.SetActive(false);
        }
        
    }

    public TextMeshProUGUI GetActiveText()
    {
        if (text_reference_tactical.IsActive())
        {
            return text_reference_tactical;
        }
        else
        {
            return text_reference_sniper;
        }
    }

    public void SetSameText(string value)
    {
        text_reference_sniper.text = value;
        text_reference_tactical.text = value;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Platform")
        {
            if (Input.GetKeyDown(KeyCode.E) && !other.GetComponent<MiniShopFunctionality>().IsUpgradeDone())
            {
                other.GetComponent<MiniShopFunctionality>().SetUpgradeDone(true);
                other.GetComponent<MiniShopFunctionality>().MakeUpgrade(other.GetComponent<MiniShopFunctionality>().GetMinishopUpgrade());
                SetSameText("");
            }
        }
        
    }

    

}


