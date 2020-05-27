using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMainMenu : MonoBehaviour
{

    public void Jugar()
    {
        GameObject overlay = GameObject.Find("Overlay");
        overlay.GetComponent<ControlOverlay>().configurarNombre();
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

}
