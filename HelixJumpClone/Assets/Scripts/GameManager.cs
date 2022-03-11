using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private HelixSetup helixSetup;

    [SerializeField] BallBounce ballBounce;

    public int CurrentScore;
    public int BestScore;

    public int currentStage = 0;

    public float levelProgress;

    public static GameManager Instance;

    public bool gameOver = false;
    [SerializeField] private GameObject _gameOverPanel;

    public Color ballColor = Color.white;

    public bool mute = false;

    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _currentLevelText;
    [SerializeField] TextMeshProUGUI _nextLevelText;

    private bool _inGame;

    [SerializeField] private GameObject _inGamePanel;
    [SerializeField] private GameObject _settingPanel;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        BestScore = PlayerPrefs.GetInt("Highscore");

        Advertisement.Initialize("4443573");
    }

    public void UpdateSlide()
    {
        levelProgress = ballBounce.totalRingsPassed * 100 / helixSetup.RingsInStage;

        _slider.value = levelProgress;

        _currentLevelText.text = currentStage.ToString();
        _nextLevelText.text = (currentStage + 1).ToString();

    }

    public void NextLevel()
    {
        helixSetup.LoadStage(++currentStage);

    }
    public void SetColor(Color colorToSet)
    {
        ballColor = colorToSet;

        ballBounce._trail.material.color = GameManager.Instance.ballColor;

        ballBounce._trailSuper.startColor = GameManager.Instance.ballColor;
        ballBounce._trailSuper.endColor = GameManager.Instance.ballColor;

        _slider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = ballColor;

    }
    public void RestartLevel()
    {
        Advertisement.Show();

        helixSetup.LoadStage(currentStage);
    }
    public void AddScore(int pointsToAdd)
    {
        CurrentScore += pointsToAdd;

        if (CurrentScore > BestScore)
        {
            BestScore = CurrentScore;
            PlayerPrefs.SetInt("Highscore", BestScore);
        }
    }

    private void Update()
    {
        if (_inGame == false && Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            _inGame = true;
            _settingPanel.SetActive(false);
            _inGamePanel.SetActive(true);
        }


        if (gameOver)
        {
            _gameOverPanel.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                RestartLevel();
                gameOver = false;
                _gameOverPanel.SetActive(false);
            }

        }
    }
}
