using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levels_Manager_scriptableObject : ScriptableObject
{
    [SerializeField] private List<Scene> levels_list;
    [SerializeField] private int level_index = 0;

    public void NextLevel()
    {
        level_index++;
        SceneManager.LoadScene(levels_list[level_index].name);
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levels_list[level_index].name);
    }
}
