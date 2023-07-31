using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;


public class Playerlookscript : MonoBehaviour//, Cinemachine.AxisState.IInputAxisProvider
{
    //Cameras
    [SerializeField] private CinemachineFreeLook cinemachine;
    [SerializeField] private Camera tactical_view;
    [SerializeField] private Camera sniper_view;
    [SerializeField] private GameObject look_at_object;
    
    private bool sniper_view_on = false;
    public bool GetViewState() => sniper_view_on;
    public GameObject crosshair_sniper;
    [SerializeField] private float default_sniper_fov = 25f;
    [SerializeField] private float mouse_sensitivity = 10.0f;
    [SerializeField] private Turret tank_turret;
    private float x_axis_rotation = 0.0f;

    private float screen_height = Screen.height / 2.0f;
    private float screen_width = Screen.width / 2.0f;

    [SerializeField] GameState game_state_ref;

    // Start is called before the first frame update
    void Start()
    {
        SetUPCameras();

    }

    private void SetUPCameras()
    {
        sniper_view.gameObject.SetActive(false);
        tactical_view.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sniper_view.fieldOfView = default_sniper_fov;

        screen_height = sniper_view.scaledPixelHeight / 2.0f;
        screen_width = sniper_view.scaledPixelWidth / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        TurnActions();

    }

    private void TurnActions()
    {
        if (game_state_ref.GetGameState())
        {
            float mouse_x = Input.GetAxis("Mouse X") * mouse_sensitivity * Time.deltaTime;
            float mouse_y = Input.GetAxis("Mouse Y") * mouse_sensitivity * Time.deltaTime;
            SetCamera();

            RotateCamera(mouse_x, mouse_y);


            Shoot();
        }
        
    }


    //To Do: Move Method to Own Class
    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (GetViewState())
            {
                tank_turret.GetGun().GiveDamagePulse(sniper_view.transform);
            }
            else
            {
                tank_turret.GetGun().GiveDamage();
            }

        }
    }

    private void RotateCamera(float mouse_x, float mouse_y)
    {
        cinemachine.m_XAxis.Value = Mathf.Clamp(mouse_x, -180, 180);

        if (!sniper_view_on)
        {
            Vector3 looking_direction = tactical_view.transform.forward;
            looking_direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(looking_direction);
            tank_turret.transform.rotation = rotation;
        }
        else
        {
            tank_turret.transform.Rotate(Vector3.up * mouse_x);
        }


        x_axis_rotation -= mouse_y;
        x_axis_rotation = Mathf.Clamp(x_axis_rotation, tank_turret.GetGun().GetGunDownConstrain(), tank_turret.GetGun().GetGunUpConstrain());
        tank_turret.GetGun().transform.localRotation = Quaternion.Euler(x_axis_rotation, 0.0f, 0.0f);

        sniper_view.transform.localRotation = Quaternion.Euler(x_axis_rotation, 0.0f, 0.0f);
        //look_at_object.transform.position = new Vector3( look_at_object.transform.position.x, -x_axis_rotation, look_at_object.transform.position.z);
    }

    private void SetCamera()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SniperViewToggle();
        }
        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            CameraSetCloser();

        }
        else if (Input.GetKeyDown(KeyCode.Mouse4))
        {
            CameraSetAway();
        }
    }

    private Camera SniperViewToggle()
    {
        GetComponentInParent<Playermovescript>().SetSameText(GetComponentInParent<Playermovescript>().GetActiveText().text);

        if (sniper_view_on)
        {
            sniper_view.gameObject.SetActive(false);
            tactical_view.gameObject.SetActive(true);
            sniper_view_on = false;
            return tactical_view;
        }
        else
        {
            sniper_view.gameObject.SetActive(true);
            tactical_view.gameObject.SetActive(false);
            sniper_view_on = true;
            return sniper_view;
        }

        
    }

    /// <summary>
    /// Sniper view zoom in
    /// </summary>
    private void CameraSetCloser()
    {
        if (sniper_view_on && sniper_view.fieldOfView >= 25.0f)
        {
            sniper_view.fieldOfView -= 20.0f;
        }

    }


    /// <summary>
    /// Sniper view zoom out
    /// </summary>
    private void CameraSetAway()
    {
        if (sniper_view_on && sniper_view.fieldOfView <= 45.0f)
        {
            sniper_view.fieldOfView += 20.0f;
        }
    }

    public Camera ActiveCamera()
    {
        if (!sniper_view_on)
        {
            return tactical_view;
        }
        else
        {
            return sniper_view;
        }
    }

    public void CameraMove(Vector3 direction)
    {
        tactical_view.transform.Rotate(direction);
        sniper_view.transform.Rotate(direction);
    }


}



