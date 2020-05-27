using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenuPausa : MonoBehaviour
{
    public static bool JuegoPausado = false;
    public GameObject menuPausaUI;


    private void Start()
    {
        //por posibles bugs que hayan pasado en algún nivel
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                //FindObjectOfType<AudioManager>().Play("sfx_pause_out");
                Reanudar();
            } else
            {
                FindObjectOfType<AudioManager>().Play("sfx_pause_in");
                Pausar();
            }
        }
    }

    public void CargarMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<AudioManager>().StopAll();
        Debug.Log("Saliendo al menú...");
        Reanudar();
        Destroy(GameObject.FindGameObjectWithTag("Overlay"));
        SceneManager.LoadScene(0);
    }

    public void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }

    public void Reanudar()
    {
        FindObjectOfType<AudioManager>().Play("sfx_pause_out");
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
