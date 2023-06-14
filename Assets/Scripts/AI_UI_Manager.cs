using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AI_UI_Manager : MonoBehaviour
{
    [SerializeField] private Tank ai_tank_reference;
    [SerializeField] private int max_health;
    [SerializeField] private Slider health_bar_ui;
    [SerializeField] private TextMeshProUGUI health_UI;
    [SerializeField] private Tank player_reference;
    RaycastHit obj_hit;
    // Start is called before the first frame update
    void Start()
    {
        max_health = ai_tank_reference.GetHull().GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        ShowEnemyHealth();
    }

    private void ShowEnemyHealth()
    {
        Physics.Raycast(player_reference.GetTurret().GetGun().GetBarrelEnd().transform.position, player_reference.GetTurret().GetGun().GetBarrelEnd().transform.forward, out obj_hit, 100f);
        if (obj_hit.transform == null)
            return;
        if (obj_hit.transform.GetComponentInParent<AI_UI_Manager>() == this)
        {
            health_bar_ui.gameObject.SetActive(true);
            health_UI.gameObject.SetActive(true);
            health_UI.text = ai_tank_reference.GetHull().GetHealth().ToString() + "/" + max_health.ToString();
            health_bar_ui.value = (float)ai_tank_reference.GetHull().GetHealth() / max_health;
        }
        else
        {
            health_bar_ui.gameObject.SetActive(false);
            health_UI.gameObject.SetActive(false);
        }
    }
}
