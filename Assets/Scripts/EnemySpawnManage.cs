using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnManage : MonoBehaviour
{
    public static List<EnemyBehaviour> enemyList = new();
    [SerializeField] private EnemyBehaviour enemyPrefab;
    [SerializeField] private GameObject enemyParent;
    private readonly int maxCount = 5;
    private readonly List<GameObject> spawnPointList = new();

    private void Start()
    {
        foreach (Transform child in transform) spawnPointList.Add(child.gameObject);
        InvokeRepeating("RepeatCheckList", 1f, 1f);

        for (var i = 0; i < maxCount; i++) Spawn();
    }
    // Start is called before the first frame update

    public static List<EnemyBehaviour> GetEnemyList()
    {
        return enemyList;
    }

    public void Spawn()
    {
        var randomPoint = Random.Range(0, spawnPointList.Count);
        var randomPos = new Vector2(spawnPointList[randomPoint].transform.position.x + Random.Range(-3f, 3f),
            spawnPointList[randomPoint].transform.position.y + Random.Range(-3f, 3f));
        var enemy = Instantiate(enemyPrefab, enemyParent.transform);
        enemy.transform.position = randomPos;
        enemy.areaColli = transform.parent.GetComponent<PolygonCollider2D>();
        enemyList.Add(enemy);
    }

    private void RepeatCheckList() //and spawn
    {
        // enemyList.RemoveAll(enemy => enemy == null);
        // if (enemyList.Count < maxCount) Spawn();
        foreach (var e in enemyList)
            if (!e.GameObject().gameObject.activeSelf)
                e.GameObject().gameObject.SetActive(!e.GameObject().gameObject.activeSelf);
    }
}