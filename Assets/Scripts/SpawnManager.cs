using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

public class SpawnManager : NetworkBehaviour
{
    //array of spawn points set in the Unity Inspector
    [SerializeField] private Transform[] spawnPoints;

    //called when this object spawns on the network
    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayer(clientId);
        }

        // Set players alive to actual connected count
        GameManager.Instance.PlayersAlive.Value = NetworkManager.Singleton.ConnectedClientsIds.Count;

    }

    //runs every time a new client joins
    private void OnClientConnected(ulong clientId)
    {
        SpawnPlayer(clientId);
    }
    private void SpawnPlayer(ulong clientId)
    {
        int index = (int)(clientId % (ulong)spawnPoints.Length);
        NetworkObject playerObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(clientId);

        if (playerObj == null)
        {
            //Player was despawned, respawn them
            GameObject player = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab,
                spawnPoints[index].position, Quaternion.identity);
            player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
        else
        {
            playerObj.transform.position = spawnPoints[index].position;
        }
    }
}
