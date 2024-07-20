using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleManage : MonoBehaviour
{
    private readonly List<CharaBehaviour> charaBehaviours = new();
    // public List<EnemyBehaviour> enemyList = new();
    // public TeamBehavier teamBehavier;

    // public int CharaDeadCount;

    private UnityAction<bool> onBattleEnd;

    // Start is called before the first frame update
    private void Start()
    {
        // teamBehavier = transform.GetComponent<TeamBehavier>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) onBattleEnd?.Invoke(true);

        if (Input.GetKeyDown(KeyCode.B)) onBattleEnd?.Invoke(false);
    }

    public void AddOnBattleEnd(UnityAction<bool> action)
    {
        onBattleEnd += action;
    }

    public void OnBattleStart(CharaBehaviour charaBehaviour)
    {
        charaBehaviours.Add(charaBehaviour);
    }

    public void AddEnemy(EnemyBehaviour enemy)
    {
        // if (!enemyList.Contains(enemy))
        //     enemyList.Add(enemy);
    }

    public void DeleteEnemy(EnemyBehaviour enemy)
    {
        // enemyList.Remove(enemy);
        // if (enemyList.Count > 0)
        //     teamBehavier.SetTarget(enemyList[0]);
    }

    public void battleUpdate()
    {
        // if (enemyList.Count == 0)
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