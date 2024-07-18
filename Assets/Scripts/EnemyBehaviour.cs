using System.Collections.Generic;
using Enemy.save;
using Player.save;
using UnityEngine;
using UnityEngine.AI;

internal enum EnemyState
{
    Idle,
    Walk,
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
        state = EnemyState.Idle;
        GoRandomPos();
    }

    // Update is called once per frame
    private void Update()
    {
        if (targetChara != null)
        {
            if (AtkRange())
            {
                agent.SetDestination(transform.position);
                SetState(EnemyState.Attack);
            }
            else
            {
                agent.SetDestination(targetChara.transform.position);
                SetState(EnemyState.Walk);
            }
        }
        else
        {
            Vision();
        }

        if (enemyData.hp.now <= 0)
        {
            Destroy(gameObject);
        }
        else if (state == EnemyState.Idle)
        {
            if (agent.velocity != Vector3.zero) SetState(EnemyState.Walk);
        }
        else if (state == EnemyState.Walk)
        {
            if (agent.velocity != Vector3.zero)
                agent.speed = enemyData.battleData.movspd * GridManage.CalculateOval(agent.velocity);
            else
                SetState(EnemyState.Idle);
        }
        else if (state == EnemyState.Attack)
        {
            if (isAllowAttack)
            {
                targetChara.getDamage(enemyData.battleData.atk);
                if (targetChara.charaData.hp.now <= 0)
                {
                    targetChara = null;
                    SetState(EnemyState.Walk);
                }

                isAllowAttack = false;
                Invoke("AttackCooldown", enemyData.battleData.atkspd);
            }
        }
    }

    private void GoRandomPos() //walk around
    {
        while (true)
        {
            var randomPos = new Vector2(transform.position.x + Random.Range(-1f, 1f),
                transform.position.y + Random.Range(-1f, 1f));
            if (TouchArea(randomPos, areaColli))
            {
                agent.SetDestination(randomPos);
                break;
            }
        }

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
            case EnemyState.Idle:
                break;
            case EnemyState.Walk:
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Attack:
                break;
        }
    }

    private void Vision() //see chara
    {
        foreach (var chara in CharaManage.CharaList) //TODO enemy list
            if (GridManage.CalculateOval(chara.transform.position - transform.position)
                * Vector2.Distance(chara.transform.position, transform.position)
                < enemyData.battleData.vision
                && targetChara == null)
            {
                CancelInvoke("GoRandomPos");
                targetChara = chara;
                // chara.team.battleManager.AddEnemy(this);
                break;
                //TODO: cal hatred
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