using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main m_Instance;

    private List<Obstacle> m_Obstacles;
    public GameObject m_Player;

    private void Awake()
    {
        m_Instance = this;
    }

    // ��Start�ӹ����(Initialization)����
    void Start()
    {
        m_Obstacles = new List<Obstacle>();
        GameObject[] gObjectS = GameObject.FindGameObjectsWithTag("Obstacle");
        //�p�G�o�}�C��������A�h��foreach���C�Ӫ���M�W
        if (gObjectS != null || gObjectS.Length > 0)
        {
            Debug.Log(gObjectS.Length);
            foreach (GameObject gObject in gObjectS)
            {
                m_Obstacles.Add(gObject.GetComponent<Obstacle>());
            }
        }
    }

    public GameObject GetPlayer()
    {
        return m_Player;
    }

    public List<Obstacle> GetObstacles()
    {
        return m_Obstacles;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
