using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressStart : MonoBehaviour
{
    private bool firstPush = false;

    public void GameStart()
    {
        if (!firstPush)
        {
            SceneManager.LoadScene("SampleStage");
            firstPush = true;
        }
    }
}
