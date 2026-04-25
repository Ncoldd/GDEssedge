using Unity.Netcode;
using UnityEngine;

public class EnemyAI : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    private Transform target;
    private float attackTimer;

    //NetworkVariable so all clients see same health
    public NetworkVariable<float> CurrentHealth = new NetworkVariable<float>(
        50f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    // Update is called once per frame
    private void Update()
    {
        if (!IsServer) return;

        FindClosestPlayer();

        if (target == null) return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            //Move toward closest player
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                moveSpeed * Time.deltaTime
                );
        }
        else
        {
            //Attack if in range and cooldown is done
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    private void FindClosestPlayer()
    {
        PlayerHealth[] players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);
        float closestDist = Mathf.Infinity;

        foreach (PlayerHealth player in players)
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                target = player.transform;
            }
        }
    }

    private void Attack()
    {
        target.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);
    }

    public void TakeDamage(float damage, ulong attackerClientId)
    {
        if (!IsServer) return;
        CurrentHealth.Value = Mathf.Max(0, CurrentHealth.Value - damage);
        FlashClientRpc();

        if (CurrentHealth.Value <= 0)
        {
            //Give kill credit to the attacker
            foreach (var player in FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None))
            {
                if (player.OwnerClientId == attackerClientId)
                {
                    player.AddKill();
                    break;
                }
            }
            GetComponent<NetworkObject>().Despawn();
        }
    }

    //clientRpc tells all clients to flash the enemy red
    [ClientRpc]
    private void FlashClientRpc()
    {
        StartCoroutine(FlashRed());
    }

    private System.Collections.IEnumerator FlashRed()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.13f);
        GetComponent<SpriteRenderer>().color = new Color(0.145f, 0.51f, 0.263f); 
    }
}
