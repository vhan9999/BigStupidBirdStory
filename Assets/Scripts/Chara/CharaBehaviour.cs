using Enemy.save;
using Player.save;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

internal enum CharaState
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

    [SerializeField] private CharaState state;

    [SerializeField] private Vector2 walkTarget;

    public GameObject attackTarget;

    public NavMeshAgent agent;

    public Transform trans;

    private GameObject target = null;
    private bool isAllowAttack = true;

    private void Start()
    {
        charaData = new CharaData
        {
            name = "Chara1",
            battleData = new BattleData
            {
                atk = 10,
                atkspd = 10,
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
        state = CharaState.Idle;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (target != null)
        {
            if (AtkRange())
            {
                agent.SetDestination(transform.position);
                SetState(CharaState.Attack);
            }
            else
            {
                agent.SetDestination(target.transform.position);
                SetState(CharaState.Walk);
            }
        }


        if (charaData.hp.now <= 0)
        {
            SetState(CharaState.Die);
        }
        else if (state == CharaState.Idle)
        {
            if (agent.velocity != Vector3.zero)
            {
                SetState(CharaState.Walk);
            }
        }
        else if (state == CharaState.Walk)
        {
            if (agent.velocity != Vector3.zero)
                agent.speed = charaData.battleData.movspd * GridManage.CalculateOval(agent.velocity);
            else
            {
                SetState(CharaState.Idle);
            }

        }
        else if (state == CharaState.Attack)
        {
            if (isAllowAttack)
            {
                target.GetComponent<CharaBehaviour>().getDamage(charaData.battleData.atk);
                isAllowAttack = false;
                Invoke("AttackCooldown", charaData.battleData.atkspd);
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

    private void Vision()//see chara
    {
        foreach (CharaBehaviour chara in CharaManage.CharaList)//TODO enemy list
        {
            if (GridManage.CalculateOval(chara.transform.position - transform.position)
                * Vector2.Distance(chara.transform.position, transform.position)
                < charaData.battleData.vision
                && target == null)
            {
                CancelInvoke("GoRandomPos");
                target = chara.gameObject;
                break;
                //TODO: cal hatred
            }
        }
    }

    private bool AtkRange()
    {
        if (GridManage.CalculateOval(target.transform.position - transform.position)
                * Vector2.Distance(target.transform.position, transform.position)
                < charaData.battleData.range)
            return true;
        else
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