using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool isPlayerAlive = true;
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("SpawnEnemies");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemies()
    {
        while (isPlayerAlive)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(-10f, 10f), 7f, 0), Quaternion.identity);
            yield return new WaitForSeconds(4f);
        }

    }

}
