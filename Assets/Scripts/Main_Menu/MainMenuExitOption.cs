using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuExitOption : MonoBehaviour
{
    private void Update()
    {
        ExitApplicationOutSideUnityEditor();

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

}
