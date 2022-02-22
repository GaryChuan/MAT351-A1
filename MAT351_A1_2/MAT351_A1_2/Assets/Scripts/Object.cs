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

    Quaternion mInitial;
    Quaternion mFinal;
    UInt32 mMaxCount;
    UInt32 mCount = 0u;
    Dictionary<UInt32, Quaternion> mOrientations;

    public Slider SliderControl;
    public Text FinalOrientationText;
    public Text InitialOrientationText;
    public Text CurrentOrientationText;
    public Text ErrorText;

    private bool loaded = false;

    void Awake()
    {
        if(File.Exists(Application.dataPath + "/path.txt"))
        {
            Load();
            loaded = true;
        }
        else
        {
            ErrorText.gameObject.SetActive(true);
        }
    }

    string QuaternionToString(Quaternion q)
    {
        return "(" 
             + q.x.ToString("F3") + ", " 
             + q.y.ToString("F3") + ", " 
             + q.z.ToString("F3") + ", " 
             + (q.w * 180 / Math.PI).ToString("F3")
             + ")";
    }

    Quaternion Axis4DToQuaternions(float x, float y, float z, float angle)
    {
        float s = (float)Math.Sin(angle / 2);
        Vector3 axis = new Vector3(x, y, z);

        axis.Normalize();
        axis *= s;

        return new Quaternion(axis.x, axis.y, axis.z, (float)Math.Cos(angle / 2));
    }

    void Load()
    {
        StreamReader inputFile = File.OpenText(Application.dataPath + "/path.txt");

        var info1 = inputFile.ReadLine()?.Split(' ');
        var info2 = inputFile.ReadLine()?.Split(' ');
        var info3 = inputFile.ReadLine();

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
        
        InitialOrientationText.text = "Initial Orientation: " + QuaternionToString(mInitial);
        FinalOrientationText.text = "Final Orientation: " + QuaternionToString(mFinal);
        CurrentOrientationText.text = "Current Orientation: " + QuaternionToString(mOrientations[0]);
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

        // var path = "Assets/Resources/output.txt"; //AssetDatabase.GetAssetPath(OutputFile);
        
        StreamWriter file = File.CreateText(Application.dataPath + "/output.txt");
        file.Write(serializedData);
        file.Flush();

        // if(File.Exists(path))
        // {
        //     StreamWriter writer = new StreamWriter(path, false);
        //     writer.Write(serializedData);
        //     writer.Flush();
            
        //     Debug.Log("Sucecssfully written to file");
        // }
        // else
        // {
        //     Debug.Log(path + " could not be found!");
        // }
        
        // File.CreateText(Application.dataPath + "/outputt.txt");
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
        if(loaded == false)
        {
            SliderControl.value = 0;
            return;
        }

        mCount = (UInt32)value;
        transform.rotation = mOrientations[mCount];
        CurrentOrientationText.text = "Current Orientation: " + QuaternionToString(transform.rotation);
    }

    public void Reload()
    {
        if(File.Exists(Application.dataPath + "/path.txt"))
        {
            Load();
            ErrorText.gameObject.SetActive(false);
            loaded = true;
        }
        else
        {
            ErrorText.gameObject.SetActive(true);
            loaded = false;
        }
    }

    public void Reset()
    {
        if(loaded)
        {
            mCount = 0;
            SliderControl.value = 0;
            transform.rotation = mOrientations[mCount];
        }
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
