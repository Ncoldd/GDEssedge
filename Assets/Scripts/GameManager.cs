using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    //Singleton instance so only one GameManager exists
    public static GameManager Instance { get; private set; }

    //Network vars sync across clients
    public NetworkVariable<int> CurrentRound = new NetworkVariable<int>(1);
    public NetworkVariable<int> CurrentWave = new NetworkVariable<int>(1);
    public NetworkVariable<int> PlayersAlive = new NetworkVariable<int>(0);

    private const int MAX_ROUNDS = 3;
    private const int MAX_WAVES = 3;

    private void Awake()
    {
        //If an instance already exists destroy this one, otherwise set it
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); //persist between scenes
    }

    //Called by server to Advance Wave
    public void AdvanceWave()
    {
        if (!IsServer) return;

        if (CurrentWave.Value >= MAX_WAVES)
        {
            AdvanceRound();
        }
        else
        {
            CurrentWave.Value++;
        }
    }

    //Called when all waves are complete "if (CurrentWave.Value >= MAX_WAVES)"
    private void AdvanceRound()
    {
        if (CurrentRound.Value >= MAX_ROUNDS)
        {
            EndGame();
        }
        else 
        {
            CurrentRound.Value++;
            CurrentWave.Value = 1; //Reset wave count for new round

            NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", LoadSceneMode.Single);
        }
    }

    //Game over - all rounds complete or party wiped
    private void EndGame()
    {
        TriggerGameOver();
    }

    //Store stats before despawning

    ///non network var version-------
    //public static int Player1Kills;
    //public static int Player1Health;
    //public static int Player2Kills;
    //public static int Player2Health;
    ///-----------------------------

    public NetworkVariable<int> Player1Kills = new NetworkVariable<int>(0);
    public NetworkVariable<int> Player1Health = new NetworkVariable<int>(0);
    public NetworkVariable<int> Player2Kills = new NetworkVariable<int>(0);
    public NetworkVariable<int> Player2Health = new NetworkVariable<int>(0);

    public void TriggerGameOver()
    {
        if (!IsServer) return;

        //Save stats first
        PlayerHealth[] players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        if (players.Length > 0) 
        {
            Player1Kills.Value = players[0].EnemiesKilled.Value; 
            Player1Health.Value = (int)players[0].CurrentHealth.Value;
        }

        if (players.Length > 1) 
        { 
            Player2Kills.Value = players[1].EnemiesKilled.Value;
            Player2Health.Value = (int)players[1].CurrentHealth.Value;
        }

        //Then despawn
        foreach (var enemy in FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        {
            EnemyPool.Instance.ReturnEnemy(enemy.gameObject);
        }

        foreach (var player in FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None))
            player.GetComponent<NetworkObject>().Despawn();

        NetworkManager.Singleton.SceneManager.LoadScene("ResultsScene", LoadSceneMode.Single);
    }
}
