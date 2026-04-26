using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class EnemyPool : NetworkBehaviour
{
    public static EnemyPool Instance { get; private set; }

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;

        // Pre-spawn pool of enemies, hide them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.GetComponent<NetworkObject>().Spawn();
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }
    }

    //Get an enemy from the pool
    public GameObject GetEnemy(Vector3 position)
    {
        if (!IsServer) return null;

        if (pool.Count > 0)
        {
            GameObject enemy = pool.Dequeue();
            enemy.transform.position = position;
            enemy.SetActive(true);
            return enemy;
        }

        //Pool empty — spawn a new one
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        newEnemy.GetComponent<NetworkObject>().Spawn();
        return newEnemy;
    }

    // Return enemy to pool instead of destroying
    public void ReturnEnemy(GameObject enemy)
    {
        if (!IsServer) return;
        HideEnemyClientRpc(enemy.GetComponent<NetworkObject>().NetworkObjectId);
        enemy.SetActive(false); // hide on server
        pool.Enqueue(enemy);
    }

    [ClientRpc]
    private void HideEnemyClientRpc(ulong networkObjectId)
    {//SpawnedObjects - dictionary of every active NetworkObject on the network, keyed by their ID
                                                    //TryGetValue looks up that ID
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out NetworkObject obj))
        {
            obj.gameObject.SetActive(false);
        }
    }
}