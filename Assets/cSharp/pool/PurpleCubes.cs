using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCubes : MonoBehaviour
{
    //checkpoint的計數器
    //List用來裝checkpoint
    int PointNum = 0;
    List<string> CheckPoints;
    Transform target;

    [SerializeField]
    float speed = 10f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void MoveTo(Vector3 destination)
    {
        //Vector3.Distance() 求長度
        //設定死區以防呆
        if (Vector3.Distance(transform.position, destination) < 0.1f) return;
        //調整物件朝向，一種有Lerp一種沒有
        Vector3.Lerp(transform.forward, (destination - transform.position).normalized, Time.deltaTime);
        transform.forward = (destination - transform.position).normalized;

        transform.position += transform.forward * speed * Time.deltaTime;
    }



    //被collier碰撞到目標的時候
    private void OnTriggerEnter(Collider other)
    {
        //只在這裡運作的function
        void pathRun(List<string> checks, int key)
        {
            if (key > (checks.Count - 1))
            {
                return;
            }
            else
            {
                if (other.name == checks[key])
                {
                    if (key + 1 > (checks.Count - 1))
                    {
                        target = GameObject.Find("Sphere").transform;
                        return;
                    }
                    target = GameObject.Find(checks[key + 1]).transform;
                }
                pathRun(CheckPoints, key + 1);
            }
        }
        pathRun(CheckPoints, 0);
        if (other.name == "Sphere")
        {
            if (CheckPoints.Count > 0)
            {
                target = GameObject.Find(CheckPoints[0]).transform;
            }
            else
            {
                target = GameObject.Find("Sphere").transform;
            }
        }
    }








    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 3);

    }
}
