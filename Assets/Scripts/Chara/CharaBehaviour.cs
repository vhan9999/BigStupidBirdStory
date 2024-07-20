using System;
using System.Collections.Generic;
using Player.save;
using UnityEngine;
using UnityEngine.AI;

internal enum CharaState
{
    InVillage,
    InTeam
}

internal enum CharaInVillageState
{
    Idle,
    GoToUnity,
    Prepared
}

internal enum CharaInTeamState
{
    Idle,
    Walk,
    Tired,
    Hungry,

    // Run,
    Attack,

    // Skill,
    Die
}

public class CharaBehaviour : MonoBehaviour
{
    public CharaData charaData;

    [SerializeField] private CharaInTeamState inTeamState;
    [SerializeField] private CharaInVillageState inVillageState;
    [SerializeField] private CharaState state;
    public Dictionary<Item,int> bag = new Dictionary<Item, int>();

    public GameObject attackTarget;

    public NavMeshAgent agent;

    public Transform trans;

    public EnemyBehaviour target;

    public TeamBehavier team;
    private bool isAllowAttack = true;

    private void Start()
    {
        charaData = new CharaData
        {
            name = "Chara1",
            battleData = new BattleData
            {
                atk = 1,
                atkspd = 5,
                movspd = 1,
                range = 0.1f,
                vision = 1
            },
            energy = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            hp = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            hunger = new MaxableNumber
            {
                now = 100,
                max = 100
            },
            mood = new MaxableNumber
            {
                now = 100,
                max = 100
            }
        };
        inTeamState = CharaInTeamState.Idle;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        state = CharaState.InVillage;
        inVillageState = CharaInVillageState.Idle;
    }

    private void Update()
    {
        switch (state)
        {
            case CharaState.InVillage:
                UpdateInVillage();
                break;
            case CharaState.InTeam:
                UpdateInTeam();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        // if (charaData.hp.now <= 0)
        // {
        //     SetState(CharaState.Die);
        // }
        // else if (target != null)
        // {
        //     if (AtkRange())
        //     {
        //         agent.SetDestination(transform.position);
        //         SetState(CharaState.Attack);
        //     }
        //     else
        //     {
        //         agent.SetDestination(target.transform.position);
        //         SetState(CharaState.Walk);
        //     }
        // }
        //
        //
        //
        // if (state == CharaState.Idle)
        // {
        //     if (agent.velocity != Vector3.zero)
        //     {
        //         SetState(CharaState.Walk);
        //     }
        // }
        // else if (state == CharaState.Walk)
        // {
        //     if (agent.velocity != Vector3.zero)
        //         agent.speed = charaData.battleData.movspd * GridManage.CalculateOval(agent.velocity);
        //     else
        //     {
        //         SetState(CharaState.Idle);
        //     }
        //
        // }
        // else if (state == CharaState.Attack)
        // {
        //     if (isAllowAttack)
        //     {
        //         target.getDamage(charaData.battleData.atk);
        //         isAllowAttack = false;
        //         Invoke("AttackCooldown", charaData.battleData.atkspd);
        //     }
        // }
    }


    private void UpdateInTeam()
    {
    }

    private void UpdateInVillage()
    {
        switch (inVillageState)
        {
            case CharaInVillageState.Idle:
                if (Math.Abs(charaData.hp.now - charaData.hp.max) < 0.1)
                    SetInViilageState(CharaInVillageState.GoToUnity);
                break;
            case CharaInVillageState.GoToUnity:
                if (agent.speed == 0) SetInViilageState(CharaInVillageState.Prepared);
                break;
            case CharaInVillageState.Prepared:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetInViilageState(CharaInVillageState newState)
    {
        switch (newState)
        {
            case CharaInVillageState.Idle:
                break;
            case CharaInVillageState.GoToUnity:
                SetWalkTo(team.transform.position);
                break;
            case CharaInVillageState.Prepared:
                team.CharaPrepared();
                state = CharaState.InTeam;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        inVillageState = newState;
    }

    public void SetWalkTo(Vector2 pos)
    {
        agent.SetDestination(pos);
    }

    public void SetCharaData(CharaData charaData)
    {
        this.charaData = charaData;
    }

    private void SetState(CharaInTeamState inTeamState)
    {
        this.inTeamState = inTeamState;
        switch (inTeamState)
        {
            case CharaInTeamState.Idle:
                break;
            case CharaInTeamState.Walk:
                break;
            case CharaInTeamState.Die:
                break;
        }
    }

    public void ToMovePoint()
    {
        // SetWalkTo(new Vector2(-5.2f, -3.65f));
    }

    private bool AtkRange()
    {
        if (GridManage.CalculateOval(target.transform.position - transform.position)
            * Vector2.Distance(target.transform.position, transform.position)
            < charaData.battleData.range)
            return true;
        return false;
    }

    private void AttackCooldown()
    {
        isAllowAttack = true;
    }


    public void getDamage(float damage)
    {
        charaData.hp.now -= damage;
    }
}