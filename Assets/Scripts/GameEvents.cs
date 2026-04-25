using UnityEngine;

public class GameEvents : MonoBehaviour
{
    //Singleton //get - others can read//private set - only this instance can write
    public static GameEvents Instance { get; private set; }

    private void Awake()
    {
        //If it awakes and sees a doppelganger, kill it
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Delegates - "event types"
    //defines shape of func
    public delegate void WaveCompleted(int waveNumber);
    public delegate void PlayerDamaged(ulong clientId, float damage);
    public delegate void PlayerDied(ulong clientId);

    //Events - "signals" scripts subscribe to
    public event WaveCompleted OnWaveCompleted;
    public event PlayerDamaged OnPlayerDamaged;
    public event PlayerDied OnPlayerDied;

    //Methods to fire each event
    public void WaveComplete(int waveNumber) => OnWaveCompleted?.Invoke(waveNumber);
    public void PlayerTakeDamage(ulong clientId, float damage) => OnPlayerDamaged?.Invoke(clientId, damage);
    public void PlayerDie(ulong clientId) => OnPlayerDied?.Invoke(clientId);
}
