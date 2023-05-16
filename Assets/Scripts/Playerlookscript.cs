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

    [SerializeField] private float mouse_sensitivity = 10.0f;
    [SerializeField] private Turret tank_turret;
    private float x_axis_rotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        sniper_view.gameObject.SetActive(false);
        tactical_view.gameObject.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sniper_view.fieldOfView = 25.0f;
        
    }

    // Update is called once per frame
    void Update()
    { 
        float mouse_x = Input.GetAxis("Mouse X") * mouse_sensitivity * Time.deltaTime;
        float mouse_y = Input.GetAxis("Mouse Y") * mouse_sensitivity * Time.deltaTime;

        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SniperViewToggle();
        }
        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            CameraSetCloser();
           
        }
        else if(Input.GetKeyDown(KeyCode.Mouse4))
        {
            CameraSetAway();
        }

        x_axis_rotation -= mouse_y;
        x_axis_rotation = Mathf.Clamp(x_axis_rotation, tank_turret.GetGun().GetGunDownConstrain(), tank_turret.GetGun().GetGunUpConstrain());
        tank_turret.GetGun().transform.localRotation = Quaternion.Euler(x_axis_rotation, 0.0f, 0.0f);
        tank_turret.transform.Rotate(Vector3.up * mouse_x);
        sniper_view.transform.localRotation = Quaternion.Euler(x_axis_rotation, 0.0f, 0.0f);
        //look_at_object.transform.position = new Vector3( look_at_object.transform.position.x, -x_axis_rotation, look_at_object.transform.position.z);
        cinemachine.m_XAxis.Value = mouse_x;
        



    }

    private Camera SniperViewToggle()
    {
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



