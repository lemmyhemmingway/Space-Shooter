using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _liveImg;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _resetText;

    private GameManager _gameManager;

    private void Start()
    {
        _scoreText.text = "SCORE: " + 0;
        _gameOverText.gameObject.SetActive(false);
        _resetText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager null");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "SCORE: " + playerScore;
    }

    public void UpdateLives(int currentLive)
    {
        _liveImg.sprite = _liveSprites[currentLive];

        if (currentLive == 0)
        {
            GameOverSeq();
        }
    }

    private IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void GameOverSeq()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _resetText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }
}