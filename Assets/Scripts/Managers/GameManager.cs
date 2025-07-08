using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TileCounter tileCounter;
    [SerializeField] private int maxTilesBeforeGameOver = 40;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject gameOverPanel;


    private readonly List<PlayerTile> activeTiles = new();
    private bool isGameOver = false;
    private bool hasWon = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Application.targetFrameRate = 60;

        if (winPanel != null) winPanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void RegisterTile(PlayerTile tile)
    {
        if (isGameOver) return;

        if (tile.GetValue() == 2048)
        {
            HandleWin();
        }

        activeTiles.Add(tile);
        tile.OnTileDestroyed += UnregisterTile;

        UpdateTileUI();
        CheckTileCount();
    }

    public void UnregisterTile(BaseTile tile)
    {
        if (tile is PlayerTile playerTile)
        {
            activeTiles.Remove(playerTile);
            UpdateTileUI();
        }
    }

    private void CheckTileCount()
    {
        if (hasWon) return;

        if (activeTiles.Count > maxTilesBeforeGameOver)
        {
            TriggerGameOver();
        }
    }

    private void HandleWin()
    {
        if (isGameOver || hasWon) return;

        isGameOver = true;
        hasWon = true;
        Time.timeScale = 0f;

        if (winPanel != null)
        {
            int score = ScoreManager.Instance?.GetScore() ?? 0;
            scoreText.text = $"Score: {score}";
            winPanel.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;

        if (winPanel != null)
            winPanel.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        hasWon = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        SoundManager.Instance?.PlaySFX(SoundManager.Instance.gameOverClip);
        if (gameOverPanel != null)
        {
            int score = ScoreManager.Instance?.GetScore() ?? 0;
            if (scoreText != null)
                scoreText.text = $"Score: {score}";
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    private void UpdateTileUI()
    {
        tileCounter?.UpdateTileCount(activeTiles.Count, maxTilesBeforeGameOver);
    }
}
