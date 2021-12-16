using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Button : MonoBehaviour
{
    //避免回到開頭無限重生ui系統
    //此為singleton寫法之一，要再研究各種寫法差別。
    public static Scene_Button Instance { get; set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
    }

}

