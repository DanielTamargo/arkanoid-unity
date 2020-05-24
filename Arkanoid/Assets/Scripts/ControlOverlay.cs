using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlOverlay : MonoBehaviour
{
    // Puntos ganados en la partida
    public int puntos = 0;
    public int nivel = 1;
    public int puntosPreNivel = 0;

    // Objeto donde mostramos el texto
    public GameObject o_puntuacion;
    public GameObject o_nivel;

    private Text t_puntuacion;
    private Text t_nivel;

    // Use this for initialization
    void Start()
    {
        // Localizamos el componente del UI
        t_nivel = o_nivel.GetComponent<Text>();
        t_puntuacion = o_puntuacion.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        // Actualizamos el marcador
        t_nivel.text = "Nivel: " + nivel.ToString();
        t_puntuacion.text = "Puntos: " + puntos.ToString();
    }
}
