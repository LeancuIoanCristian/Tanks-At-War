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
    // Start is called before the first frame update
    void Start()
    {
        max_health = ai_tank_reference.GetHull().GetHealth();
    }

    // Update is called once per frame
    void Update()
    {
        health_UI.text = ai_tank_reference.GetHull().GetHealth().ToString() + "/" + max_health.ToString();
        health_bar_ui.value = (float)ai_tank_reference.GetHull().GetHealth() / max_health;
    }
}
