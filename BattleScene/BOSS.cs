using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BOSS : Enemy
{
    public bool IsDead;
    [Header("��� ��� �����")]

    public int[] DotCount = new int[4];
    public int[] MaxDotCount = new int[4];

    public int oldsliderColor;
    public SliderForGame LeadSlider
    {
        get { return leadSlider; }
        set {

            if (leadSlider != null)
            {
                oldsliderColor = leadSlider.noteInfo.Color;
            }
            if (state == BossState.BladeSlider) // ���� ���� ����� � ����� �������
            {
                CastWaveAttack(leadSlider);
            }

            leadSlider = value;
            if (leadSlider == null)
            {
                State = BossState.Idle;

            }
            else
            {

                if (leadSlider.noteInfo.Color < 3)
                {
                    State = BossState.Slider;
                }
                else
                {
                    State = BossState.BladeSlider;
                }
                SliderAnimation();
            }



        }
    }
    public SliderForGame leadSlider;

    public BossState State
       {
        get { return state; }
        set 
        {
            
            if (value == BossState.Idle && state == BossState.BladeSlider)
            {
                PlayAnim("Attack_Blade_long_end");
            }
            state = value;
            switch (state)
            {
                case (BossState.Idle):
                    Debug.Log("��� ���������");
                    break;
                case (BossState.Slider):
                    Debug.Log("���� �������");
                    break;
                case (BossState.BladeSlider):
                    Debug.Log("�����");
                    break;
            }
        }
        }
    public BossState state;

    // ��� ��� ������� ����
    public int[] DamageForColors = new int[4];
    public float[] SliderCheckDelay = new float[4];

    public int BladeCharges;
    public int DamagePerCharge;

    public float ShieldPerNote;
    public GameObject ShieldPole;

    public NoteForGame deltanfg;
    public bool Pressed;

    public bool BladeAttack;
    public GameObject SmallBladeAttack;
    public GameObject BigBladeAttack;

    public GameObject BladeAttackShootPosition;

    public Transform HealthBarSpawnPoint;

    public GameObject BladeEffect;
    public GameObject BigBladeEffect;
    public GameObject BladeEffectPlaceDot;


    public GameObject BossGunEffect;
    public GameObject BossGunEffectPlaceDot;

    public GameObject BossShotGunEffect;
    public GameObject BossShotGunEffectPlaceDot;

    public GameObject BossLaserGunEffect;
    public GameObject BossLaserGunEffectPlaceDot;


    
   
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!IsDead)
        {
            this.Pressed = Pressed;
            deltanfg = nfg;

            if (!rt.nt.IsGgFhase) // ���� ���� �����������
            {
                if (nfg.noteInfo.type == NoteType.Note) // ���� ������� ����
                {
                    NoteColorAction(nfg, nfg.noteInfo.Color);
                }
                if (nfg.noteInfo.type == NoteType.SliderDot) // ���� ����� ��������
                {
                    int Color = nfg.noteInfo.Color;
                    DotCount[Color]++;
                    if (DotCount[Color] >= MaxDotCount[Color])
                    {
                        DotCount[Color] = 0;
                        NoteColorAction(nfg, Color);
                    }
                }
            }
            else // ��������� ���� ��� �������
            {
                if (nfg.noteInfo.type == NoteType.Note) // ���� ������� ����
                {
                    ShieldChargeAction(nfg.noteInfo.Color);
                }
                if (nfg.noteInfo.type == NoteType.SliderDot) // ���� ����� ��������
                {
                    int Color = nfg.noteInfo.Color;
                    DotCount[Color]++;
                    if (DotCount[Color] >= MaxDotCount[Color])
                    {
                        DotCount[Color] = 0;
                        ShieldChargeAction(Color);
                    }
                }

            }
        }
    }

    public new void Start()
    {
        base.Start();
        rt.nt.SliderActionEvent += SliderAction;
        rt.nt.PhaseChangedEvent += PhaseChangeAction;
       

    }

    public void Update()
    {


        BossLaserGunEffect.transform.position = BossLaserGunEffectPlaceDot.transform.position;
        LookAt2D(BossLaserGunEffect, rt.pa.TargetPoint.transform.position);

        BossShotGunEffect.transform.position = BossShotGunEffectPlaceDot.transform.position;
        LookAt2D(BossShotGunEffect, rt.pa.TargetPoint.transform.position);

        BossGunEffect.transform.position = BossGunEffectPlaceDot.transform.position;
        LookAt2D(BossGunEffect, rt.pa.TargetPoint.transform.position);

        BigBladeEffect.transform.position = BladeEffectPlaceDot.transform.position;
        BigBladeEffect.transform.rotation = BladeEffectPlaceDot.transform.rotation;
        BladeEffect.transform.position = BladeEffectPlaceDot.transform.position;
        BladeEffect.transform.rotation = BladeEffectPlaceDot.transform.rotation;

        if (State == BossState.Slider && an.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            SliderAnimation();
        }
        //�������� ����� �������� � ���������
    }
    public override void LoadOptions()
    {

        SerializeOptions so = rt.oh.so;
        int[] delta = new int[4] { so.PistolDamage, so.ShotgunDamage, so.LaserDamage, so.BladeDamage };
        DamageForColors = delta;
        health = so.BossHP;
        DamagePerCharge = so.BossPlusDamage;
        ShieldPerNote = so.BossPlusShield;

    }
    public override void DotsAction()
    {
        
    }
    public void PhaseChangeAction(bool IsGGPhase)
    {
        if (IsGGPhase) // ���� ���� ��
        {
            LeadSlider = null;
        }
        else // ���� ���� �����������
        {
            TryToFindSlider();
        }
    }
    public void SliderAction(bool StartOrEnd, NoteForGame nfg)
    {
        if (!rt.nt.IsGgFhase)
        {
        
        if (StartOrEnd) // ���� ������� ����� �������
        {
            switch (State)
            {
                case (BossState.Idle): // ���� �����
                    LeadSlider = (SliderForGame)nfg;

                    break;
                case (BossState.Slider): // ���� �������
                    LeadSlider = (SliderForGame)nfg;
                    break;
                case (BossState.BladeSlider):

                    break;
            }

        }
        else
        {
            if (nfg == LeadSlider)
            {
                TryToFindSlider();
            }

        }
    }
       
               
    }
    public void TryToFindSlider() // ���� ������� ����������, �� ���� �����. ������ ��������� �������
    {

        SliderForGame delta = null;
        SliderForGame Blade = null;
        for (int i = 0; i < 4; i++)
        {
            if (rt.nt.sliderInfo[i] != null)
            {
                delta = rt.nt.sliderInfo[i];

            }
            if (rt.nt.sliderInfo[i] != null && rt.nt.sliderInfo[i].noteInfo.Color == 3)
            {
                Blade = rt.nt.sliderInfo[i];
            }
        }
        if (Blade == null)
        {
            LeadSlider = delta;
        }
        else { LeadSlider = Blade; }
        
    }
    public void NoteColorAction(NoteForGame nfg, int Color)
    {
        if (State == BossState.BladeSlider) // ���� ������, �� ������� ����
        {
            switch (Color)
            {
                case (0):
                    BladeCharges++;
                    break;
                case (1):
                    BladeCharges += 2;
                    break;
                case (2):
                    BladeCharges += 2;
                    break;
                case (3):
                    BladeCharges += 4;
                    break;
            }
        }
        else // ���� ����� ��� �������, �� �������� ��������
        {
            switch (Color)
            {
                case (0):
                    PlayAnim("Atta�k_Gun", "Shoot_Gun");
                    break;
                case (1):
                    PlayAnim("Attack_Shotgun", "Shoot_ShotGun");
                    break;
                case (2):
                    PlayAnim("Attack_Laser", "Shoot_Laser");
                    break;
                case (3):
                    SmallBladeAttackA(nfg);
                    PlayAnim("Attack_Blade", "Shoot_Blade");
                    break;
            }
            //���� ������ ��� �������
            if (!Pressed && Color != 3)
            {
                DoDamageToPlayer(DamageForColors[Color]);
            }
        }
            
           
        
      
    }
    public void SmallBladeAttackA(NoteForGame nfg)
    {    
        // ������� �����
        GameObject g = Instantiate(SmallBladeAttack, BladeAttackShootPosition.transform.position, BladeAttackShootPosition.transform.rotation);
        // ��������� �������� � ���� �� � �����������, � ����� ������� �� � ����
        int FoundNote = rt.nam.FoundNoteWithTime(nfg.noteInfo, 3f);
        


        float f = rt.nam.TimeFromNowToNote(FoundNote);
        if (FoundNote == nfg.noteInfo.IdInTrack || f < 0.3f)
        {
            
            if (!Pressed)
            {
                DoDamageToPlayer(DamageForColors[3], 0.3f);
            }
            g.GetComponent<ProjectileMover>().StepPerSecond = (rt.pa.TargetPoint.transform.position - BladeAttackShootPosition.transform.position) / 0.3f;
        }
        else {
            rt.nam.AttachModToNote(FoundNote, new BossProjectileMod(), new object[] { this, g, (float)DamageForColors[3] });
           
            g.GetComponent<ProjectileMover>().StepPerSecond = (rt.pa.TargetPoint.transform.position - BladeAttackShootPosition.transform.position) / (f);
        }
        
    }
    public void ShieldChargeAction(int Color)
    {
        Debug.Log("�����");
        if (!Pressed)
        {
            switch (Color)
            {
                case (0):
                    Shield += ShieldPerNote;
                    break;
                case (1):
                    Shield += ShieldPerNote * 2;
                    break;
                case (2):
                    Shield += ShieldPerNote * 2;
                    break;
                case (3):
                    Shield += ShieldPerNote * 4;
                    break;
            }
        }
    }

    public void SliderAnimation()
    {
        switch (leadSlider.noteInfo.Color)
        {
            case (0):
                PlayAnim("Attack_Gun_long_begin");
                break;
            case (1):
                PlayAnim("Attack_Shotgun");
                break;
            case (2):
                PlayAnim("Attack_Laser_long_begin");
                break;
            case (3):
                PlayAnim("Attack_Blade_long_begin");
                break;
        }
    }
    

    public void CastWaveAttack(NoteForGame nfg)
    {
       
        // ������� �����
        GameObject g = Instantiate(BigBladeAttack, BladeAttackShootPosition.transform.position, BladeAttackShootPosition.transform.rotation);
        // ��������� �������� � ���� �� � �����������, � ����� ������� �� � ����
        
        int FoundNote = rt.nam.FoundNoteWithTime(nfg.noteInfo, 3f);
        float Damage = DamageForColors[3] + BladeCharges * DamagePerCharge;
        BladeCharges = 0;
        // ��������� � � ��
        float f = rt.nam.TimeFromNowToNote(FoundNote);
       
        if (FoundNote == nfg.noteInfo.IdInTrack || f < 0.3f)
        {

            if (!Pressed)
            {
                DoDamageToPlayer(Damage, 0.3f);
                
            }
            g.GetComponent<ProjectileMover>().StepPerSecond = (rt.pa.TargetPoint.transform.position - BladeAttackShootPosition.transform.position) / 0.3f;
        }
        else
        {     
            rt.nam.AttachModToNote(FoundNote, new BossProjectileMod(), new object[] { this, g, Damage });
            g.GetComponent<ProjectileMover>().StepPerSecond = (rt.pa.TargetPoint.transform.position - BladeAttackShootPosition.transform.position) / f;
        }
    }


    
    public override void OnShielded()
    {
        // ����� ������� ����
        ShieldPole.GetComponent<Animator>().SetTrigger("BLINK");
    }
    
    public override void OnTakingDamage()
    {
        if (!IsDead)
        {


            base.OnTakingDamage();
            if (an.GetCurrentAnimatorStateInfo(0).IsName("Taking damage_1"))
            {
                an.SetTrigger("OnTakingDamage");
            }
            else { an.Play("Taking damage_1"); }
        }
    }
    public override void OnDeath()
    {
        Destroy(gameObject, EndDeathTime);
        SpawnDeathEffect(DeathEffectSpawnPoint);
       
        rt.es.DoEnemyEvent(this, EnemyAction.Died);
        IsInvisible = true;
        
        an.Play("Death");
        IsDead = true;
        rt.nt.SliderActionEvent -= SliderAction;
        rt.nt.PhaseChangedEvent -= PhaseChangeAction;
    }
    
        
    
    public override void OnEnemyArrived()
    {
        HealthBar.transform.position = HealthBarSpawnPoint.position;
        HealthBar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }



    public enum BossState 
    {
        Idle,
        Slider,
        BladeSlider
    }
}
