using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    //"=>"Lambda �B��l
    string Name => gameObject.name;
    string getNum;

    int num;
    public int GetNum => num;
    //�᭱num�Q��J

    void Start()
    {
        for (int i = 10; i < Name.Length; i++)
        {
            //(string getNum) = (string getNum) + (string Name[i])
            getNum += Name[i];
        }
        //�Nstring getNum �૬�A��Jnum
        num = int.Parse(getNum);
    }
}
