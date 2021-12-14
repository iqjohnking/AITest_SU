using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Button : MonoBehaviour
{
    public void toseek(string name)
        {
            SceneManager.LoadScene(name);
        }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        return;
    }
}

