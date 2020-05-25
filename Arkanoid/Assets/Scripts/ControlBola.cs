using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlBola : MonoBehaviour
{

    public float speed = 1000.0f;

    private int numBloquesTotal = 0;
    private int bloquesGolpeados = 0;
    private int golpesSeguidos = 0;

    private bool nivelFallido = false;
    private bool nivelSuperado = false;

    public GameObject efectoParticulasAzul;


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

        if (transform.position.y <= -11f && !nivelSuperado) 
        {
            if (!nivelFallido)
                overlay.GetComponent<ControlOverlay>().intentoFallido();

            nivelFallido = true;
        }

        if (bloquesGolpeados >= numBloquesTotal)
        {
            if (!nivelSuperado)
            {
                nivelSuperado = true;
                StartCoroutine(animacionFinal());
            }
        }
    }

    IEnumerator animacionFinal()
    {
        //animacion explosion  //poner un sonido

        //esperar a que el sonido acabe
        yield return new WaitForSeconds(4);

        overlay.GetComponent<ControlOverlay>().pasarNivel();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Plataforma")
        {
            // Reseteamos los golpes seguidos
            golpesSeguidos = 0;

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
            golpesSeguidos += 1;
            bloquesGolpeados += 1;
            overlay.GetComponent<ControlOverlay>().puntos += 100 * golpesSeguidos;
            Instantiate(efectoParticulasAzul, collision.transform.position, Quaternion.identity);
            if (golpesSeguidos == 3)
            {
                StartCoroutine(comboYouRock());
            } else if (golpesSeguidos == 4)
            {
                StartCoroutine(comboWooho());
            } else if (golpesSeguidos == 5)
            {
                StartCoroutine(comboR2D2());
            }
        }
    }

    IEnumerator comboYouRock()
    {
        //falta sonido you rock
        Time.timeScale = 0.75f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1f;
    }

    IEnumerator comboWooho()
    {
        //falta sonido wooho
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1f;
    }

    IEnumerator comboR2D2()
    {
        //falta sonido waaaaaaaaaaaaaAAAAAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHH!!!
        Time.timeScale = 0.25f;
        yield return new WaitForSeconds(1);
        Time.timeScale = 1f;
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
