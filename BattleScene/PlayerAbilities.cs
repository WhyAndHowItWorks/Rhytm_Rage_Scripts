using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/// <summary>
/// Класс отвечающий за характеристики главного героя и взаимодействие игрока с игрой
/// </summary>
public class PlayerAbilities : Actor
{
    public bool IsDead 
    {
        get { return isDead; }
        set { 
            isDead = value;
            if (isDead)
            {
                Head_an.Play("Помехи_Idle");
                Death();
            }

        }
    }
    bool isDead;

    public Router rt;
    public float health;
    public float shield_Health;
    public float Shield_Health
    {
        get { return shield_Health; }
        set 
        {
            shield_Health = value;
            Healthbar.Shield_health = value;
        }
    }
    public GameObject ShieldPole;
    public Transform ShieldPoint;

    [Header("Анимация")]
    public Animator animator;
    public const string Shoot_onehand_short = "Shooting_short";
    public const string Shoot_twohand_short = "2 hands_Shooting_short";


    public Transform TargetPoint; // Цель для противников

    public float Health
    {
        get { return health; }
        set {
            if (health < value)
            {
                //Хил
            }
            if (health > value)
            {
                if (!IsDead)
                {
                    float Damage = health - value;
                    if (Shield_Health >= Damage) // Если здоровье щита полностью покрывает дамаг
                    {
                        ShieldEffect();
                        Shield_Health -= Damage;
                    }
                    else
                    {
                        if (Shield_Health > 0) // Если есть щит, то он активируется и снижает влетающий урон
                        {
                            ShieldEffect();
                        }
                        TakeDamage();
                        Damage -= Shield_Health;
                        Shield_Health = 0;
                        rt.uie.TakeDamage(Damage, gameObject.transform.Find("Graphic").Find("Head").gameObject);
                        Healthbar.Health = health - Damage;
                        health -= Damage;
                        if (health <= 0)
                        {
                            Destroy(Healthbar);
                            rt.st.EndBattle(false);
                            IsDead = true;
                        }
                    }
                }
              
            }
            
        }
    }
    public float MaxHealth { get; set; }

    public Transform HealthBarPoint;
    public HealthBar Healthbar;

    public Transform Head;
    public SpriteRenderer Head_sprite;
    public Animator Head_an;

    public List<Sprite> EmojiSprites = new List<Sprite>();
    public List<int> EmojiPriority = new List<int>();
    public List<Emoji> Emoji_type = new List<Emoji>();

    public enum Emoji : int
    {
        Idle,
        Transition,
        Disappointed,
        Surprised,
        Fuck,
        WarningSign,
        Crying,
        ScaryFace,
        Smile,
        Shit,
        UpUp,
        Angry
    }
    public bool IsActive { get; set; }

    [Header("Все про эмоции")]
    public float MaxTimeBetweenKills;
    public int Kills 
    {
        get
        {
            return kills;
        }
        set 
        {
            int oldkills = kills;
            kills = value;
            Invoke("ReduceKills", kills - oldkills,MaxTimeBetweenKills);
            if (kills >= 2)
            {
                CastEmoji(Emoji.ScaryFace);
            }
        }
    }
    public int kills;

    public float MaxTimeBetwwenMisses;
    public int Misses 
    {
        get
        {
            return misses;
        }
        set 
        {
            int oldmisses = misses;
            misses = value;
            Invoke("ReduceMisses", misses- oldmisses, MaxTimeBetwwenMisses);
            if (misses >= 3)
            {
                CastEmoji(Emoji.Shit);
            }
        }

    }
    public int misses;

    public float MaxTimeBetweenTakeDamage;
    public int DamageTakes
    {
        get
        {
            return damageTakes;
        }
        set
        {
            int oldmisses = damageTakes;
            damageTakes = value;
            Invoke("ReduceDamageTakes", damageTakes - oldmisses, MaxTimeBetweenTakeDamage);
            if (damageTakes >= 3)
            {
                CastEmoji(Emoji.Crying);
            }
        }

    }
    public int damageTakes;
    public Emoji CurrentEmoji
    {
        get { return currentEmoji; }
        set
        {          
            currentEmoji = value;
            Head_sprite.sprite = EmojiSprites[(int)currentEmoji];
            CurrentEmojiPriority = EmojiPriority[(int)currentEmoji];
            deltaEmojiTime = 0;
        }
    }
    Emoji currentEmoji;
    public int CurrentEmojiPriority;
   
    public bool IsEmojiVisible 
    {
        get { return isEmojiVisible; }
        set
        {
            if (isEmojiVisible != value)
            {
                isEmojiVisible = value;

            }
                      
        }
    }
    public bool isEmojiVisible;
    public float MaxEmojiTime;
    public float deltaEmojiTime;

    bool _50Achived;
    bool _100Achived;
    bool _150Achived;


    public void CastEmoji(Emoji e)
    {
        if (!IsDead)
        {
            int index = (int)e;
            if (CurrentEmojiPriority <= index)
            {

                if (CurrentEmoji != e)
                {
                    if (Head_an.GetCurrentAnimatorStateInfo(0).IsName("Анимация помех"))
                    {
                        Head_an.SetTrigger("Transtion");
                    }
                    else { Head_an.Play("Анимация помех"); }
                }
                CurrentEmoji = e;

            }
        }
        
    }

    public void OnEnemyAction(Enemy e, EnemyAction ea)
    {

        if (ea == EnemyAction.Died)
        {
            CastEmoji(Emoji.Fuck);
            Kills++;
        }
        else if(ea == EnemyAction.Created)
        {
            CastEmoji(Emoji.WarningSign);
        }
    }

    public void OnStart()
    {
        IsActive = true;
        LoadOptions();
        MaxHealth = Health; 
        rt.nt.NoteActedEvent += NoteAction;
        rt.es.OnEnemyEvent += OnEnemyAction;
        rt.cm.ComboChanged += OnComboChanged;
        Invoke("AfterGGCome", 1f);
    }
    public void OnComboChanged(int combo)
    {
        if (combo >= 50 && !_50Achived)
        {
            CastEmoji(Emoji.Smile);
            _50Achived = true;
        }
        if (combo >= 100 && !_100Achived)
        {
            CastEmoji(Emoji.UpUp);
            _100Achived = true;
        }
        if (combo >= 150 && !_150Achived)
        {
            CastEmoji(Emoji.Angry);
            _150Achived = true;
        }
    }
    public void ReduceKills(int Amount)
    {
        kills -= Amount;
    }
    public void ReduceMisses(int Amount)
    {
        misses -= Amount;
    }
    public void ReduceDamageTakes(int Amount)
    {
        damageTakes -= Amount;
    }

    public void LoadOptions()
    {
        health = rt.oh.so.HeroHP;
    }

    public void AfterGGCome()
    {
        Head.DetachChildren();
        Head_an.gameObject.transform.SetParent(Head);
        Healthbar = rt.uie.CreateGGHealthBar(this);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        ShieldPole.transform.position = ShieldPoint.position + new Vector3(0,0,-3);
        if (currentEmoji != Emoji.Idle)
        {
            deltaEmojiTime += Time.deltaTime;
            if (deltaEmojiTime >= MaxEmojiTime)
            {
                CurrentEmoji = Emoji.Idle;
                
            }
        }

    }
    
    public void TakeDamage()
    {
        


            CastEmoji(Emoji.Surprised);
            DamageTakes++;
            // Проигрывание анимации
            PlayAnim("Taking FinalDamage", "Take_Damage");
        
    }

    public void ShieldEffect()
    {
        ShieldPole.GetComponent<Animator>().SetTrigger("BLINK");
    }
    public void Block()
    {

        // Проигрывание анимации
        PlayAnim("Blocking", "Block");
        Animator an = rt.ui.BlockPanel.GetComponent<Animator>();
        if (an.GetCurrentAnimatorStateInfo(0).IsName("Blink"))
        {
            an.SetTrigger("Blink");
        }
        else { an.Play("Blink"); }
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (IsActive)
        {
            if (!rt.nt.IsGgFhase)
            {
                if (Pressed)
                {
                    rt.es.GatherEnemyInformation();
                    if (rt.es.KolvoEnemies > 0)
                    {
                        Block();
                    }
                }
                

                
            }
            else 
            {
                if (!Pressed && nfg.noteInfo.type != NoteType.SliderDot)
                {
                    CastEmoji(Emoji.Disappointed);
                    Misses++;
                }
            }
            
    }
    }
    public void Death()
    {
        animator.Play("Death");
        CurrentEmoji = Emoji.Idle;
        Head.gameObject.GetComponent<SpriteRenderer>().sprite = EmojiSprites[12];
    }
    public void BattleWasEnd(bool IsWin)
    {
        if (IsWin)
        {
            PlayerWon();
        }
    }
    public void PlayerWon()
    {
        if (rt.rp.StoryLevel == 3 && !rt.rp.IsCustomLevel)
        {
            animator.Play("LeavingB");
        }
        else 
        {
            animator.Play("Leaving");
        }
        
        CurrentEmoji = Emoji.Idle;
        Head_an.gameObject.SetActive(false);

    }
    public void PlayAnim(string Name)
    {
        if (!IsDead && !rt.st.BattleWasEnded)
        {
            animator.Play(Name);
        }
    }
    public void PlayAnim(string AnimName, string TrigerName)
    {
        if (!IsDead && !rt.st.BattleWasEnded)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
            {
                animator.SetTrigger(TrigerName);
            }
            else { animator.Play(AnimName); }
        }
    }

    /// <summary>
    ///  НЕ РАБОТАЕТ С ПРИВАТНЫМИ МЕТОДАМИ
    /// </summary>
    /// <param name="method"></param>
    /// <param name="options"></param>
    /// <param name="delay"></param>
    public void Invoke(string method, object options, float delay)
    {
        StartCoroutine(_invoke(method, delay, options));
    }
    private IEnumerator _invoke(string method, float delay, params object[] obj)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        
        Type instance = GetType();

        MethodInfo[] mth = instance.GetMethods();
        List<MethodInfo> mthd = new List<MethodInfo>();

        for (int i = 0; i < mth.Length; i++)
        {
            if (mth[i].Name == method)
            {
                mthd.Add(mth[i]);
            }
        }
      

        if (mthd.Count == 1)
        {
            mthd[0].Invoke(this, obj);
        }
        else
        {
            for (int i = 0; i < mthd.Count; i++)
            {
                ParameterInfo[] par = mthd[i].GetParameters();
                if (par.Length == obj.Length)
                {
                    bool IsCorrect = true;
                    for (int g = 0; g < par.Length; g++)
                    {
                        if (par[g].ParameterType != obj[g].GetType())
                        {
                            IsCorrect = false;
                            break;
                        }
                    }
                    if (IsCorrect)
                    {
                        
                        mthd[i].Invoke(this, obj);
                    }
                }
            }
        }



        yield return null;
    }
}
