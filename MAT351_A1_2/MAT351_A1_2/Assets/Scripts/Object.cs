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
    int mMaxCount;
    int mCount = 0;
    List<Quaternion> mOrientations;

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
        
        mMaxCount = Convert.ToInt32(info3);

        Debug.Log("Initial: "  + QuaternionToString(mInitial));
        Debug.Log("Final: " + QuaternionToString(mFinal));
        Debug.Log("N : " + mMaxCount);
        
        mOrientations =  new List<Quaternion>();
        SliderControl.maxValue = mMaxCount;

        InitializeOrientations();
        
        InitialOrientationText.text = "Initial Orientation: " + QuaternionToString(mInitial);
        FinalOrientationText.text = "Final Orientation: " + QuaternionToString(mFinal);
        CurrentOrientationText.text = "Current Orientation: " + QuaternionToString(mOrientations[0]);
    }


    void InitializeOrientations()
    { 
        string serializedData = string.Empty;

        mCount = 0;
        SliderControl.value = 0;

        for(UInt32 i = 0; i < mMaxCount + 1; ++i)
        {
            float t = (float)i / mMaxCount;
            Quaternion q = Slerp(mInitial, mFinal, t);
            string outputLine = QuaternionToString(q);

            mOrientations.Add(q);
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
        Quaternion q3 = q2;
        double dot = Quaternion.Dot(q1, q2);

        if(dot < 0)
        {
            q3 = new Quaternion(-q2.x, -q2.y, -q2.z, -q2.w);
            dot = Quaternion.Dot(q1, q3);
        }
        
        double theta = Math.Acos(dot);
        float a, b;

        if(theta > 0)
        {
            double denom = Math.Sin(theta); // Math.Sqrt(1 - dot * dot);
            a = (float)(Math.Sin((1 - t) * theta) / denom);
            b = (float)(Math.Sin(t * theta) / denom);
        }
        else
        {
            a = (1 - t);
            b = t;
        }

        return new Quaternion(
            q1.x * a + q3.x * b,
            q1.y * a + q3.y * b,
            q1.z * a + q3.z * b,
            q1.w * a + q3.w * b
        );
    }

    public void AdjustOrientation(float value)
    {
        if(loaded == false)
        {
            SliderControl.value = 0;
            return;
        }

        mCount = (int)value;
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
