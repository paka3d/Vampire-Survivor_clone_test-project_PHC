using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI scoreText;

    private PlayerAgentController player;
    private GameManager gm;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAgentController>();
        gm = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        healthText.text = $"Health: {player.CurrentHealth}";
        ammoText.text   = $"Ammo : {player.CurrentAmmo}";
        scoreText.text  = $"Score: {gm.score}";
    }
}
