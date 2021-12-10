using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public List<GameObject> m_Neibors;
    public bool boolLink = false;
    public int intFloor = 0;

    void Start()
    {
    }
    void Update()
    {
    }

    /// <summary>
    /// if(防呆，確定有m_Neibors在旁邊)
    /// foreach(每一個在m_Neibors中的go)
    /// Gizmo顏色為藍
    /// 畫Gizmo線，以this為起點，go為終點。
    /// </summary>
    private void OnDrawGizmos()
    {
        if (m_Neibors != null && m_Neibors.Count > 0)
        {
            foreach (GameObject go in m_Neibors)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(this.transform.position, go.transform.position);
            }
        }
    }
}