using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    public Button StartButton;

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    private void OnEnable()
    {
        StartButton.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        StartButton.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        Time.timeScale = 1f;

        // Hides the button
        StartButton.gameObject.SetActive(false);
    }
}
