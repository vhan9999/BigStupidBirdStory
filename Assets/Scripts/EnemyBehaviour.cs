using System;
using System.Collections.Generic;
using DefaultNamespace;
using Enemy.save;
using Player.save;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

internal enum EnemyState
{
    NotAttack,
    Attack,
    Die
}

public class EnemyBehaviour : MonoBehaviour
{
    public PolygonCollider2D areaColli;
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private EnemyState state;
    private NavMeshAgent agent;
    private Dictionary<GameObject, int> hateList = new();
    private bool isAllowAttack = true;
    private CharaBehaviour targetChara;
    private TeamBehavier targetTeamBehavier;

    // Start is called before the first frame update
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        enemyData = new EnemyData
        {
            name = "Enemy1",
            battleData = new BattleData
            {
                atk = 10,
                atkspd = 5,
                movspd = 1,
                range = 0.1f,
                vision = 1
            },
            hp = new MaxableNumber
            {
                now = 100,
                max = 100
            }
        };
        state = EnemyState.NotAttack;
        GoRandomPos();
    }

    // Update is called once per frame
    private void Update()
    {
        // make target chara to be the nearest chara in vision


        if (enemyData.hp.now <= 0)
            // Destroy(gameObject);
            gameObject.SetActive(false);
        if (agent.velocity != Vector3.zero)
            agent.speed = enemyData.battleData.movspd * GridManage.CalculateOval(agent.velocity);

        switch (state)
        {
            case EnemyState.NotAttack:
                var teamBehaviers = GameManager.GetTeamList();
                foreach (var teamBehavier in teamBehaviers)
                {
                    var visionDistance = 1; // todo: make this static or something
                    // dalan todo: use 1:2 distance
                    if (Vector2.Distance(teamBehavier.transform.position, transform.position) < visionDistance)
                    {
                        //target team 
                        targetChara = teamBehavier.GetMonsterAttackChara();
                        targetTeamBehavier = teamBehavier;
                        SetState(EnemyState.Attack);
                        break;
                    }
                }

                break;
            case EnemyState.Attack:

                if (targetChara == null) SetState(EnemyState.NotAttack);

                if (AtkRange())
                    if (isAllowAttack)
                    {
                        targetChara.getDamage(enemyData.battleData.atk);
                        if (targetChara.charaData.hp.now <= 0) targetChara = null;

                        isAllowAttack = false;
                        Invoke("AttackCooldown", enemyData.battleData.atkspd);
                    }
                // else
                // {
                //     dalan todo: go closer
                //     dalan todo: monster cant go village
                //     agent.SetDestination(targetChara.transform.position);
                // }

                break;
            case EnemyState.Die:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    // private void Vision() //see chara
    // {
    //     foreach (var chara in CharaManage.CharaList) //TODO enemy list
    //         if (GridManage.CalculateOval(chara.transform.position - transform.position)
    //             * Vector2.Distance(chara.transform.position, transform.position)
    //             < enemyData.battleData.vision
    //             && targetChara == null)
    //         {
    //             CancelInvoke("GoRandomPos");
    //             targetChara = chara;
    //             // chara.team.battleManager.AddEnemy(this);
    //             break;
    //             //TODO: cal hatred
    //         }
    // }

    private void GoRandomPos() //walk around
    {
        var randomPos = new Vector2(transform.position.x + Random.Range(-1f, 1f),
            transform.position.y + Random.Range(-1f, 1f));
        if (TouchArea(randomPos, areaColli)) agent.SetDestination(randomPos);

        var waitTime = Random.Range(2f, 13f);
        Invoke("GoRandomPos", waitTime);
    }


    private bool TouchArea(Vector2 position, PolygonCollider2D areaCollider) //whether touch area
    {
        if (areaCollider.OverlapPoint(position))
            return true;
        return false;
    }

    private void SetState(EnemyState state)
    {
        this.state = state;
        switch (state)
        {
            case EnemyState.NotAttack:
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Attack:
                break;
        }
    }


    private bool AtkRange()
    {
        if (GridManage.CalculateOval(targetChara.transform.position - transform.position)
            * Vector2.Distance(targetChara.transform.position, transform.position)
            < enemyData.battleData.range)
            return true;
        return false;
    }

    private void AttackCooldown()
    {
        isAllowAttack = true;
    }

    public void getDamage(float damage)
    {
        enemyData.hp.now -= damage;
    }
}