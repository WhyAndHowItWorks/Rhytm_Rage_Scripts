using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Level : MonoBehaviour
{
    public AudioClip audioClip;
    public string soundPath;
    
    public string TrackPath;

    public string LevelName;
    public void LoadLevel()
    {
        StartCoroutine(LoadAudio());
    }
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadAudio()
    {
 
        WWW request = new WWW(soundPath);
        yield return request;
        audioClip = request.GetAudioClip();
        audioClip.name = "l";
    }

}
