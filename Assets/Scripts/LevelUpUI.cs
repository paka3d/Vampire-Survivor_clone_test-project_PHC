using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LevelUpUI : MonoBehaviour
{
    public GameObject panel;
    public Button speedButton;
    public Button healthButton;
    public Button ammoButton;
    public VideoPlayer videoPlayer; // Assign in Inspector
    public VideoClip[] levelUpVideos; // Assign different mp4 clips in Inspector

    private PlayerAgentController player;

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);

        speedButton.onClick.AddListener(IncreaseSpeed);
        healthButton.onClick.AddListener(RechargeHealth);
        ammoButton.onClick.AddListener(ReloadAmmo);
    }

    public void ShowLevelUpOptions(PlayerAgentController pc)
    {
        Debug.Log("Level Up UI Activated!");
        player = pc;
        if (panel != null)
            panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Play a random video from the array
        if (videoPlayer != null && levelUpVideos.Length > 0)
        {
            int index = Random.Range(0, levelUpVideos.Length);
            videoPlayer.clip = levelUpVideos[index];
            videoPlayer.Play();
        }
    }

    private void Hide()
    {
        if (panel != null)
            panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (videoPlayer != null)
            videoPlayer.Stop();
    }

    private void IncreaseSpeed()
    {
        player.moveSpeed += 1f;
        Hide();
    }

    private void RechargeHealth()
    {
        player.SetHealth(100);
        Hide();
    }

    private void ReloadAmmo()
    {
        player.Reload();
        Hide();
    }
}
