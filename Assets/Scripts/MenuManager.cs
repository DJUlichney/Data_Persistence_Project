using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
# if UNITY_EDITOR
    using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static string playerName;

    // Gets the player's name input. The name input is supplied as a parameter by the text field object.
    public void UpdateName(string nameInput){
        playerName = nameInput;
        //Debug.Log(playerName);
    }

    public void StartGame(){
        SceneManager.LoadScene(1);
    }

    public void ExitGame(){
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();

        #else
            Application.Quit();

        #endif
    }
}
