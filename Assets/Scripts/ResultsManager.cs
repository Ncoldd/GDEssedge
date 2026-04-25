using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI player1StatsText;
    [SerializeField] private TextMeshProUGUI player2StatsText;
    [SerializeField] private Button mainMenuBtn;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioManager.Instance.lobbyMusic);
        mainMenuBtn.onClick.AddListener(ReturnToMainMenu);

        PlayerHealth[] players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        for (int i = 0; i < players.Length; i++)
        {
            string stats = $"Player {players[i].OwnerClientId + 1} — " +
                          $"HP: {players[i].CurrentHealth.Value} | " +
                          $"Kills: {players[i].EnemiesKilled.Value}";

            if (i == 0) player1StatsText.text = stats;
            else if (i == 1) player2StatsText.text = stats;
        }

        //Check if it was a wipe or victory
        bool anyAlive = false;
        foreach (var p in players)
            if (p.CurrentHealth.Value > 0) anyAlive = true;

        titleText.text = anyAlive ? "VICTORY" : "DEFEAT";
    }

    private void ReturnToMainMenu()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}