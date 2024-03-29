﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private Text highscoreText;
#pragma warning restore

    void Start() {
        highscoreText.text = !PlayerPrefs.HasKey("highscore") ? "Highscore: 0" : "Highscore: " + PlayerPrefs.GetInt("highscore");
        AudioManager.instance.Play("Theme");
    }

    public void StartGame() {
        SceneManager.LoadScene("MainGameScene");
        AudioManager.instance.Stop("Theme");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
