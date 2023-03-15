using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour, IDestroyable
{
    bool touched = false;
    bool on_ground = false;
    GameObject prefab;
    float collision_direction;

    // Update is called once per frame
    void Update()
    {
        if(touched)
        {
            //prefab.GetComponent<SpriteRenderer>.color
            if (!on_ground)
            {
                prefab.transform.rotation = Quaternion.Lerp(prefab.transform.rotation, Quaternion.Euler(90, 0, 0 - collision_direction), 4);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        touched = true;
        collision_direction = Quaternion.Angle(other.transform.rotation, prefab.transform.rotation);
    }

    public void DestroyObject()
    {

    }
}
