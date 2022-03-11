using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _currentScoreText;
    [SerializeField] private Text _bestScoreText;


    void Update()
    {
        _currentScoreText.text = "Score: " + GameManager.Instance.CurrentScore;
        _bestScoreText.text = "Best: " + GameManager.Instance.BestScore;
    }
}
