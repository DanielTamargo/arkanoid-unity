using System;
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
    private string nombreJugador;

    private bool loveEnMarcha = false;
    private bool love = false;

    // Objeto donde mostramos el texto
    public GameObject o_puntuacion;
    public GameObject o_nivel;
    public GameObject o_vidas;
    public GameObject o_jugador;
    public GameObject panel;
    public GameObject input_jugador;

    public GameObject o_gameover;
    public GameObject o_nivelsuperado;
    public GameObject o_hasganado;

    private TextMeshProUGUI t_puntuacion;
    private TextMeshProUGUI t_nivel;
    private TextMeshProUGUI t_vidas;
    private TextMeshProUGUI t_jugador;
    private TextMeshProUGUI t_input_jugador;

    public static ControlOverlay instance;

    private bool activo = false;

    public void test() 
    {
        if (!activo)
        {
            panel.SetActive(true);
            StartCoroutine(animacionNivelSuperado());
            activo = true;
        }
        else
        {
            intentoFallido();
        }
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
        t_vidas = o_vidas.GetComponent<TextMeshProUGUI>();
        t_jugador = o_jugador.GetComponent<TextMeshProUGUI>();
        t_input_jugador = input_jugador.GetComponent<TextMeshProUGUI>();
        pasarNivel();
    }

    // Update is called once per frame
    void Update()
    {
        t_nivel.text = "Nivel: " + nivel.ToString();
        t_puntuacion.text = "Puntos: " + puntos.ToString();
        t_vidas.text = "Vidas: " + vidas.ToString();
        //t_jugador.text = nombreJugador;
    }

    public void intentoFallido()
    {
        vidas -= 1;
        puntos = puntosPreNivel;
        o_vidas.SetActive(false);
        o_vidas.SetActive(true);
        //pararMusica();

        if (vidas <= 0)
        {
            // Reseteamos valores
            vidas = 3;
            puntos = 0;
            puntosPreNivel = 0;
            nivel = -1;
            FindObjectOfType<AudioManager>().StopAll();
            //TODO falta guardar registro en la BBDD

            FindObjectOfType<AudioManager>().Play("sfx_lose");

            pasarNivel();
        } else
        {
            DontDestroyOnLoad(this);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void animacion(int anim)
    {
        if (anim == 0)
            StartCoroutine(animacionGameOver());
        else if (anim == 1)
            StartCoroutine(animacionNivelSuperado());
        else
            StartCoroutine(animacionHasGanado());

    }

    IEnumerator animacionGameOver()
    {
        o_gameover.SetActive(true);
        yield return new WaitForSeconds(3);
        o_gameover.SetActive(false);
    }

    IEnumerator animacionNivelSuperado()
    {
        o_nivelsuperado.SetActive(true);
        yield return new WaitForSeconds(3);
        o_nivelsuperado.SetActive(false);
    }

    IEnumerator animacionHasGanado()
    {
        o_hasganado.SetActive(true);
        yield return new WaitForSeconds(3);
        o_hasganado.SetActive(false);
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
        if (string.Equals(nombreJugador, "love", System.StringComparison.OrdinalIgnoreCase))
            FindObjectOfType<AudioManager>().Stop("bg_especial");

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

    public void configurarNombre()
    {
        nombreJugador = t_input_jugador.text;
        if (nombreJugador.Length < 3)
            nombreJugador = "Guest";
        else if (nombreJugador.Length > 16)
            nombreJugador = nombreJugador.Substring(0, 16);
        t_jugador.text = nombreJugador.Trim();

        if (nombreJugador.ToLower().CompareTo("love") == 1)
        {
            love = true;
        }
        /* Todos estos ifs (y más) no me funcionaban bien :) (: :) (: :) (: :) (:
        if (nombreJugador.Trim().ToLower() == "love")
        {
            love = true;
        }
        if (nombreJugador.Equals("love", StringComparision.OrdinalIgnoreCase))
        {
            love = true;
        }
        if (nombreJugador.ToLower() == "love")
        {
            love = true;
        }
        if (nombreJugador.Equals("love"))
        {
            love = true;
        }
        if (nombreJugador is "love")
        {
            love = true;
        }*/
        pasarNivel();
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
                ponerMusicaLove();
                Debug.Log("Cargando nivel 1...");
                DontDestroyOnLoad(this);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 2:
                //Parar canción 1
                FindObjectOfType<AudioManager>().Stop("bg_nivel_1");
                FindObjectOfType<AudioManager>().Play("bg_nivel_2");
                //ponerMusicaLove();
                //Cargar escena nivel 2
                Debug.Log("Cargando nivel 2...");
                DontDestroyOnLoad(this);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 3:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_2");
                FindObjectOfType<AudioManager>().Play("bg_nivel_3");
                //ponerMusicaLove();
                //cargamos la escena
                Debug.Log("Cargando nivel 3...");
                DontDestroyOnLoad(this);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 4:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_3");
                FindObjectOfType<AudioManager>().Play("bg_nivel_4");
                //ponerMusicaLove();
                //cargamos la escena
                Debug.Log("Cargando nivel 4...");
                DontDestroyOnLoad(this);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 5:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_4");
                //FindObjectOfType<AudioManager>().Play("bg_especial");
                //ponerMusicaLove();
                //TODO Falta guardar en la bbdd
                //TODO falta mostrar mensaje fin del juego
                nivel = -1;
                puntos = 0;
                puntosPreNivel = 0;
                vidas = 3;
                pasarNivel();
                break;
        }
    }

    void ponerMusicaLove()
    {
        if (love)
        {
            if (!loveEnMarcha)
            {
                Debug.Log("Eres Love? Eres Love! y... What is Love?");
                loveEnMarcha = true;
                pararMusica();
                FindObjectOfType<AudioManager>().Play("bg_especial");
            }
        }
    }

}
