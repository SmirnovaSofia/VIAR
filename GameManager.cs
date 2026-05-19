using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject player;
    public GameObject enemy;

    public Transform[] spawnPoints;
    public GameObject pointPrefab;

    public TextMeshProUGUI scoreText;

    private int collected = 0;
    private GameObject currentPoint;

    private bool gameRunning = false;
    private bool isPaused = false;

    // 🧠 ДОБАВЛЯЕМ СОСТОЯНИЕ ИГРЫ
    private bool gameEnded = false;

    void Start()
    {
        Time.timeScale = 1f;

        SetMenu(true);
        SetGame(false);

        UpdateScoreUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HandleEscape();
        }
    }

    // 🎮 ESC ЛОГИКА (ГЛАВНАЯ ЧАСТЬ)
    void HandleEscape()
    {
        // 💀🏆 ЕСЛИ ИГРА ЗАКОНЧЕНА → РЕСТАРТ
        if (gameEnded)
        {
            RestartGame();
            return;
        }

        // 🟢 ЕСЛИ ИГРА ЕЩЁ НЕ ЗАПУЩЕНА → СТАРТ
        if (!gameRunning)
        {
            StartGame();
            return;
        }

        // ⏸ ИНАЧЕ → ПАУЗА
        isPaused = !isPaused;

        SetMenu(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // 🎮 СТАРТ
    public void StartGame()
    {
        gameRunning = true;
        gameEnded = false;
        isPaused = false;
        collected = 0;

        SetMenu(false);
        SetGame(true);

        Time.timeScale = 1f;

        UpdateScoreUI();

        if (currentPoint != null)
            Destroy(currentPoint);

        SpawnPoint();
    }

    // 🔁 РЕСТАРТ ЧЕРЕЗ СЦЕНУ
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 🎯 СБОР ТОЧКИ
    public void CollectPoint()
    {
        collected++;

        UpdateScoreUI();

        if (currentPoint != null)
            Destroy(currentPoint);

        if (collected >= 3)
        {
            EndGame(true);
            return;
        }

        SpawnPoint();
    }

    void SpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        if (pointPrefab == null) return;

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        currentPoint = Instantiate(pointPrefab, spawn.position, Quaternion.identity);
    }

    // 💀 ПРОИГРЫШ
    public void EnemyCaughtPlayer()
    {
        EndGame(false);
    }

    // 🧠 КОНЕЦ ИГРЫ
    void EndGame(bool win)
    {
        gameRunning = false;
        gameEnded = true; // 💥 ВОТ ЭТО ВАЖНО
        isPaused = false;

        Time.timeScale = 1f;

        if (currentPoint != null)
            Destroy(currentPoint);

        SetMenu(true);
        SetGame(false);

        Debug.Log(win ? "YOU WIN 🎉" : "YOU LOSE 💀");

        UpdateScoreUI();
    }

    // 🧩 UI
    void SetMenu(bool state)
    {
        if (menuPanel != null)
            menuPanel.SetActive(state);
    }

    void SetGame(bool state)
    {
        if (player != null) player.SetActive(state);
        if (enemy != null) enemy.SetActive(state);
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Collected: " + collected + " / 3";
    }
}