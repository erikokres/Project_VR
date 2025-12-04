using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
   public void ChangeScene(string nameScene)
    {
        SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
