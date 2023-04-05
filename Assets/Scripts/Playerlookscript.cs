using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Playerlookscript : MonoBehaviour, Cinemachine.AxisState.IInputAxisProvider
{
    //Cameras
    [SerializeField] private Camera tactical_view;
    [SerializeField] private Camera sniper_view;
 
    private bool sniper_view_on = false;
    private int tactical_scale = 1;

    [SerializeField] private float mouse_sensitivity = 10.0f;
    [SerializeField] private Turret tank_turret;
    private float x_axis_rotation = 0.0f;
    private float x_axis_rotation_speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        sniper_view.gameObject.SetActive(false);
        tactical_view.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
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
        CameraMove(Vector3.up * mouse_x);
        tank_turret.transform.Rotate(Vector3.up * mouse_x);




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

    private void CameraSetCloser()
    {
        if (sniper_view_on && sniper_view.fieldOfView >= 20.0f)
        {
            sniper_view.fieldOfView -= 20.0f;
        }

    }

    private void CameraSetAway()
    {
        if (sniper_view_on && sniper_view.fieldOfView <= 40.0f)
        {
            sniper_view.fieldOfView += 20.0f;
        }
    }

    public float GetAxisValue(int index)
    {        
        return CameraSet(tactical_scale);
    }

    private float CameraSet(float input)
    {
        if (Input.mouseScrollDelta.y > 0.0f)
        {
            if (input < 2)
            {
                input++;
                return 30.0f;
            }
            else
            {
                input++;
                return 0.0f;
            }
           
        }
        else
        {
            if (input > 1)
            {
                input--;
                return 0.0f;
            }
            else
            {
                return -5.0f;
            }
            
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



//tank_turret.GetGun().transform.Rotate(Vector3.right * x_axis_rotation); for self shooting

