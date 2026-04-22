using UnityEngine;
using Unity.Netcode;

public class WaveSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int enemiesPerWave = 5;

    private int enemiesAlive = 0;
    private bool waveActive = false;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        StartWave();
    }

    public void StartWave()
    {
        if (!IsServer) return;
        waveActive = true;
        enemiesAlive = enemiesPerWave;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            //Cycle through spawn points
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            enemy.GetComponent<NetworkObject>().Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer || !waveActive) return;

        //Count living enemies
        enemiesAlive = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None).Length;

        if (enemiesAlive <= 0)
        {
            waveActive = false;
            //Tell GameManager wave is done
            GameManager.Instance.AdvanceWave();
            GameEvents.Instance.WaveComplete(GameManager.Instance.CurrentWave.Value);
        }
    }
}
