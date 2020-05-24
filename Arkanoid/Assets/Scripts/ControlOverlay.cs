using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlOverlay : MonoBehaviour
{
    // Puntos ganados en la partida
    public int puntos = 0;
    public int niveles = 1;

    // Objeto donde mostramos el texto
    public GameObject nivel;
    public GameObject puntuacion;

    private GameObject mantenerDatos;

    private Text punt;
    private Text niv;

    // Use this for initialization
    void Start()
    {

        mantenerDatos = GameObject.Find("MatenerDatos(Clone)");
        if (mantenerDatos != null) 
        {
            puntos = mantenerDatos.GetComponent<MantenerDatosScript>().puntuacionInicioNivel;
            niveles = mantenerDatos.GetComponent<MantenerDatosScript>().nivel;
        }

        // Localizamos el componente del UI
        niv = nivel.GetComponent<Text>();
        punt = puntuacion.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        // Actualizamos el marcador
        niv.text = "Nivel: " + niveles.ToString();
        punt.text = "Puntos: " + puntos.ToString();
    }
}
