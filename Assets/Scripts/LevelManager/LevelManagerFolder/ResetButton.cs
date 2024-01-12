using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    [SerializeField] private TankUpgradesScriptableObjectScipt upgrades_references;
    [SerializeField] private Levels_Manager_scriptableObject objects_reference;
    [SerializeField] private Slider ai_slider_reference;
    [SerializeField] private Slider platform_slider_reference;
    [SerializeField] private Slider x_scale_slider_reference;
    [SerializeField] private Slider z_scale_slider_reference;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject main_menu;

    [SerializeField] private const int min_slider_value = 1;
    [SerializeField] private const int max_slider_value = 1000;
    [SerializeField] private bool settings_menu = false;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        CheckChanges();
    }

    public void SetUp()
    {
        ai_slider_reference.minValue = min_slider_value;
        ai_slider_reference.maxValue = max_slider_value;
        platform_slider_reference.minValue = min_slider_value;
        platform_slider_reference.maxValue = max_slider_value;
        x_scale_slider_reference.minValue = min_slider_value * 50f;
        x_scale_slider_reference.maxValue = max_slider_value;
        z_scale_slider_reference.minValue = min_slider_value * 90f;
        z_scale_slider_reference.maxValue = max_slider_value;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ai_slider_reference.value = objects_reference.GetAINumber();
        platform_slider_reference.value = objects_reference.GetPlatformNumber();
        x_scale_slider_reference.value = objects_reference.GetXScale();
        z_scale_slider_reference.value = objects_reference.GetZScale();

    }

    public void OnResetPressed()
    {
        upgrades_references.ResetValues();
    }

    private void CheckChanges()
    {
        objects_reference.SetXScale(x_scale_slider_reference.value);
        objects_reference.SetZScale(z_scale_slider_reference.value);
        objects_reference.SetAINumber((int)ai_slider_reference.value);
        objects_reference.SetPlatformNumber((int)platform_slider_reference.value);
        ScaleCheck();
    }

    private void ToogleSettings()
    {
        if (settings_menu)
        {
            settings_menu = false;
        }
        else
        {
            settings_menu = true;
        }
    }

    public void SettingsOnOff()
    {
        if (settings_menu)
        {
            settings.SetActive(false);
            main_menu.SetActive(true);
        }
        else
        {
            settings.SetActive(true);
            main_menu.SetActive(false);
        }

        ToogleSettings();
    }

    private void ScaleCheck()
    {
        if (ai_slider_reference.value > x_scale_slider_reference.value + z_scale_slider_reference.value)
        {
            ai_slider_reference.value = Mathf.Min(x_scale_slider_reference.value, z_scale_slider_reference.value);
        }
        if (platform_slider_reference.value > Mathf.Min(x_scale_slider_reference.value, z_scale_slider_reference.value))
        {
            platform_slider_reference.value = Mathf.Min(x_scale_slider_reference.value, z_scale_slider_reference.value);
        }
    }

    public void OnExitPressed()
    {
        Application.Quit();
    }
}
