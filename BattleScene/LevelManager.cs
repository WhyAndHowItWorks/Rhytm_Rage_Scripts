using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject EmptyLevel;
    public List<Level> StoryLevel = new List<Level>();
    public List<Level> CustomLevel = new List<Level>();
    public List<string> LevelPath = new List<string>();
    public List<string> MusicPath = new List<string>();

    public List<AudioClip> musics = new List<AudioClip>();

    public void Start()
    {
        LoadLevels();
        LoadCustomLevels();
    }

    public void LoadLevels() // Прочтение базовых уровней
    {
        string mainpath = Application.streamingAssetsPath;
        
        StoryLevel.Clear();
        for (int i = 0; i < LevelPath.Count; i++)
        {
           StoryLevel.Add(LoadLevel(musics[i], mainpath + LevelPath[i]));
        }
       
    }
    public Level LoadLevel(string SoundPath, string TrackPath)
    {
        GameObject level = Instantiate(EmptyLevel, transform.position, transform.rotation);
        Level temp = level.GetComponent<Level>();
        temp.soundPath = SoundPath;
        temp.TrackPath = TrackPath;
        StreamReader sr = new StreamReader(TrackPath);
        temp.LevelName = sr.ReadLine();
        temp.LoadLevel();
        sr.Close();
        return temp;
    }
    public Level LoadLevel(AudioClip au, string TrackPath)
    {
        GameObject level = Instantiate(EmptyLevel, transform.position, transform.rotation);
        Level temp = level.GetComponent<Level>();
        temp.audioClip = au;
        temp.TrackPath = TrackPath;
        StreamReader sr = new StreamReader(TrackPath);
        temp.LevelName = sr.ReadLine();
        sr.Close();
        return temp;
    }
    public void LoadCustomLevels()
    {
        string mainpath = Application.streamingAssetsPath + "\\Maps";
        
        DirectoryInfo MapsDir = new DirectoryInfo(mainpath);
        DirectoryInfo[] Maps = MapsDir.GetDirectories();
        for (int i = 0; i < Maps.Length; i++)
        {
            FileInfo[] MusicFile = Maps[i].GetFiles("Music*");          
            FileInfo[] TrackFile = Maps[i].GetFiles("*.txt");          
            if (TrackFile.Length > 0 && MusicFile.Length > 0)
            {
               
                CustomLevel.Add( LoadLevel(MusicFile[0].FullName, TrackFile[0].FullName)); 
            }
        }

    }
}
