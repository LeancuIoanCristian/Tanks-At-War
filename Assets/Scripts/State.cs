using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "State", menuName = "States")]
class State : ScriptableObject
{

    [SerializeField] private bool playing = true;
    private static State instance;

    public static State Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new State();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public bool GetState() => playing;

    public void ToogleState()
    {
        if (playing)
        {
            playing = false;
        }
        else
        {
            playing = true;
        }
    }
}

