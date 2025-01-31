﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBola : MonoBehaviour
{
    public int nivel = 1;
    private float speed = 12.0f;

    private int numBloquesTotal = 0;
    private int bloquesGolpeados = 0;
    private int golpesSeguidos = 0;
    private int golpesBordesSeguidos = 0;

    private bool nivelFallido = false;
    private bool nivelSuperado = false;

    public GameObject efectoExplosion;
    private bool slowmo = false;
    private bool r2d2 = false;
    private bool wohoo = false;
    private bool revert = false;

    private GameObject overlay;

    // Start is called before the first frame update
    void Start()
    {
        overlay = GameObject.Find("Overlay");

        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;

        nivel = overlay.GetComponent<ControlOverlay>().nivel;
        actualizarVelocidad();
        GameObject[] bloquesAzules = GameObject.FindGameObjectsWithTag("BloqueAzul");
        numBloquesTotal += bloquesAzules.Length;
    }

    void actualizarVelocidad()
    {
        if (nivel == 1)
            speed = 10.0f;
        else if (nivel == 2)
            speed = 12.0f;
        else if (nivel == 3)
            speed = 14.0f;
        else
            speed = 16.0f;

        GetComponent<Rigidbody2D>().velocity = Vector2.up * speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y <= -11f && !nivelSuperado) 
        {
            if (!nivelFallido)
            {
                if(overlay.GetComponent<ControlOverlay>().vidas <= 1)
                    StartCoroutine(animacionLose());
                else
                {
                    FindObjectOfType<AudioManager>().Play("sfx_fall");
                    overlay.GetComponent<ControlOverlay>().intentoFallido();
                }
            }
                
            nivelFallido = true;
        }

        if (bloquesGolpeados >= numBloquesTotal)
        {
            if (!nivelSuperado)
            {
                nivelSuperado = true;
                StartCoroutine(animacionWin());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Plataforma")
        {
            // Reseteamos los golpes seguidos
            golpesSeguidos = 0;
            //golpesBordesSeguidos = 0;

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
            int puntosBloque = 100;
            if (collision.gameObject.name == "prefabBloqueMorado(Clone)")
                puntosBloque = 150;
            else if (collision.gameObject.name == "prefabBloqueRojo(Clone)")
                puntosBloque = 200;
            else if (collision.gameObject.name == "prefabBloqueVerde(Clone)")
                puntosBloque = 300;


            golpesSeguidos += 1;
            //golpesBordesSeguidos = 0;
            bool bloqueMuere = collision.gameObject.GetComponent<ControlBloqueAzul>().bloqueMuere();
            if (bloqueMuere)
            {
                bloquesGolpeados += 1;
                overlay.GetComponent<ControlOverlay>().puntos += puntosBloque * golpesSeguidos;
                //Instantiate(efectoExplosion, collision.transform.position, Quaternion.identity);
                if (golpesSeguidos == 3)
                {
                    StartCoroutine(comboYouRock());
                }
                else if (golpesSeguidos == 4)
                {
                    StartCoroutine(comboWooho());
                }
                else if (golpesSeguidos >= 5)
                {
                    StartCoroutine(comboR2D2());
                }
                if (bloquesGolpeados >= numBloquesTotal)
                {
                    Instantiate(efectoExplosion, collision.transform.position, Quaternion.identity);
                    FindObjectOfType<AudioManager>().Play("sfx_exp_long");
                    nivelSuperado = true;
                    StartCoroutine(animacionWin());
                }
            }
            
        }
        if (collision.gameObject.tag == "Bordes")
        {
            golpesBordesSeguidos += 1;
            if (golpesBordesSeguidos >= 7) // Si da 7 golpes seguidos entre paredes, se lanza hacia abajo
            {
                // Calculamos la dirección
                Vector2 dir = new Vector2(transform.position.x, -1).normalized;

                // Cofiguramos la Velocity con dir * speed
                GetComponent<Rigidbody2D>().velocity = dir * speed;
            }
        }
        else
            golpesBordesSeguidos = 0;
    }

    IEnumerator animacionLose()
    {
        overlay.GetComponent<ControlOverlay>().animacionVidas();
        FindObjectOfType<AudioManager>().Play("sfx_lose");
        overlay.GetComponent<ControlOverlay>().animacion(0);
        yield return new WaitForSeconds(3);
        overlay.GetComponent<ControlOverlay>().intentoFallido();
    }

    IEnumerator animacionWin()
    {
        FindObjectOfType<AudioManager>().Play("sfx_win");
        if (overlay.GetComponent<ControlOverlay>().nivel == 4)
            overlay.GetComponent<ControlOverlay>().animacion(2);
        else
            overlay.GetComponent<ControlOverlay>().animacion(1);
        yield return new WaitForSeconds(3);
        overlay.GetComponent<ControlOverlay>().pasarNivel();
    }

    IEnumerator comboYouRock()
    {
        Time.timeScale = 0.65f;
        if (!slowmo)
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo");
            yield return new WaitForSeconds(0.2f);
        }
        slowmo = true;
        
        FindObjectOfType<AudioManager>().Play("sfx_combo_1");
        yield return new WaitForSeconds(1.5f);
        if (golpesSeguidos <= 3 || (golpesSeguidos > 3 && !wohoo && !r2d2))
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo_revert");
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
            slowmo = false;
        }
    }

    IEnumerator comboWooho()
    {
        wohoo = true;
        Time.timeScale = 0.4f;
        if (!slowmo)
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo");
            yield return new WaitForSeconds(0.2f);
        }
        slowmo = true;
        FindObjectOfType<AudioManager>().Play("sfx_combo_2");
        yield return new WaitForSeconds(1.5f);
        if (golpesSeguidos <= 4 || (golpesSeguidos > 4 && !r2d2))
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo_revert");
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
            slowmo = false;
            wohoo = false;
        }
    }

    IEnumerator comboR2D2()
    {
        Time.timeScale = 0.25f;
        if (!slowmo)
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo");
            yield return new WaitForSeconds(0.2f);
        }
        slowmo = true;
        if (!r2d2)
        {
            FindObjectOfType<AudioManager>().Play("sfx_combo_3");
            r2d2 = true;
        }
        yield return new WaitForSeconds(0.8f);
        if (!revert)
        {
            revert = true;
            FindObjectOfType<AudioManager>().Play("sfx_slowmo_revert");
        }
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1f;
        slowmo = false;
        r2d2 = false;
        yield return new WaitForSeconds(0.5f);
        revert = false;
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
