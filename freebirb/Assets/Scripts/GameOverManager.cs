using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text highscoreText;
#pragma warning restore

    private void Start() {
        scoreText.text = !PlayerPrefs.HasKey("latestScore") ? "Score: 0" : "Score: " + PlayerPrefs.GetInt("latestScore");
        highscoreText.text = !PlayerPrefs.HasKey("highscore") ? "Highscore: 0" : "Highscore: " + PlayerPrefs.GetInt("highscore");
    }

    public void RestartGame() {
        SceneManager.LoadScene("MainGameScene");
    }

    public void ReturnToMainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
