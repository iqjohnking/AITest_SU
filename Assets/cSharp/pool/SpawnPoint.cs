using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//System.Linq�A��s
using System.Linq;

public class SpawnPoint : MonoBehaviour
{
    //�Ы�list�귽��
    List<ObjectPool> objectPool;

    //�귽���j�p
    [SerializeField]
    int howManySoldiers = 10;

    void Start()
    {
        PrepareObject(howManySoldiers);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Load();
        }
    }

    public void PrepareObject(int howManySoldiers)
    {
        this.howManySoldiers = howManySoldiers;
        objectPool = new List<ObjectPool>();
        for (int i = 0; i < howManySoldiers; i++)
        {
            //Resources.Load()�w�]�� UnityEngine.Object
            //Resources.Load<GameObject>Ū������GameObject
            GameObject rLoadgObject = Resources.Load<GameObject>("PurpleCubes") ;
            //���ê���
            rLoadgObject.SetActive(false);
            //�����Ū�i�Ӫ��F��
            GameObject gObject = Instantiate(rLoadgObject);
            //�N����add�i�hlist�����ݡA���O�o�Ӧ�m�ȮɨS�H��
            objectPool.Add( new ObjectPool { gameobject = gObject , isUsing=false});
        }
    }
    public void Load()
    {
        var OPLength = objectPool.Where(o => o.isUsing).Count();
        if (OPLength == howManySoldiers)
        {
            return;
        }
        var target = objectPool.First(o => o.isUsing == false);
        if (target == null) return;
        target.isUsing = true;
        target.gameobject.SetActive(true);
    }
    public void Unload(GameObject removeTG)
    {
        var OPLength = objectPool.Where(o => o.isUsing == false).Count();
        if (OPLength == howManySoldiers)
        {
            return;
        }
        var target = objectPool.First(o => o.gameobject == removeTG);
        if (target == null) return;
        target.isUsing = false;
        target.gameobject.SetActive(false);
    }




}

public class ObjectPool
{
    public GameObject gameobject;

    //�P�_�O�_�ϥΤ�
    public bool isUsing;

}
