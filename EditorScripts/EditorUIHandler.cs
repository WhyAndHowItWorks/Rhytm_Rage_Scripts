using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class EditorUIHandler : MonoBehaviour
{
    public NoteTrack nt;

    ////////UI////////
    [Header("Детали интерфейса")]
    public GameObject CreateNewLevelPanel;
  
    public GameObject ChooseEnemyPanel;
    public Dropdown ChooseTrack;
    public GameObject PlayButton;
    public GameObject Text; // Текст для триггера 

    public GameObject HelpPanel;
    public GameObject CanvasForTexts;
    public GameObject CanvasMenu;
    public GameObject HighDot;
    public GameObject SliderDotsObject;
    public Scrollbar scr;
    public Toggle CenterXAxisToogle;
    public Dropdown AxisDivideLevel;
    public Text ChosenInstrument;
    public InputField TrackNameInput;

    public Text ConsoleInfo;
    public Text CurrentCamPhase;

    public bool CursorOnTrack;
    public bool CursorIsActive;
    
   
    public float ClickX;
    public float ClickY;
    [Header("Изменение ключевых параметров")]
    public InputField NoteSpeedUI; // Поле для изменения скорости нот
    
    
    [Header ("Изменение масштаба")]
    public Slider TrackSize;
    public float MinTrackSize;
    public float MaxTrackSize;
    public GameObject DownStick;
    public GameObject Wallpaper;

    [Header ("Отображение на миникарте")]
    public GameObject DotOnMinimapNote; // точка на миникарте для обычной ноты
    public GameObject DotOnMinimapTrigger; // точка на миникарте для триггера
    public GameObject DotOnMinimapSlider; // точка на миникарте для слайдера
    public RectTransform[] AxisDots = new RectTransform[4]; // точки для отображения осей
    public RectTransform AxisForTrigger; // точки для отображения оси триггера

    public InputField CamSpeedInput;

    //Шорткаты
    SpawningMode spm;
    public void Start()
    {
        CamSpeedInput.text = (nt.oh.so.CamSpeed/100).ToString();
        
        spm = (SpawningMode)nt.Instruments[(int)ToolMode.PlaseNote];
        nt.ParameterChangedEvent += ParameterChanged;
        for (int i = 0; i < nt.lm.CustomLevel.Count; i++)
        {
            AddLevelToTrackChoose(nt.lm.CustomLevel[i]);
        }
    }
    public void Update()
    {
        CameraPhaseFound();
    }
    public void CameraPhaseFound()
    {
        SpawningMode sm = (SpawningMode)nt.Instruments[(int)ToolMode.PlaseNote];
        List<GameObject> delta = sm.ChangeFhaseTriggers;
        float CamY = nt.TapLine.transform.position.y;
        if (delta.Count == 0)
        {
            CurrentCamPhase.text = "GG PHASE";
        }
        else 
        {
            if (delta[0].transform.position.y > CamY)
            {
                CurrentCamPhase.text = "GG PHASE";
            }
            else
            {
                for (int i = 0; i < delta.Count; i++)
                {

                    if (delta[i].transform.position.y <= CamY)
                    {
                        if (i % 2 == 0)
                        {
                            CurrentCamPhase.text = "ENEMY PHASE";
                        }
                        else
                        {
                            CurrentCamPhase.text = "GG PHASE";
                        }
                    }
                }
            }
        }
        
    }
    public void ChoosedTrackChange()
    {
        nt.BuildTrackFromFile(ChooseTrack.value);
        nt.Music.clip = nt.lm.CustomLevel[ChooseTrack.value].audioClip;
        nt.cnc.ClearHistory();
    }

    public void AddLevelToTrackChoose(Level lv)
    {
        ChooseTrack.AddOptions(new List<string>() { lv.LevelName });
    }
    public void CamSpeedInputFinished() // Была введена скорость камеры
    {
        int res;
        if (int.TryParse(CamSpeedInput.text, out res)) // Если ввод корректен
        {
            nt.CamSpeed = res * 100;
            nt.oh.so.CamSpeed = res * 100;
            nt.oh.WriteOptionsToFile();
        }
        else 
        {
            CamSpeedInput.text = "Uncorrect input AAAA!";
        }
    }

    public void OpenCreateNewLevelPanel()
    {
        CreateNewLevelPanel.SetActive(true);
        CreateNewLevelPanel.GetComponent<CreateNewLevelPanel>().Open();
    }
    public void CloseCreateNewLevelPanel()
    {
        CreateNewLevelPanel.SetActive(false);
        CreateNewLevelPanel.GetComponent<CreateNewLevelPanel>().Cancel();
            
    }
    public void ChangeLineDivideLevel()// Изменения степени деления прямых
    {
        nt.tp.DivideLevel = AxisDivideLevel.value;
    }
    public void CenterOnXAxis() // Метод при изменении значения выравнивания
    {
        nt.NeedCenterX = CenterXAxisToogle.isOn;
    }

    /// <summary>
    /// Написать инфу в консоль
    /// </summary>
    /// <param name="info"></param>
    public void WriteToConsole(string info)
    {
        ConsoleInfo.text = info;
    }
    public void Selecting()
    {
        nt.Mode = ToolMode.Selecting;
    }
    public void Play() // Запускает трек с места, где он остановился
    {
        nt.Mode = ToolMode.Playing;
        PlayingMode pm = (PlayingMode)nt.Instruments[(int)ToolMode.Playing];
        pm.Play();
    }

    public void Pause() // Ставит трек на паузу
    {
        nt.Mode = ToolMode.Playing;
        PlayingMode pm = (PlayingMode)nt.Instruments[(int)ToolMode.Playing];
        pm.Pause();
    }
    public void DeleteMode()
    {
        nt.Mode = ToolMode.Deleting;
    }
    public void Moving() // Вход в режим переноса нот
    {
        nt.Mode = ToolMode.Moving;
    }
    public void PlayFromStart() // Начинает проигрывание трека со старта
    {
        nt.Mode = ToolMode.Playing;
        PlayingMode pm = (PlayingMode)nt.Instruments[(int)ToolMode.Playing];
        pm.PlayFromStart();
    }
    public void PressRedNote()// Включает режим установки и дает для установки красную ноту
    {
        if (spm.delta_nt.Type == NoteEditorType.Trigger)
        {
            spm.delta_nt.Type = NoteEditorType.Note;
        }
        spm.delta_nt.Number = 0;
        nt.Mode = ToolMode.PlaseNote;
    }
    public void PressBlueNote()// Включает режим установки и дает для установки синюю ноту
    {
        if (spm.delta_nt.Type == NoteEditorType.Trigger)
        {
            spm.delta_nt.Type = NoteEditorType.Note;
        }
        spm.delta_nt.Number = 1;
        nt.Mode = ToolMode.PlaseNote;
    }
    public void PressYellowNote()// Включает режим установки и дает для установки желтую ноту
    {
        if (spm.delta_nt.Type == NoteEditorType.Trigger)
        {
            spm.delta_nt.Type = NoteEditorType.Note;
        }
        spm.delta_nt.Number = 2;
        nt.Mode = ToolMode.PlaseNote;
    }
    public void PressGreenNote()// Включает режим установки и дает для установки зеленую ноту
    {
        if (spm.delta_nt.Type == NoteEditorType.Trigger)
        {
            spm.delta_nt.Type = NoteEditorType.Note;
        }
        spm.delta_nt.Number = 3;
        nt.Mode = ToolMode.PlaseNote;
    }
    public void PressTriggerNoteChangeFhase() // Включает режим установки и дает для установки триггер
    {
        nt.Mode = ToolMode.PlaseNote;
        spm.delta_nt.Number = 0;
        spm.Deltanote_type = NoteEditorType.Trigger;
    }
    public void PressTriggerNoteSpawnEnemy()
    {
        nt.Mode = ToolMode.PlaseNote;
        spm.delta_nt.Number = 1;
        spm.Deltanote_type = NoteEditorType.Trigger;
        CursorIsActive = false;
        ChooseEnemyPanel.SetActive(true);      
    }
    public void PressTriggerEndLevel()
    {
        nt.Mode = ToolMode.PlaseNote;
        spm.delta_nt.Number = 2;
        spm.Deltanote_type = NoteEditorType.Trigger;
    }
    public void PressRecordingMode()
    {
        nt.Mode = ToolMode.Recording;
        nt.Music.time = nt.NotePositionToTime(nt.TapLine);
        nt.pm.IsPlaying = true;

    }


    public void ChooseEnemyId(int Id)
    {
        if (Id != -1)
        {
            spm.delta_nt.Line = Id;
        }
        ChooseEnemyPanel.SetActive(false);
        CursorIsActive = true;
    }

    public string DoNameForText(int Color,int Id)
    {

        if (Color == 1)
        {
            return nt.enf.TriggerTitles[Color] + nt.enf.TriggerEnemyTitles[Id];
        }
        else 
        {
            return nt.enf.TriggerTitles[Color];
        }                   
        
    }

    public void ChangeNoteSpeed()
    {
        float res;
        if (float.TryParse(NoteSpeedUI.text, out res))
        {
            nt.NoteSpeed = res;
        }
        else 
        {
            NoteSpeedUI.text = "UncorrectInput!";
        }

    }
    public void PressNote()
    {
        spm.Deltanote_type = NoteEditorType.Note;
        nt.Mode = ToolMode.PlaseNote;
    }
    public void PressSlider()
    {
        spm.Deltanote_type = NoteEditorType.Slider;
        nt.Mode = ToolMode.PlaseNote;
    }
    public void PressHelp()
    {
        HelpPanel.SetActive(!HelpPanel.active);
    }
    public void ChangeTrackScale()
    {
        float newScale = Mathf.Lerp(MinTrackSize, MaxTrackSize, TrackSize.value);
        Wallpaper.transform.localScale = new Vector3(newScale / 5, newScale / 5, 1);
        nt.cam.orthographicSize = newScale;
        DownStick.transform.localPosition = new Vector3(0, 0, 1);
        nt.DeltaCamVal = Mathf.Lerp(0, 0, TrackSize.value);
        nt.tp.CamSize = nt.cam.orthographicSize;
        gameObject.GetComponent<BoxCollider2D>().size = new Vector2(nt.tp.CamSize * 2, gameObject.GetComponent<BoxCollider2D>().size.y);
        nt.CameraYpos = nt.CameraYpos;
    }
    public void AttachTextToNote(GameObject Note, string TextForNote)
    {
        GameObject h = Instantiate(Text, CanvasForTexts.transform);
        h.GetComponent<Text>().text = TextForNote;
        Note.GetComponent<NotesForEditor>().Text = h;
        h.transform.position = Note.transform.position;
    }
    public void ScrollbarChangeSize()
    {
        // Обновление размера скроллбара
        if (nt.MaxY > nt.minSizeOfTrack)
        {

            scr.size = 1 / (nt.MaxY / nt.minSizeOfTrack);
            if (scr.size < 0.005f)
            {
                scr.size = 0.005f;
            }
            scr.SetValueWithoutNotify(nt.CameraYpos / nt.maxY);
        }

    }
    public void ScrollChangeValue() // Перемещение камеры при помощи бегунка
    {
        if (scr.value == float.NaN)
        {
            scr.value = 0;
        }
        nt.CameraYpos = scr.value * nt.MaxY;
    }
    public void ParameterChanged(TrackParameter par, float oldvalue, float newvalue)
    {
        if (par == TrackParameter.MaxY)
        {
            ScrollbarChangeSize();
        }
    }
   
}
