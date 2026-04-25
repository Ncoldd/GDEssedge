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
    public void TriggerGameOver()
    {
        if (!IsServer) return;
        NetworkManager.Singleton.SceneManager.LoadScene("ResultsScene", LoadSceneMode.Single);
    }

    //Game over - all rounds complete or party wiped
    private void EndGame()
    {
        TriggerGameOver();
    }
}
