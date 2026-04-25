using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : NetworkBehaviour
{
    [SerializeField] private float attackDamage = 25f;
    [SerializeField] private float attackRange = 1.5f;

    private void Update()
    {
        if (!IsOwner) return;

        //Left click to attack
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            AttackServerRpc();
        }
    }

    [ServerRpc]
    private void AttackServerRpc()
    {
        //Find all enemies within attack range

        //array of all game objects that have the "EnemyAI" script 
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        //foreach loop to go through the array
        foreach (EnemyAI enemy in enemies)
        {
            //float var that takes the distance between the player position and an enemy's
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            //if within distance
            if (dist <= attackRange)
            {
                //give damage
                enemy.TakeDamage(attackDamage);
                AudioManager.Instance.PlaySoundEffect(AudioManager.Instance.attackSound);
            }
        }
    }
}