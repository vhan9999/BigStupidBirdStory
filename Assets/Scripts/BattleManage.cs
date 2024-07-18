using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BattleManage : MonoBehaviour
{
    public List<EnemyBehaviour> enemyList = new List<EnemyBehaviour>();
    public TeamBehavier teamBehavier;
    public int CharaDeadCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        teamBehavier = transform.GetComponent<TeamBehavier>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEnemy(EnemyBehaviour enemy)
    {
        if (!enemyList.Contains(enemy))
            enemyList.Add(enemy);
    }

    public void DeleteEnemy(EnemyBehaviour enemy)
    {
        enemyList.Remove(enemy);
        if (enemyList.Count > 0)
            teamBehavier.SetTarget(enemyList[0]);
    }
    
    public void battleUpdate()
    {
        if (enemyList.Count == 0)
        {
            teamBehavier.SetState(TeamState.Advanture);
        }
        if (CharaDeadCount == teamBehavier.memberList.Count)
        {
            teamBehavier.SetState(TeamState.InVillage);
            //call alter to get pos
        }
    }
}
