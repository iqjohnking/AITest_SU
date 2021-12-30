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

    public Vector3 nextSpawnPoint;





    void Start()
    {
        CheckPoints = new List<string>();
        var CheckSet = FindObjectsOfType<CheckPoint>();

        while (CheckPoints.Count < CheckSet.Length)
        {
            for (int i = 0; i < CheckSet.Length; i++)
            {
                if (PointNum == CheckSet[i].GetNum)
                {
                    CheckPoints.Add(CheckSet[i].gameObject.name);
                }
            }
            PointNum++;
        }

        if (CheckSet.Length > 0)
        {
            target = GameObject.Find("CheckPoint1").transform;
        }
        else
        {
            target = GameObject.Find("End").transform;
        }
    }
    void Update()
    {
        MoveTo(target.position);
    }



    void MoveTo(Vector3 destination)
    {
        //Vector3.Distance() 求長度
        //設定死區以防呆
        if (Vector3.Distance(transform.position, destination) < 0.1f) return;
        transform.forward = (destination - transform.position).normalized;

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    
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
                        target = GameObject.Find("End").transform;
                        return;
                    }
                    target = GameObject.Find(checks[key + 1]).transform;
                }
                pathRun(CheckPoints, key + 1);
            }
        }
        pathRun(CheckPoints, 0);
        
        
        if (other.name == "End")
        {
            if (CheckPoints.Count > 0)
            {
                target = GameObject.Find(CheckPoints[0]).transform;
            }
            else
            {
                target = GameObject.Find("End").transform;
            }
            
        }

        if (other.tag == "unloadpoint")
        {
            var loader = FindObjectOfType<SpawnPoint>();
            loader.Unload(this.gameObject);
            this.gameObject.transform.position = nextSpawnPoint;
        }

        Debug.Log(target);
    }








    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * 3);

    }
}
