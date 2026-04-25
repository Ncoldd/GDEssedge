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

    private const float MAX_HEALTH = 100f;

    public void TakeDamage(float damage)
    {
        if (!IsServer) return;
        CurrentHealth.Value = Mathf.Max(0, CurrentHealth.Value - damage);

        //Fire the delegate event so other systems know
        GameEvents.Instance.PlayerTakeDamage(OwnerClientId, damage);
        AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.hurtSound);

        if (CurrentHealth.Value <= 0)
        {
            GameEvents.Instance.PlayerDie(OwnerClientId);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
