using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    //NetworkVariable so all clients see the same health value
    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    //Tracks enemies killed by this player
    public NetworkVariable<int> EnemiesKilled = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private const float MAX_HEALTH = 100f;
    //private bool isDead = false;

    public void TakeDamage(float damage)
    {
        if (!IsServer) return;
        CurrentHealth.Value = Mathf.Max(0, CurrentHealth.Value - damage);

        //Fire the delegate event so other systems know
        GameEvents.Instance.PlayerTakeDamage(OwnerClientId, damage);
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.hurtSound);

        if (CurrentHealth.Value <= 0)
        {
            //isDead = true;
            GameEvents.Instance.PlayerDie(OwnerClientId);
            GameManager.Instance.PlayersAlive.Value--;
            CheckWipeCondition();
        }
    }
    public void AddKill()
    {
        if (!IsServer) return;
        EnemiesKilled.Value++;
    }
    private void CheckWipeCondition()
    {
        if (GameManager.Instance.PlayersAlive.Value <= 0)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}
