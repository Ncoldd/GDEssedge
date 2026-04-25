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
            //add to the current wave value to update UI
            int nextWave = GameManager.Instance.CurrentWave.Value + 1;

            if (nextWave > 3)
            {
                //if all 3 waves ended, call AdvanceWave(), which will handle loading the lobby
                GameManager.Instance.AdvanceWave();
                AudioManager.Instance.PlayMusic(AudioManager.Instance.lobbyMusic);
            }
            else
            {
                //still call AdvanceWave() to advance wave, but it wont load lobby because next is !> 3
                GameManager.Instance.AdvanceWave();
                //enemy spawn logic
                StartWave();
            }
        }
    }
}
