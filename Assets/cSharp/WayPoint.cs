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
    /// if(���b�A�T�w��m_Neibors�b����)
    /// foreach(�C�@�Ӧbm_Neibors����go)
    /// Gizmo�C�⬰��
    /// �eGizmo�u�A�Hthis���_�I�Ago�����I�C
    /// </summary>
    private void OnDrawGizmos()
    {
        if(m_Neibors != null && m_Neibors.Count > 0)
        {
            foreach(GameObject go in m_Neibors)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(this.transform.position, go.transform.position);
            }
        }
    }
}
