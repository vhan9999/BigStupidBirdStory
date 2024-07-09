using Player.save;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

internal enum CharaState
{
    Idle,
    Walk,

    // Run,
    Attack,
    // Skill,
    Die
}

public class CharaBehavier : MonoBehaviour
{
    [SerializeField] private CharaData charaData;

    [SerializeField] private CharaState state;

    [SerializeField] private Vector2 walkTarget;

    public GameObject attackTarget;

    public NavMeshAgent agent;

    public Transform trans;

    private void Start()
    {
        charaData = new CharaData
        {
            name = "Chara1",
            battleData = new BattleData
            {
                atk = 10,
                def = 10,
                cri = 10,
                movspd = 1,
                dodge = 10
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
        state = CharaState.Idle;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (charaData.hp.now <= 0)
        {
            SetState(CharaState.Die);
        }
        else
        {
            
            if (state == CharaState.Idle)
            {
                if (agent.velocity != Vector3.zero)
                {
                    SetState(CharaState.Walk);
                }
            }
        }

        if (state == CharaState.Walk)
        {
            if (agent.velocity == Vector3.zero)
                SetState(CharaState.Idle);
            else
            {
                Debug.Log(agent.velocity);
                agent.speed = charaData.battleData.movspd * GridManage.CalculateOval(agent.velocity);
            }
            
        }
    }

    public void SetWalkTo(Vector2 pos)
    {
        walkTarget = pos;
        agent.SetDestination(pos);
    }

    public void SetCharaData(CharaData charaData)
    {
        this.charaData = charaData;
    }

    private void SetState(CharaState state)
    {
        this.state = state;
        switch (state)
        {
            case CharaState.Idle:
                break;
            case CharaState.Walk:
                break;
            case CharaState.Die:
                break;
        }
    }

    public void ToMovePoint()
    {
        SetWalkTo(new Vector2(-5.2f, -3.65f));
    }
}