using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveWayPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("WayPoint");
        StreamWriter sw = new StreamWriter("Assets/waypoint.txt" , false);
        //false means "Overwrite the file " 
        Debug.Log(sw);
        string s = "";
        for(int i = 0 ; i < gos.Length ;  i++)
        {
            s = "";
            s += gos[i].name;
            s += " ";
            WayPoint wp = gos[i].GetComponent<WayPoint>();
            s += wp.intFloor;
            s += " ";
            s += wp.boolLink;
            s += " ";
            s += wp.m_Neibors.Count;
            s += " ";
            for (int j = 0 ; j < wp.m_Neibors.Count; j++ )
            {
                s += wp.m_Neibors[j].name;
                s += " ";
            }
            sw.WriteLine(s);
        }
        sw.Close();
    }
}
