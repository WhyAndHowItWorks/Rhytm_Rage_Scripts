using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.UIElements;

public class CreateNewLevelPanel : MonoBehaviour
{
    public LevelManager lm;
    public NoteTrack nt;
    [Header("Интерфейс")]
    public InputField FoundFilePath_text;

    public InputField NameOfTrack_input;
    public InputField BPM_input;
    public InputField NoteSpeed_input;

    public string NameOfTrack;
    public float BPM;
    public float NoteSpeed;

    public Text Console;
    public string FoundFilePath
    {
        get { return foundFilePath; }
        set 
        {
            foundFilePath = value;
            FoundFilePath_text.text = value;
        }
    }
    public string foundFilePath;
    public void Start()
    {
        lm = GameObject.Find("Хранитель настроек").GetComponent<LevelManager>();
    }
    public void FoundMusicFile()
    {
        var extensions = new[] {new ExtensionFilter("Music Files" , "mp3","wav","flac")};
        string[] path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
        FoundFilePath = path[0];
    }
    public void NameOfTrackInputFinished()
    {
        NameOfTrack = NameOfTrack_input.text;
    }
    public void BpmInputFinished()
    {
        float res;
        if (float.TryParse(BPM_input.text, out res))
        {
            BPM = res;
        }
        else
        {
            BPM_input.text = "Uncorrect input";
        }
    }
    public void NoteSpeedInputFinished()
    {
        float res;
        if (float.TryParse(NoteSpeed_input.text, out res))
        {
            NoteSpeed = res;
        }
        else
        {
            NoteSpeed_input.text = "Uncorrect input";
        }
    }

    /// <summary>
    /// Действия при закрытии вкладки
    /// </summary>
    public void Cancel()
    { 
    }
    /// <summary>
    /// Действия при закртытии вкладки
    /// </summary>
    public void Open()
    {
        NameOfTrack = "";
        BPM = -10;
        NoteSpeed = -10;
        FoundFilePath = "";
    }

    public void CreateLevelActionButton()
    {
        if (NameOfTrack == "")
        {
            Console.text = "Type Track Name!";
        }
        else if (FoundFilePath == "")
        {
            Console.text = "Choose music for level!";
        }
        else if (BPM <= 0)
        {
            Console.text = "Choose right BPM!";
        }
        else if (NoteSpeed <= 0)
        {
            Console.text = "Choose right notespeed!";
        }
        else 
        {
            CreateLevel();
        }
    }
    public void CreateLevel()
    {
            string MapPath = Application.streamingAssetsPath + "\\Maps\\";
            DirectoryInfo MapDir = new DirectoryInfo(MapPath + NameOfTrack);
        if (!MapDir.Exists) // Если такого уровня еще нет
        {
            MapDir.Create();
            FileInfo Music = new FileInfo(FoundFilePath);
            
            Music.CopyTo(MapDir.FullName +"\\Music"+Music.Extension);
            FileInfo trackFile = new FileInfo(MapDir.FullName + "\\Track.txt");
            StreamWriter sw = trackFile.CreateText();
            sw.WriteLine(NameOfTrack);
            sw.WriteLine(Music.FullName);
            sw.WriteLine(BPM);
            sw.WriteLine(NoteSpeed);
            sw.WriteLine(1);
            sw.WriteLine(1);
            sw.WriteLine("Триггер");
            sw.WriteLine(2);
            sw.WriteLine(0);
            sw.WriteLine(0);
            sw.WriteLine(0.1f);
            sw.WriteLine("Конец файла");
            sw.Close();
            Console.text = "Level created! Let's make some noise!";
            lm.CustomLevel.Add(lm.LoadLevel(Music.FullName, trackFile.FullName));
            nt.euh.AddLevelToTrackChoose(lm.CustomLevel[lm.CustomLevel.Count - 1]);
            
        }
        else 
        {
            Console.text = "Level with the same name already exists, please type something else!";
        }
        
      
    }
}
