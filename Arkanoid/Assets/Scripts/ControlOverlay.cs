using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlOverlay : MonoBehaviour
{
    // Puntos ganados en la partida
    public int puntos = 0;

    // Objeto donde mostramos el texto
    public GameObject nivel;
    public GameObject puntuacion;

    private Text punt;
    private Text niv;

    // Use this for initialization
    void Start()
    {
        // Localizamos el componente del UI
        niv = nivel.GetComponent<Text>();
        punt = puntuacion.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        // Actualizamos el marcador
        niv.text = nivel.ToString();
        punt.text = "Puntos: " + puntos.ToString() + "\n";
    }
}
