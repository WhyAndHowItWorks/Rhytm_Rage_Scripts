using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;


using Unity.VisualScripting;
using Mono.Cecil;
using Unity.VisualScripting.FullSerializer;

public class OptionsHandler : MonoBehaviour
{
    public string s;
    public string path;
    public SerializeOptions so;
    public static GameObject instance;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));
    public bool Test;
    
    public float Volume
    {
        get { return so.Volume;
        }
        set { 
            so.Volume = value;
            OnVarChange(OptionVarName.Volume, so.Volume);
        }
    }
    public float HitSoundVolume
    {
        get { return so.HitSoundVolume; }
        set 
        {
            so.HitSoundVolume = value;
            OnVarChange(OptionVarName.HitSoundVolume, so.HitSoundVolume);
        }
    }

    public bool VerSins 
    {
        get { return so.VerSins; }
        set {
            
           
            so.VerSins = value;
            if (so.VerSins)
            {
                QualitySettings.vSyncCount = 1;
            }
            else { QualitySettings.vSyncCount = 0; }
        }
    }
    public bool FullScreen
    {
        get { return so.FullScreen; }
        set { 
            so.FullScreen = value;
            Screen.fullScreen = so.FullScreen;
        }
    }
    
    public delegate void VarChanged(OptionVarName var, float value);
    public event VarChanged OnVarChange;



    public void Start()
    {
        
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        so = new SerializeOptions();
        path = Application.dataPath + "/StreamingAssets/Options.txt";
        OnVarChange = SoundTest;
        ReadOptionsFromFile();
       
        DontDestroyOnLoad(gameObject);
        
        
        
        

    }
    private void OnLevelWasLoaded(int level)
    {
        ReadOptionsFromFile();
    }

    public void Update()
    {
        if (Test)
        {
            Test = false;
            WriteOptionsToFile();
        }
    }


    public void SoundTest(OptionVarName var, float value)
    { if (var == OptionVarName.Volume)
        {
            Debug.Log("Громкость теперь " + value * 100);
        }

            }
    public void WriteOptionsToFile()
    {
        Debug.Log("Запись");
        string serializePath = Application.dataPath + "/StreamingAssets/SerOptions.txt";
        FileInfo f = new FileInfo(serializePath);
        f.Delete();
        XmlSerializer ser = new XmlSerializer(typeof(SerializeOptions));
        Stream fStream = new FileStream(serializePath, FileMode.Create, FileAccess.Write, FileShare.None);
            ser.Serialize(fStream, so);
       
            fStream.Close();
             
    } // Добавить Стартовый уровень!
    public void ReadOptionsFromFile()
    {
        Debug.Log("Чтение");
        string serializePath = Application.dataPath + "/StreamingAssets/SerOptions.txt";
        StreamReader fs = new StreamReader(serializePath);
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializeOptions));
        so = xmlSerializer.Deserialize(fs) as SerializeOptions;
        fs.Close();
    } // Добавить Стартовый уровень!

   
    public KeyCode ReadLetter(StreamReader sr)
    {
        string temp = sr.ReadLine();
        foreach (KeyCode keyCode in keyCodes)
        {
            if (keyCode.ToString() == temp)
            {
                return keyCode;
            }

        }
        return KeyCode.None;
    }
    public string FloatToString(float s)
    {
        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        return s.ToString(formatter);
    }
    public float FloatParse(StreamReader sr)
    {

        float res;
        string s = sr.ReadLine();

        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        res = float.Parse(s, formatter);

        return res;
    }
}
