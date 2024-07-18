using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManage : MonoBehaviour
{ 
    private List<GameObject> spawnPointList = new List<GameObject>();
    public List<EnemyBehaviour> enemyList = new List<EnemyBehaviour>();
    private int maxCount = 35;
    [SerializeField] private EnemyBehaviour enemyPrefab;
    [SerializeField] private GameObject enemyParent;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            spawnPointList.Add(child.gameObject);
        }
        InvokeRepeating("RepeatCheckList", 1f, 1f);
    }

    public void Spawn()
    {
        int randomPoint = Random.Range(0, spawnPointList.Count);
        Vector2 randomPos = new Vector2(spawnPointList[randomPoint].transform.position.x + Random.Range(-3f,3f), spawnPointList[randomPoint].transform.position.y + Random.Range(-3f, 3f));
        EnemyBehaviour enemy = Instantiate(enemyPrefab, randomPos, transform.rotation);
        enemy.transform.SetParent(enemyParent.transform);
        enemy.areaColli = transform.parent.GetComponent<PolygonCollider2D>();
        enemyList.Add(enemy);
    }

    private void RepeatCheckList()//and spawn
    {
        enemyList.RemoveAll(enemy => enemy == null);
        if(enemyList.Count < maxCount)
        {
            Spawn();
        }
    }
}
