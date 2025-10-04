using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Animator ButtonAnimator;
    [SerializeField] Animator TouchScreenAnimator;
    [SerializeField] Animator GameTitle;
    [SerializeField] GameObject TouchToStart;

    private int SceneInt;
    public void setInt (int value)
    {
        SceneInt = value;
    }
    private void SwitchScene(int sceneIndex)
    {
        switch (sceneIndex)
        {
            case 0:
                SceneManager.LoadScene("Shop Menu");
                break;

            case 1:
                SceneManager.LoadScene("Power Menu");
                break;

            case 2:
                SceneManager.LoadScene("Settings Menu");
                break;

            case 3:
                SceneManager.LoadScene("H.Score Menu");
                break;

            case 4:
                SceneManager.LoadScene("GamePlay");
                break;

            default:
                Debug.LogWarning("Invalid scene index: " + sceneIndex);
                break;
        }
    }

    public void CloseMenu()
    {
        StartCoroutine(ClosingSequence());
    }

    public IEnumerator ClosingSequence()
    {
        TouchScreenAnimator.Play("Touch gone");
        ButtonAnimator.Play("ButtonMove");
        GameTitle.Play("Title Move");

        yield return new WaitForSeconds(0.5f);

        TouchToStart.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        SwitchScene(SceneInt);
    }

    public void QuitGame()
    {
        // This will only work in a built game
        Application.Quit();

    }
}
