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

        ///Without Gamemanager ---------------------------------------
        //PlayerHealth[] players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        //for (int i = 0; i < players.Length; i++)
        //{
        //    string stats = $"Player {players[i].OwnerClientId + 1} — " +
        //                  $"HP: {players[i].CurrentHealth.Value} | " +
        //                  $"Kills: {players[i].EnemiesKilled.Value}";

        //    if (i == 0) player1StatsText.text = stats;
        //    else if (i == 1) player2StatsText.text = stats;
        //}

        //Check if it was a wipe or victory
        //bool anyAlive = false;
        //foreach (var p in players)
        //    if (p.CurrentHealth.Value > 0) anyAlive = true;

        //titleText.text = anyAlive ? "VICTORY" : "DEFEAT";
        ///---------------------------------------------------------

        ///Use stored stats from GameManager
        player1StatsText.text = $"Player 1 — HP: {GameManager.Instance.Player1Health.Value}" +
            $" | Kills: {GameManager.Instance.Player1Kills.Value}";
        player2StatsText.text = $"Player 2 — HP: {GameManager.Instance.Player2Health.Value}" +
            $" | Kills: {GameManager.Instance.Player2Kills.Value}";

        titleText.text = GameManager.Instance.Player1Health.Value <= 0
            && GameManager.Instance.Player2Health.Value <= 0 ? "DEFEAT" : "VICTORY";
    }

    private void ReturnToMainMenu()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}