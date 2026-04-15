using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    //[SerializeField] private allows you to edit in inspector
    //but doesnt let other scripts access and mod the var
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI playersAliveText;

    // Update is called once per frame
    void Update()
    {
        //Pull live values from GameManager and display them
        if (GameManager.Instance == null) return;

        //TextMeshProUGUI is a component, and .text is the property that shows what's displayed
        //Since they are network variables, must use .Value to get the number inside
        roundText.text = $"Round: {GameManager.Instance.CurrentRound.Value}";
        waveText.text = $"Wave: {GameManager.Instance.CurrentRound.Value}";
        playersAliveText.text = $"Players Alive: {GameManager.Instance.PlayersAlive.Value}";
    }
}
