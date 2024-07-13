using Enemy.save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

internal enum TeamState
{
    InVillage,
    Advanture,
    Battle
}
public class TeamBehavier : MonoBehaviour
{
    public List<CharaBehaviour> memberList = new List<CharaBehaviour>();
    public List<EnemyBehaviour> enemyList = new List<EnemyBehaviour>();
    public GameObject area;
    
    public int DeadCount = 0;

    [SerializeField] private float vision = 1;
    private PolygonCollider2D areaColli;
    private EnemySpawnManage areaEnemys;
    private TeamState state;
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        state = TeamState.InVillage;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == TeamState.InVillage) 
        {

        }
        else if (state == TeamState.Advanture) 
        {
            Vision();
            //not in area: go area
            //random walk
        }
        else if (state == TeamState.Battle)
        {
            if(enemyList.Count == 0)
            {
                state = TeamState.Advanture;
            }
            if(DeadCount == memberList.Count)
            {
                state = TeamState.InVillage;
            }
        }
    }

    public void AddMember(CharaBehaviour member)
    {
        memberList.Add(member);
    }
    public void AddEnemy(EnemyBehaviour enemy)
    {
        if(!enemyList.Contains(enemy))
            enemyList.Add(enemy);
    }
    public void DeleteMember(CharaBehaviour member)
    {
        memberList.Remove(member);
    }
    public void DeleteEnemy(EnemyBehaviour enemy)
    {
        enemyList.Remove(enemy);
        if(enemyList.Count > 0)
            target = enemyList[0].gameObject;
    }

    private void Vision()//see enemy
    {
        foreach (EnemyBehaviour enemy in areaEnemys.enemyList)
        {
            if (GridManage.CalculateOval(enemy.transform.position - transform.position)
                * Vector2.Distance(enemy.transform.position, transform.position)
                < vision
                && target == null)
            {
                CancelInvoke("GoRandomPos");
                target = enemy.gameObject;
                AddEnemy(enemy);
                state = TeamState.Battle;
                break;
            }
        }
    }

    public void SetArea(GameObject a)
    {
        area = a;
        areaColli = area.GetComponent<PolygonCollider2D>();
        areaEnemys = area.transform.GetChild(0).GetComponent<EnemySpawnManage>();
    }
}
