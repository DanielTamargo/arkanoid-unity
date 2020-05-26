using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMainMenu : MonoBehaviour
{

    public void Jugar()
    {
        GameObject overlay = GameObject.Find("Overlay");
        overlay.GetComponent<ControlOverlay>().pasarNivel();
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
