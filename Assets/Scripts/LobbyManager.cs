using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : NetworkBehaviour
    {

    [SerializeField] private Button readyUpBtn;

    //track how many players are ready
    private NetworkVariable<int> playersReady = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        readyUpBtn.onClick.AddListener(OnReadyUp);
    }

    private void OnReadyUp()
    {
        ReadyUpServerRpc();
    }

    //Rpc = remote procedure call //"when any client calls this method, run it on the server instead" 
    [Rpc(SendTo.Server)]
    private void ReadyUpServerRpc()
    {
        playersReady.Value++;

        //If all connected players are ready, load HuntScene
        if (playersReady.Value >= NetworkManager.Singleton.ConnectedClientsIds.Count)
        {
            playersReady.Value = 0;
            NetworkManager.Singleton.SceneManager.LoadScene("HuntScene", LoadSceneMode.Single);
            AudioManager.Instance.PlayMusic(AudioManager.Instance.huntMusic);
        }
    }
}
