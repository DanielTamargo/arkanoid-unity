using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlPuntuaciones : MonoBehaviour
{
    public GameObject anterior;
    public GameObject siguiente;

    public GameObject bestScore;
    public GameObject puntuaciones_pos;
    public GameObject puntuaciones_nombre;
    public GameObject puntuaciones_puntos;

    private List<Puntuacion> listaPuntuaciones = new List<Puntuacion>();
    private int pagina = 1;

    // Start is called before the first frame update
    void Start()
    {
        //anterior.GetComponent<TextMeshProUGUI>().enabled = false; //<- error
        //anterior.GetComponent<TextMeshPro>().enabled = false; //<- error
        anterior.SetActive(false);
        actualizarPuntuaciones();
    }

    // Update is called once per frame
    void Update()
    {
        actualizarPuntuaciones();
    }

    public void botonAnterior()
    {
        pagina--;
        actualizarPuntuaciones();
        siguiente.SetActive(true);

        if (pagina <= 1)
        {
            anterior.SetActive(false);
            bestScore.SetActive(false);
        }
    }

    public void botonSiguiente()
    {
        pagina++;
        bestScore.SetActive(false);
        actualizarPuntuaciones();
        anterior.SetActive(true);
        if ((pagina * 6)  + 1>= listaPuntuaciones.Count)
            siguiente.SetActive(false);

    }

    public void volver()
    {
        FindObjectOfType<AudioManager>().Stop("bg_puntuaciones");
        SceneManager.LoadScene(0);
    }

    void actualizarPuntuaciones()
    {
        listaPuntuaciones = FindObjectOfType<BaseDeDatos>().listaPuntuaciones;
        string texto_posiciones = "";
        string texto_nombres = "";
        string texto_puntos = "";
        if (listaPuntuaciones.Count > 0)
        {
            int posicion = 1 + (6 * (pagina - 1));
            int objetivo = posicion + 6;
            for(int i = posicion; i < objetivo; i++)
            {
                if (listaPuntuaciones.Count > i)
                {
                    Puntuacion puntuacion = listaPuntuaciones[i];
                    string nombre = puntuacion.nombre;
                    string puntos = puntuacion.puntos.ToString();
                    string posicionStr = posicion.ToString();
                    texto_posiciones += posicionStr + "\n";
                    texto_nombres += nombre + "\n";
                    texto_puntos += puntos + "\n";
                    posicion++;
                }
            }
        } else
        {
            texto_posiciones = "\n\nActualmente no hay registros o hay problemas con la base de datos";
        }
        puntuaciones_pos.GetComponent<TextMeshProUGUI>().text = texto_posiciones;
        puntuaciones_nombre.GetComponent<TextMeshProUGUI>().text = texto_nombres;
        puntuaciones_puntos.GetComponent<TextMeshProUGUI>().text = texto_puntos;
    }

}
