using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCubes : MonoBehaviour
{
    //checkpoint的計數器
    //List用來裝checkpoint
    int checkPointNum = 0;
    List<string> CheckPoints;

    [SerializeField]
    float speed = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 3);

    }
}
