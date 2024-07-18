using System.Collections.Generic;
using UnityEngine;

public class BattleManage : MonoBehaviour
{
    public List<EnemyBehaviour> enemyList = new();
    public TeamBehavier teamBehavier;

    public int CharaDeadCount;

    // Start is called before the first frame update
    private void Start()
    {
        teamBehavier = transform.GetComponent<TeamBehavier>();
    }

    // Update is called once per frame
    private void Update()
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
        // if (enemyList.Count > 0)
        //     teamBehavier.SetTarget(enemyList[0]);
    }

    public void battleUpdate()
    {
        if (enemyList.Count == 0)
        {
            // teamBehavier.SetState(TeamState.Advanture);
        }
        // if (CharaDeadCount == teamBehavier.memberList.Count)
        // {
        //     teamBehavier.SetState(TeamState.InVillage);
        //     //call alter to get pos
        // }
    }
}