using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlOverlay : MonoBehaviour
{
    // Puntos ganados en la partida, nivel y vidas
    public int puntos = 0;
    public int puntosPreNivel = 0;
    public int nivel = -1;
    public int vidas = 3;
    public string nombreJugador = "Guest";

    // Objeto donde mostramos el texto
    public GameObject o_puntuacion;
    public GameObject o_nivel;
    public GameObject panel;
    public GameObject input_jugador;

    private TextMeshProUGUI t_puntuacion;
    private TextMeshProUGUI t_nivel;
    private TextMeshProUGUI t_vidas;
    private TextMeshProUGUI t_jugador;
    private TextMeshProUGUI t_input_jugador;

    public static ControlOverlay instance;

    public void test() 
    {
        Debug.Log(t_input_jugador.text);
        pasarNivel();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        panel.SetActive(false);
        t_nivel = o_nivel.GetComponent<TextMeshProUGUI>();
        t_puntuacion = o_puntuacion.GetComponent<TextMeshProUGUI>();
        t_input_jugador = input_jugador.GetComponent<TextMeshProUGUI>();
        pasarNivel();
    }

    // Update is called once per frame
    void Update()
    {
        t_nivel.text = "Nivel: " + nivel.ToString();
        t_puntuacion.text = "Puntos: " + puntos.ToString();
    }

    public void intentoFallido()
    {
        vidas -= 1;
        puntos = puntosPreNivel;

        pararMusica();

        if (vidas <= 0)
        {
            // Reseteamos valores
            vidas = 3;
            puntos = 0;
            puntosPreNivel = 0;
            nivel = -1;

            //TODO falta guardar registro en la BBDD


            pasarNivel();
        } else
        {
            DontDestroyOnLoad(this);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void mostrarPanel()
    {
        panel.SetActive(true);
    }

    public void ocultarPanel()
    {
        panel.SetActive(false);
    }

    private void pararMusica()
    {
        switch (nivel)
        {
            case 1:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_1");
                break;
            case 2:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_2");
                break;
            case 3:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_3");
                break;
            case 4:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_4");
                break;
        }
    }

    public void pasarNivel()
    {
        puntosPreNivel = puntos;
        nivel += 1;
        panel.SetActive(true);
        switch(nivel)
        {
            case 0:
                panel.SetActive(false);
                FindObjectOfType<AudioManager>().Play("bg_nivel_0");
                if (SceneManager.GetActiveScene().buildIndex != 0)
                    SceneManager.LoadScene(0);
                break;
            case 1:
                //Reproducir canción 1
                FindObjectOfType<AudioManager>().Stop("bg_nivel_0");
                FindObjectOfType<AudioManager>().Play("bg_nivel_1");

                //DontDestroyOnLoad(this);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;

            case 2:
                //Parar canción 1
                FindObjectOfType<AudioManager>().Stop("bg_nivel_1");
                FindObjectOfType<AudioManager>().Play("bg_nivel_2");

                //Cargar escena nivel 2
                DontDestroyOnLoad(this);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //borrar esta y descomentar la de abajo
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;

            case 3:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_2");
                FindObjectOfType<AudioManager>().Play("bg_nivel_3");

                //cargamos la escena

                break;
            case 4:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_3");
                FindObjectOfType<AudioManager>().Play("bg_nivel_4");

                //cargamos la escena
                break;
            case 5:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_4");
                FindObjectOfType<AudioManager>().Play("bg_especial");
                //guardar en la bbdd
                //mostramos mensaje fin del juego
                //nivel = -1
                //pasarNivel();
                break;
            default:

                break;
        }
    }

}
