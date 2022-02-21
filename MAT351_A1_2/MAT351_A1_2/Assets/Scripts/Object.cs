using System;
using UnityEngine;

public class Object : MonoBehaviour
{
    [SerializeField]
    TextAsset Config;

    Quaternion mInitial;
    Quaternion mFinal;
    UInt32 N;

    void Start()
    {
        var textArray = Config.text.Split('\n');
        var info1 = textArray[0].Split(' ');
        var info2 = textArray[1].Split(' ');
        var info3 = textArray[2];

        mInitial = 
            new Quaternion(
                (float)Convert.ToDouble(info1[0]),
                (float)Convert.ToDouble(info1[1]),
                (float)Convert.ToDouble(info1[2]),
                (float)(Convert.ToDouble(info1[3]) * Math.PI) / 180f
            );

        mFinal = 
            new Quaternion(
                (float)Convert.ToDouble(info2[0]),
                (float)Convert.ToDouble(info2[1]),
                (float)Convert.ToDouble(info2[2]),
                (float)(Convert.ToDouble(info2[3]) * Math.PI) / 180f
            );

        N = Convert.ToUInt32(info3);
        
        Debug.Log("Initial: " + mInitial);
        Debug.Log("Final: " + mFinal);
        Debug.Log("N : " + N);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
