using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//System.Linq再研究
using System.Linq;

public class SpawnPoint : MonoBehaviour
{
    //創建list資源池
    List<ObjectPool> objectPool;

    //資源池大小
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
            //Resources.Load()預設為 UnityEngine.Object
            //Resources.Load<GameObject>讀取物件為GameObject
            GameObject rLoadgObject = Resources.Load<GameObject>("PurpleCubes") ;
            //隱藏物件
            rLoadgObject.SetActive(false);
            //實體化讀進來的東西
            GameObject gObject = Instantiate(rLoadgObject);
            //將物件add進去list的末端，但是這個位置暫時沒人坐
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

    //判斷是否使用中
    public bool isUsing;

}
