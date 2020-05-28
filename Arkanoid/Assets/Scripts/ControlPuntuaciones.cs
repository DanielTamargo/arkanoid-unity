using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlPuntuaciones : MonoBehaviour
{
    public GameObject anterior;
    public GameObject siguiente;
    public GameObject puntuaciones;
    private List<Puntuacion> listaPuntuaciones = new List<Puntuacion>();
    private int pagina = 1;

    // Start is called before the first frame update
    void Start()
    {
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
            anterior.SetActive(false);
        
    }

    public void botonSiguiente()
    {
        pagina++;
        actualizarPuntuaciones();
        anterior.SetActive(true);
        
        if ((pagina + 1) * 6 >= listaPuntuaciones.Count)
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
        string texto_puntuaciones = "";
        if (listaPuntuaciones.Count > 0)
        {
            //string formato = "{0,16}{1,13}{2,17}";
            string formato = "{0}{1}{2}";
            int posicion = 1 + (6 * (pagina - 1));
            int objetivo = posicion + 6;
            for(int i = posicion; i < objetivo; i++)
            {
                if (listaPuntuaciones.Count > i)
                {
                    Puntuacion puntuacion = listaPuntuaciones[i];
                    string nombre = puntuacion.nombre;
                    string puntos = puntuacion.puntos.ToString();
                    string linea = string.Format(formato, posicion.ToString().PadLeft(16), 
                        nombre.PadLeft(18), puntos.PadLeft(11));
                    texto_puntuaciones += linea + "\n";
                    posicion++;
                }
            }
        } else
        {
            texto_puntuaciones = "\n\nActualmente no hay registros o hay problemas con la base de datos";
        }
        puntuaciones.GetComponent<TextMeshProUGUI>().text = texto_puntuaciones;
    }

}
