using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Repairer : Enemy
{

    public bool deltapr;
    public bool IsRepairing 
    {
        get { return isRepairing; }
        set 
        {
            if (isRepairing != value)
            {
                isRepairing = value;
                if (isRepairing)
                {
                    deltaRepairEffect = Instantiate(RepairEffect, RepairEffectSpawn);
                    if (!an.GetCurrentAnimatorStateInfo(0).IsName("Repair_Start") && !an.GetCurrentAnimatorStateInfo(0).IsName("Repairing"))
                    {
                        PlayAnim("Repair_start");
                    }
                }
                else
                {
                    if (an.GetCurrentAnimatorStateInfo(0).IsName("Repair_Start") || an.GetCurrentAnimatorStateInfo(0).IsName("Repairing"))
                    {
                        PlayAnim("Repair_End");
                    }
                    if (deltaRepairEffect != null)
                    { 
                        Destroy(deltaRepairEffect);
                    }
                }
            }
           

        }
    }
    public bool isRepairing;

    public bool RepairOtherEnemy;
    public Enemy RepairTarget;

    public int HealAmount;

    public int FoundedPos;
    public bool IsMoving;

    [Header("������ ������")]
    public GameObject RepairEffect;
    public GameObject deltaRepairEffect;
    public Transform RepairEffectSpawn;

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 1)
        {
            deltapr = Pressed;
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {        
                DotsCheck();
            }
            if (nfg.noteInfo.type == NoteType.Note)
            {
                Action();
            }

        }
    }
    public new void Start()
    {
        base.Start();
        rt.nt.PhaseChangedEvent += SwitchPhaseAction;
    }
    public override void DotsAction()
    {
        Action();
       
    }
    public void SwitchPhaseAction(bool IsGGPhase)
    {
        if (IsGGPhase)
        {
            EndRepair();
        }
    }
  
    public void StartRepair() // ��������� �������� �������
    {
        IsRepairing = true;
       
        IsMoving = false;
    }
    public void EndRepair() // �������� ��������� �������
    {
        IsRepairing = false;
       
    }
    public void Repairing() // �������� �������
    {
        if (IsRepairing && !deltapr)
        {
            if (RepairTarget != null)
            {
                if (Health <= MaxHealth)
                {
                    RepairTarget.Health += HealAmount * DamageMultiplayer;
                }
                
                
            }
            else
            {
                if (Health <= MaxHealth)
                {
                    Health += HealAmount * DamageMultiplayer;
                }
                
            }
        }
    }
   
    public void Moving(int Pos)
    {
        IsMoving = true;
        an.Play("Moving");
        // ������ ����������� �� ������ ������� � �����
        MoveToNewPosition(0.66f, Pos);
    }
    public void Action()
    {

        if (RepairTarget == null) // ���� ���� ���, �� �������� � �����. ���� ����, �� �����.
        {

            if (TryToFindTarget(out FoundedPos))
            {
                // ���� ��������� � ������ ����� ���, �� �������� ������. ���� ���, �� ������������ � �������� ������
                Moving(FoundedPos);
                Invoke("StartRepair", 0.66f);
            }
            else 
            {
                if (!IsMoving)
                {
                    if (!IsRepairing)
                    {
                        StartRepair();
                    }
                    else
                    {
                        Repairing();
                    }
                }
                    
                
               
            }
        }
        else 
        {
            if (!IsMoving)
            {
                if (!IsRepairing)
                {
                    StartRepair();
                }
                else
                {
                    Repairing();
                }
            }
            
        }
    }
    public bool TryToFindTarget(out int FoundedPos) // �������� ����� ����, ���� ����� ���� true, � ����� ���������� � � ����
    {
        FoundedPos = 0;
        for (int i = 1; i < 7; i++)
        {
            if (i != 3 && i != 4)
            {
                if (rt.es.EnemiesOnPositions[i] != null && rt.es.EnemiesOnPositions[i] != gameObject && (rt.es.EnemiesOnPositions[i + 1] == null || rt.es.EnemiesOnPositions[i + 1] == gameObject))
                {
                    FoundedPos = i+1;
                    RepairTarget = rt.es.EnemiesOnPositions[i].GetComponent<Enemy>();
                    return true;
                }
            }
        }
        
        return false;
    }
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking FinalDamage", "TakingDamage");
    }
    public override void OnBirth()
    {
        base.OnBirth();

    }
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");
        IsInvisible = true;
        if (deltaRepairEffect != null)
        {
            Destroy(deltaRepairEffect);
        }
    }
    public override void LoadOptions()
    {
        base.LoadOptions();
        HealAmount = rt.oh.so.RepairerPlusHP;    
    }

}
