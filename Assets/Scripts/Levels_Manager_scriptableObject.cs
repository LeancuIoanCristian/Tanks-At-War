using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels_Manager_scriptableObject : ScriptableObject
{
    [SerializeField] private int level_index = 0;

    public void NextLevel()
    {
        level_index++;
        LoadLevel();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(level_index);
    }
}
