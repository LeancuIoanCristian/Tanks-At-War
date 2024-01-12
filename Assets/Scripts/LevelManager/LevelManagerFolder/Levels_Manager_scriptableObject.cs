using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings")]
public class Levels_Manager_scriptableObject : ScriptableObject
{
    [SerializeField] private int level_index = 0;
    [SerializeField] private int ai_number_for_level;
    [SerializeField] private int platform_number_for_level;
    [SerializeField] private float x_scale = 1000f;
    [SerializeField]  private float z_scale = 1000f;


    public int GetAINumber() => ai_number_for_level;
    public int GetPlatformNumber() => platform_number_for_level;
    public float GetXScale() => x_scale;
    public float GetZScale() => z_scale;

    public void SetXScale(float value) => x_scale = value;
    public void SetZScale(float value) => z_scale = value;

    public void SetAINumber(int value) => ai_number_for_level = value;
    public void SetPlatformNumber(int value) => platform_number_for_level = value;
   

    public void NextLevel()
    {
        level_index = 0;
        LoadLevel();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(level_index);
    }
}
