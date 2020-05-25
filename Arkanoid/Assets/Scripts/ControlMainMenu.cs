using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMainMenu : MonoBehaviour
{

    void Start()
    {
        
    }

    public void Jugar()
    {
        
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

}
