using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class ReturnMenu : MonoBehaviour
{
    [SerializeField] Animator menuAnim;
    public void BackToMenu(float Secs)
    {
        StartCoroutine(loadSecs(Secs));
    }

    public IEnumerator loadSecs(float newSec)
    {
        if (menuAnim != null)
        {
            menuAnim.SetBool("Exit", true);
        }

        yield return new WaitForSeconds(newSec);
        SceneManager.LoadScene("Menu");
    }
}
