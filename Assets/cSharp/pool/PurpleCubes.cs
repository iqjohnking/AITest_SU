using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCubes : MonoBehaviour
{
    //checkpoint���p�ƾ�
    //List�ΨӸ�checkpoint
    int checkPointNum = 0;
    List<string> CheckPoints;

    [SerializeField]
    float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 3);

    }
}
