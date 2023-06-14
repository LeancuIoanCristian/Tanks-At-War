using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManagerPlayer : MonoBehaviour
{
    [SerializeField] private Tank player_tank_reference;
    [SerializeField] private GameObject reload_UI_object;
    [SerializeField] private Image reload_circle;
    [SerializeField] private int max_health;
    [SerializeField] private Slider health_bar_ui;
    [SerializeField] private TextMeshProUGUI health_UI;

    // Start is called before the first frame update
    void Start()
    {
        max_health = player_tank_reference.GetHull().GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        ShowPlayerUI();

    }

    private void ShowPlayerUI()
    {
        health_UI.text = player_tank_reference.GetHull().GetHealth().ToString() + "/" + max_health.ToString();
        health_bar_ui.value = (float)player_tank_reference.GetHull().GetHealth() / max_health;
        reload_circle.fillAmount = player_tank_reference.GetTurret().GetGun().GetReload() / 3f;
        if (!player_tank_reference.GetTurret().GetGun().CanShoot())
        {
            reload_UI_object.SetActive(true);

        }
        else
        {
            reload_UI_object.SetActive(false);

        }
    }
}
