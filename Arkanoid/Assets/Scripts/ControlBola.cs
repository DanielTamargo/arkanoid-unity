﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBola : MonoBehaviour
{

    public float speed = 1000.0f;

    private int numBloquesTotal = 0;
    private int bloquesGolpeados = 0;

    private GameObject overlay;

    // Start is called before the first frame update
    void Start()
    {
        overlay = GameObject.Find("Overlay");
       

        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;

        GameObject[] bloquesAzules = GameObject.FindGameObjectsWithTag("BloqueAzul");
        numBloquesTotal += bloquesAzules.Length;

    }

    // Update is called once per frame
    void Update()
    {
        if (bloquesGolpeados >= numBloquesTotal) 
        {
            DontDestroyOnLoad(overlay);
            SceneManager.LoadScene("EscenaPrincipal");
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Plataforma")
        {
            // Calculamos el golpe
            float x = calcularGolpe(transform.position,
                              collision.transform.position,
                              collision.collider.bounds.size.x);

            // Calculamos la dirección, hacemos set length hasta 1
            Vector2 dir = new Vector2(x, 1).normalized;

            // Cofiguramos la Velocity con dir * speed
            GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
        if (collision.gameObject.tag == "BloqueAzul")
        {
            bloquesGolpeados += 1;
            overlay.GetComponent<ControlOverlay>().puntos += 100;
        }
    }

    float calcularGolpe(Vector2 posicionBola, Vector2 posicionPlataforma,
            float anchuraPlataforma)
    {
        // ascii art:
        //
        //-1  -0.5  0  0.5   1  <- x
        // ===================  <- plataforma
        //
        return (posicionBola.x - posicionPlataforma.x) / anchuraPlataforma;
    }


}
