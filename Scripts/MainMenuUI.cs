using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{                                                 
    [SerializeField] GameObject buttonsPanel = default;
    [SerializeField] GameObject instructionsPanel = default;
    [SerializeField] GameObject creditsPanel = default;
    [SerializeField] GameObject rulesPanel = default;
    [SerializeField] GameObject controlPanel = default;

    public void NewGame()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu") {
            SceneManager.LoadScene("level1");
        }
    }

    public void BackButton()
    {
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }

    public void Instructions()
    {
        instructionsPanel.SetActive(true);
        creditsPanel.SetActive(false);
        buttonsPanel.SetActive(false);

        controlPanel.SetActive(false);
        rulesPanel.SetActive(true);
    }

    public void Credits()
    {
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(true);
        buttonsPanel.SetActive(false);
    }

    public void InstructionsToControls()
    {
        controlPanel.SetActive(true);
        rulesPanel.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
