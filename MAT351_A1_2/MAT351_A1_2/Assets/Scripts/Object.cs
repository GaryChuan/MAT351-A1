using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

public class Object : MonoBehaviour
{
    [SerializeField]
    TextAsset InputFile;

    [SerializeField]
    TextAsset OutputFile;

    public Slider SliderControl;

    Quaternion mInitial;
    Quaternion mFinal;
    UInt32 mMaxCount;
    UInt32 mCount = 0u;
    Dictionary<UInt32, Quaternion> mOrientations;

    void Start()
    {
        var textArray = InputFile.text.Split('\n');
        var info1 = textArray[0].Split(' ');
        var info2 = textArray[1].Split(' ');
        var info3 = textArray[2];

        mInitial = 
            Axis4DToQuaternions(
                (float)Convert.ToDouble(info1[0]),
                (float)Convert.ToDouble(info1[1]),
                (float)Convert.ToDouble(info1[2]),
                (float)(Convert.ToDouble(info1[3]) * Math.PI) / 180f
            );

        mFinal = 
            Axis4DToQuaternions(
                (float)Convert.ToDouble(info2[0]),
                (float)Convert.ToDouble(info2[1]),
                (float)Convert.ToDouble(info2[2]),
                (float)(Convert.ToDouble(info2[3]) * Math.PI) / 180f
            );

        mMaxCount = Convert.ToUInt32(info3);

        Debug.Log("Initial: "  + QuaternionToString(mInitial));
        Debug.Log("Final: " + QuaternionToString(mFinal));
        Debug.Log("N : " + mMaxCount);
        
        mOrientations =  new Dictionary<uint, Quaternion>();
        SliderControl.maxValue = mMaxCount;

        InitializeOrientations();
    }

    string QuaternionToString(Quaternion q)
    {
        return q.x + ", " + q.y + ", " + q.z + ", " + (q.w * 180 / Math.PI);
    }

    Quaternion Axis4DToQuaternions(float x, float y, float z, float angle)
    {
        float s = (float)Math.Sin(angle / 2);
        Vector3 axis = new Vector3(x, y, z);

        axis.Normalize();
        axis *= s;

        return new Quaternion(axis.x, axis.y, axis.z, (float)Math.Cos(angle / 2));
    }


    void InitializeOrientations()
    { 
        string serializedData = string.Empty;

        for(UInt32 i = 0; i < mMaxCount + 1; ++i)
        {
            float t = (float)i / mMaxCount;
            Quaternion q = Slerp(mInitial, mFinal, t);
            string outputLine = QuaternionToString(q);

            mOrientations.Add(i, q);
            serializedData += outputLine + "\r\n";
        }

        var path = AssetDatabase.GetAssetPath(OutputFile);
        
        if(File.Exists(path))
        {
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(serializedData);
            writer.Flush();

            Debug.Log("Sucecssfully written to file");
        }
        else
        {
            Debug.Log(path + " could not be found!");
        }

        transform.rotation = mOrientations[0];
    }

    Quaternion Slerp(Quaternion q1, Quaternion q2, float t)
    {
        double dot = Quaternion.Dot(mInitial, mFinal);
        double invCos = Math.Acos(dot);
        double denom = Math.Sqrt(1 - dot * dot);

        float a = (float)(Math.Sin((1 - t) * invCos) / denom);
        float b = (float)(Math.Sin(t * invCos) / denom);

        return new Quaternion(
            q1.x * a + q2.x * b,
            q1.y * a + q2.y * b,
            q1.z * a + q2.z * b,
            q1.w * a + q2.w * b
        );
    }

    public void AdjustOrientation(float value)
    {
        mCount = (UInt32)value;
        transform.rotation = mOrientations[mCount];
    }

    // Update is called once per frame
    void Update()
    {
    }
}
