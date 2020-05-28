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

    // Textos HUD
    public GameObject o_puntuacion;
    public GameObject o_nivel;
    public GameObject o_vidas;
    public GameObject o_jugador;
    public GameObject panel;
    public GameObject input_jugador;

    // Textos niveles superados / fallidos
    public GameObject o_gameover;
    public GameObject o_nivelsuperado;
    public GameObject o_hasganado;

    // Textos cuenta atrás comenzar nivel en móvil
    public GameObject o_tocaParaEmpezar;
    public GameObject o_3;
    public GameObject o_2;
    public GameObject o_1;
    public GameObject o_go;

    private TextMeshProUGUI t_puntuacion;
    private TextMeshProUGUI t_nivel;
    private TextMeshProUGUI t_vidas;
    private TextMeshProUGUI t_jugador;
    private TextMeshProUGUI t_input_jugador;

    public static ControlOverlay instance;

    private int testeito = 0;

    public void test() 
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (testeito == 0)
                o_tocaParaEmpezar.SetActive(true);
            else
            {
                o_tocaParaEmpezar.SetActive(false);
                StartCoroutine(animacionComenzarPartida());
            }

            testeito++;
        }

    }

    IEnumerator animacionComenzarPartida()
    {
        o_tocaParaEmpezar.SetActive(false);
        o_3.SetActive(true);
        yield return new WaitForSeconds(1);
        o_3.SetActive(false);
        o_2.SetActive(true);
        yield return new WaitForSeconds(1);
        o_2.SetActive(false);
        o_1.SetActive(true);
        yield return new WaitForSeconds(1);
        o_1.SetActive(false);
        o_go.SetActive(true);
        yield return new WaitForSeconds(1);
        o_go.SetActive(false);
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
        // Si no estamos en el móvil, le decimos que tiene que empezar pulsando la barra espaciadora
        if (Application.platform != RuntimePlatform.Android || Application.platform != RuntimePlatform.IPhonePlayer)
        {
            o_tocaParaEmpezar.GetComponent<TextMeshProUGUI>().text = "Pulsa la barra espaciadora para empezar";
            o_tocaParaEmpezar.GetComponent<TextMeshProUGUI>().fontSize = o_tocaParaEmpezar.GetComponent<TextMeshProUGUI>().fontSize - 10;
        }
        panel.SetActive(false);

        //input_jugador.GetComponent<TextMeshProUGUI>().maxVisibleCharacters = 5;
        
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
        //t_nivel.text = "Nivel: " + nivel.ToString(); // lo traslado al metodo pasarNivel(); para no actualizarlo innecesariamente
        t_puntuacion.text = "Puntos: " + puntos.ToString();
        t_vidas.text = "Vidas: " + vidas.ToString();
        //t_jugador.text = nombreJugador;
    }

    public void intentoFallido()
    {
        if (vidas > 0)
        {
            puntos = puntosPreNivel;
            vidas -= 1;
            o_vidas.SetActive(false);
            o_vidas.SetActive(true);
        }
        
        //pararMusica();

        if (vidas <= 0)
        {
            // Guardamos el registro cuando se acaban las vidas
            FindObjectOfType<BaseDeDatos>().nuevaPuntuacion(puntos, nombreJugador);

            // Reseteamos valores
            vidas = 3;
            puntos = 0;
            puntosPreNivel = 0;
            nivel = -1;
            FindObjectOfType<AudioManager>().StopAll();

            // Volvemos al menú (nivel -1 + pasarNivel = menú)
            pasarNivel();
        } else
        {
            DontDestroyOnLoad(this);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void animacionVidas()
    {
        vidas -= 1;
        o_vidas.SetActive(false);
        o_vidas.SetActive(true);
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
        if (nombreJugador.Length > 5)
            nombreJugador = nombreJugador.Substring(0, 5);
        else
            nombreJugador = "Guest";

        t_jugador.text = nombreJugador;

        if (nombreJugador.ToLower().Contains("love"))
            love = true;
        else
            love = false;

        /* Todos estos ifs (y más) no me funcionaban bien :) (: :) (: :) (: :) (:
        if (nombreJugador.ToLower().CompareTo("love") == 1)
            love = true;
        if (nombreJugador.Trim().ToLower() == "love")
            love = true;
        if (nombreJugador.Equals("love", StringComparision.OrdinalIgnoreCase))
            love = true;
        if (nombreJugador.ToLower() == "love")
            love = true;
        if (nombreJugador.Equals("love"))
            love = true;
        if (nombreJugador is "love")
            love = true;
        */
        pasarNivel();
    }

    public void pasarNivel()
    {
        puntosPreNivel = puntos;
        nivel += 1;
        t_nivel.text = "Nivel: " + nivel.ToString();
        panel.SetActive(true);
        switch(nivel)
        {
            case 0:
                panel.SetActive(false);
                FindObjectOfType<AudioManager>().Play("bg_nivel_0");
                if (SceneManager.GetActiveScene().buildIndex != 0)
                {
                    DontDestroyOnLoad(this);
                    SceneManager.LoadScene(0);
                }
                break;
            case 1:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_0");
                FindObjectOfType<AudioManager>().Play("bg_nivel_1");
                ponerMusicaLove();
                Debug.Log("Cargando nivel 1...");
                DontDestroyOnLoad(this);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 2:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_1");
                FindObjectOfType<AudioManager>().Play("bg_nivel_2");

                Debug.Log("Cargando nivel 2...");
                DontDestroyOnLoad(this);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 3:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_2");
                FindObjectOfType<AudioManager>().Play("bg_nivel_3");

                Debug.Log("Cargando nivel 3...");
                DontDestroyOnLoad(this);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 4:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_3");
                FindObjectOfType<AudioManager>().Play("bg_nivel_4");

                Debug.Log("Cargando nivel 4...");
                DontDestroyOnLoad(this);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 5:
                FindObjectOfType<AudioManager>().Stop("bg_nivel_4");

                // Guardamos el registro cuando se pasa el nivel
                FindObjectOfType<BaseDeDatos>().nuevaPuntuacion(puntos, nombreJugador);
                
                //Reseteamos datos
                nivel = -1;
                puntos = 0;
                puntosPreNivel = 0;
                vidas = 3;
                pasarNivel();
                break;
        }
    }

    public void escenaPuntuaciones()
    {
        FindObjectOfType<AudioManager>().Stop("bg_nivel_0");
        FindObjectOfType<AudioManager>().Play("bg_puntuaciones");
        SceneManager.LoadScene(5);
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
