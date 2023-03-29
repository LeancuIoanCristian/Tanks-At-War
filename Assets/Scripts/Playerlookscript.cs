using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Playerlookscript : MonoBehaviour, Cinemachine.AxisState.IInputAxisProvider
{
    //Cameras
    [SerializeField] private Camera tactical_view;
    [SerializeField] private Camera sniper_view;
    [SerializeField] public Camera current_camera;
    private bool sniper_view_on = false;
    private int tactical_scale = 1;



    // Start is called before the first frame update
    void Start()
    {
        sniper_view.enabled = false;
        tactical_view.enabled = true;
        current_camera = tactical_view;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SniperViewToggle();
        }
        if (Input.mouseScrollDelta.y > 0.0f)
        {
            CameraSetCloser();
            Debug.Log(Input.mouseScrollDelta);
        }
        else
        {
            CameraSetAway();
        }

        float mouse_x = Input.GetAxis("Mouse X");
        float mouse_y = Input.GetAxis("Mouse Y");
    }

    private Camera SniperViewToggle()
    {
        if (sniper_view_on)
        {
            sniper_view.enabled = false;
            tactical_view.enabled = true;
            sniper_view_on = false;
            return tactical_view;
        }
        else
        {
            sniper_view.enabled = true;
            tactical_view.enabled = false;
            sniper_view_on = true;
            return sniper_view;
        }
    }

    private void CameraSetCloser()
    {
        if (sniper_view_on && sniper_view.fieldOfView >= 10.0f)
        {
            sniper_view.fieldOfView -= 20.0f;
        }

    }

    private void CameraSetAway()
    {
        if (sniper_view_on && sniper_view.fieldOfView <= 60.0f)
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
}
