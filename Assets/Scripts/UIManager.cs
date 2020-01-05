using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;


    [SerializeField] private Sprite[] _livesSprites;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartGameText;
    [SerializeField] private GameManager _gameManager;


    void Start()
    {
        _scoreText.text = "Score: 0";
        
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL, this should never happen");
        }

    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = $"Score: {playerScore}";

    }

    public void UpdateLives(int currentLives)
    {
        if(currentLives < 0)
        {
            currentLives = 0;
        }

        _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives <= 0)
        {
            GameOverSequence();
        }

    }

    private void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);

        _gameManager.SetGameOver();

        StartCoroutine(GameOverFlickerText());

    }

    IEnumerator GameOverFlickerText()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";

            yield return new WaitForSeconds(0.5f);

            _gameOverText.text = "";

            yield return new WaitForSeconds(0.5f);
        }

    }

}
