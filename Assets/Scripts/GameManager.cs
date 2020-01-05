using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;


    private void Start()
    {
        _isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        ExitApplicationOutSideUnityEditor();

        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            int LoadGameLevel = 0;

            SceneManager.LoadScene(LoadGameLevel);
        }
    }

    private static void ExitApplicationOutSideUnityEditor()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNTIY_EDITOR
                return;
            #elif UNTIY_STANDALONE_WIN
                Application.Quit();
            #endif
        }
    }

    public void SetGameOver()
    {
        _isGameOver = true;
    }
}
