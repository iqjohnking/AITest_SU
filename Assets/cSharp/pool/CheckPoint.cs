using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //"=>"Lambda 運算子
    string Name => gameObject.name;
    string getNum;

    int num;
    public int GetNum => num;
    //後面num被放入

    void Start()
    {
        for (int i = 10; i < Name.Length; i++)
        {
            //(string getNum) = (string getNum) + (string Name[i])
            getNum += Name[i];
        }
        //將string getNum 轉型，放入num
        num = int.Parse(getNum);
    }
}
