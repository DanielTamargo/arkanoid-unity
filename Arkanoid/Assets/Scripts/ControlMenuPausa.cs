using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenuPausa : MonoBehaviour
{
    public static bool JuegoPausado = false;
    public GameObject menuPausaUI;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
            } else
            {
                Pausar();
            }
        }
    }

    public void CargarMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Saliendo al menú...");
        SceneManager.LoadScene(0);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        if (UnityEditor.EditorApplication.isPlaying) {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    public void Reanudar()
    {
        Debug.Log("Reanudando...");
        menuPausaUI.SetActive(false);
        Time.timeScale = 1f;
        JuegoPausado = false;
    }

    void Pausar()
    {
        menuPausaUI.SetActive(true);
        Time.timeScale = 0f;
        JuegoPausado = true;
    }

}
