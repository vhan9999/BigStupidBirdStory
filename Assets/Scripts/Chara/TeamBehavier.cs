using System;
using UnityEngine;
using UnityEngine.AI;

public enum TeamState
{
    InVillage,
    Advanture,
    BackToVillage,
    Battle
}

public class TeamBehavier : MonoBehaviour
{
    [SerializeField] private TeamState state;
    [SerializeField] private CharaBehaviour charaBehaviour;
    [SerializeField] private EnemyBehaviour enemyBehaviour;

    private NavMeshAgent agent;
    private Vector2 breakPosition;
    private int preparedCount;


    private void Start()
    {
        SetState(TeamState.InVillage);

        breakPosition = Vector2.zero;

        #region agent

        agent = GetComponent<NavMeshAgent>();
        if (agent == null) agent = gameObject.AddComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        #endregion
    }

    private void Update()
    {
        // unity chara
        switch (state)
        {
            case TeamState.InVillage:
                break;
            case TeamState.Advanture:
                // if (Input.GetKeyDown(KeyCode.C)) SetWalkTo(enemyBehaviour.transform.position);
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetMouseButtonDown(0))
                {
                    SetWalkTo(pos);
                }
                else
                {
                    var enemyList = EnemySpawnManage.GetEnemyList();
                    float minDistance = 100;
                    EnemyBehaviour target = null;
                    foreach (var enemy in enemyList)
                    {
                        var distance = Vector2.Distance(enemy.transform.position, charaBehaviour.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            target = enemy;
                        }

                        if (distance < 1)
                        {
                            SetState(TeamState.Battle);
                            break;
                        }
                    }

                    if (target != null) SetWalkTo(target.transform.position);
                }

                break;
            case TeamState.BackToVillage:
                if (Vector2.Distance(charaBehaviour.transform.position, transform.position) < 0.1f)
                    SetState(TeamState.InVillage);
                break;
            case TeamState.Battle:
                charaBehaviour.Attack(enemyBehaviour);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SetState(TeamState newState)
    {
        switch (newState)
        {
            case TeamState.InVillage:
                agent.enabled = false;
                break;
            case TeamState.Advanture:
                agent.enabled = true;
                break;
            case TeamState.BackToVillage:
                SetWalkTo(breakPosition);
                break;
            case TeamState.Battle:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        state = newState;
    }


    public void OnBattleEnd(bool result)
    {
        if (result)
            SetState(TeamState.Advanture);
        else
            SetState(TeamState.BackToVillage);
    }

    private void SetWalkTo(Vector2 pos)
    {
        // agent.SetDestination(pos);
        charaBehaviour.SetWalkTo(pos);
        transform.position = pos;
    }

    public void CharaPrepared()
    {
        preparedCount++;
        if (preparedCount == 1)
        {
            state = TeamState.Advanture;
            preparedCount = 0;
        }
    }

    public void AddMember(CharaBehaviour member)
    {
        // memberList.Add(member);
        charaBehaviour = member;
    }

    #region old

    // public List<CharaBehaviour> memberList = new List<CharaBehaviour>();
    // public GameObject area;
    //
    //
    //
    // [SerializeField] private float vision = 1;
    // private float speed = 1;
    // private NavMeshAgent agent;
    // private PolygonCollider2D areaColli;
    // private EnemySpawnManage areaEnemys;
    // [SerializeField] private bool isRandomWalk = false;
    // [SerializeField] public EnemyBehaviour target;
    // [SerializeField] public BattleManage battleManager;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     SetArea(area);
    //     battleManager = transform.GetComponent<BattleManage>();
    //     state = TeamState.Advanture;
    //     agent = transform.GetComponent<NavMeshAgent>();
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     
    //     if(state == TeamState.InVillage) 
    //     {
    //
    //     }
    //     else if (state == TeamState.Advanture) 
    //     {
    //         if(agent.velocity != Vector3.zero)
    //             agent.speed = speed * GridManage.CalculateOval(agent.velocity);
    //         
    //
    //         if (!isTogether())
    //         {
    //             foreach (CharaBehaviour chara in memberList)
    //                 chara.agent.SetDestination(transform.position);
    //         }
    //         else if (TouchArea(transform.position, areaColli))
    //         {
    //             if (!isRandomWalk)
    //             {
    //                 GoRandomPos();
    //                 isRandomWalk = true;
    //             }
    //                 
    //         }
    //         else
    //         {
    //             agent.SetDestination(new Vector2(-6.04f, -3.21f));
    //         }
    //         Vision();
    //     }
    //     else if (state == TeamState.Battle)
    //     {
    //         battleManager.battleUpdate();
    //     }
    // }
    //
    // public void SetState(TeamState state)
    // {
    //     this.state = state;
    //     switch (state)
    //     {
    //         case TeamState.InVillage:
    //             break;
    //         case TeamState.Advanture:
    //             break;
    //         case TeamState.Battle:
    //             CancelInvoke("GoRandomPos");
    //             break;
    //     }
    // }
    //
    // public void DeleteMember(CharaBehaviour member)
    // {
    //     memberList.Remove(member);
    // }
    //
    // private void Vision()//see enemy
    // {
    //     foreach (EnemyBehaviour enemy in areaEnemys.enemyList)
    //     {
    //         
    //         if (GridManage.CalculateOval(enemy.transform.position - transform.position)
    //             * Vector2.Distance(enemy.transform.position, transform.position)
    //             < vision
    //             && target == null)
    //         {
    //             SetTarget(enemy);
    //             battleManager.AddEnemy(enemy);
    //             state = TeamState.Battle;
    //             break;
    //         }
    //     }
    // }
    //
    // public void SetArea(GameObject a)
    // {
    //     area = a;
    //     areaColli = area.GetComponent<PolygonCollider2D>();
    //     areaEnemys = area.transform.GetChild(0).GetComponent<EnemySpawnManage>();
    // }
    //
    // private void GoRandomPos()//walk around
    // {
    //     while (true)
    //     {
    //         Vector2 randomPos = new Vector2(
    //             transform.position.x + ((UnityEngine.Random.Range(0, 1) * 2 - 1) * UnityEngine.Random.Range(2f, 5f)),
    //             transform.position.y + ((UnityEngine.Random.Range(0, 1) * 2 - 1) * UnityEngine.Random.Range(2f, 5f))
    //             );
    //         if (TouchArea(randomPos, areaColli))
    //         {
    //             foreach(CharaBehaviour chara in memberList)
    //             {
    //                 chara.agent.SetDestination(randomPos);
    //             }
    //             agent.SetDestination(randomPos);
    //             break;
    //         }
    //     }
    //     float waitTime = UnityEngine.Random.Range(6f, 20f);
    //     Invoke("GoRandomPos", waitTime);
    // }
    //
    // private bool TouchArea(Vector2 position, PolygonCollider2D areaCollider)//whether touch area
    // {
    //     if (areaCollider.OverlapPoint(position))
    //         return true;
    //     return false;
    // }
    //
    // private bool isTogether()
    // {
    //     foreach(CharaBehaviour chara in memberList)
    //     {
    //         if(GridManage.CalculateOval(chara.transform.position - transform.position)
    //             * Vector2.Distance(chara.transform.position, transform.position)
    //             >= 0.1f)
    //             return false;
    //     }
    //     return true;
    // }
    //
    // public void SetTarget(EnemyBehaviour enemy)
    // {
    //     target = enemy;
    //     foreach(CharaBehaviour chara in memberList)
    //     {
    //         chara.target = target;
    //     }
    // }

    #endregion
}