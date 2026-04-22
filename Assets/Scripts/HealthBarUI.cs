using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar1;
    [SerializeField] private Slider healthBar2;
    [SerializeField] private TextMeshProUGUI playerLabel1;
    [SerializeField] private TextMeshProUGUI playerLabel2;

    private PlayerHealth[] players;

    // Update is called once per frame
    void Update()
    {
        //Find all players in the scene
        players = FindObjectsByType<PlayerHealth>(FindObjectsSortMode.None);

        for (int i = 0; i < players.Length; i++)
        {
            if (i == 0)
            {
                healthBar1.value = players[i].CurrentHealth.Value;
                playerLabel1.text = $"Player {players[i].OwnerClientId + 1}";
            }
            else if (i == 1)
            {
                healthBar2.value = players[i].CurrentHealth.Value;
                playerLabel2.text = $"Player {players[i].OwnerClientId + 1}";
            }
        }
    }
}
