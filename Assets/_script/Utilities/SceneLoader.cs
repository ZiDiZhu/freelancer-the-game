using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //sets object active for seconds then quit
    public void ExitGame(GameObject toSetActive)
    {
        toSetActive.SetActive(true);
        WaitForSecondsRealtime(2f);
        Debug.Log("Quitting app");
        Application.Quit();
    }

    private void WaitForSecondsRealtime(float v)
    {
        throw new NotImplementedException();
    }
}
