using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Button : MonoBehaviour
{
    //�קK�^��}�Y�L������ui�t��
    //����singleton�g�k���@�A�n�A��s�U�ؼg�k�t�O�C
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

