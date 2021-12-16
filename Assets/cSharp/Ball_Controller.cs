using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Controller : MonoBehaviour
{
    public AI_Data_Su m_Data;

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0 || v != 0)
        {
            //��V�ؼФ��V�q = ��i�V�q
            Vector3 target = transform.position + new Vector3(h, 0, v);
            //����k�S���C�C��V�A����ɤO
            transform.LookAt(target);
            PlayerMove(h, v);
        }
    }

    void PlayerMove(float h, float v)
    {
        transform.Translate(Vector3.forward * m_Data.m_Speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * this.m_Data.m_fProbeLength);

    }



}
