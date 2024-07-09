using Player.save;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavier : MonoBehaviour
{
    public PolygonCollider2D areaColli;
    private NavMeshAgent agent;
    [SerializeField] private BattleData charaData;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        GoRandomPos();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void GoRandomPos()
    {
        while(true)
        {
            Vector2 randomPos = new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f));
            if (TouchBuilding(randomPos, areaColli))
            {
                agent.SetDestination(randomPos);
                break;
            }
        }
        float waitTime = Random.Range(2f, 13f);
        Invoke("GoRandomPos", waitTime);
    }


    private bool TouchBuilding(Vector2 position, PolygonCollider2D areaCollider)
    {
        if (areaCollider.OverlapPoint(position))
            return true;
        return false;
    }
}
