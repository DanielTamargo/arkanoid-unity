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

    public GameObject efectoExplosion;
    private bool slowmo = false;

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
            //Instantiate(efectoExplosion, collision.transform.position, Quaternion.identity);
            if (golpesSeguidos == 3)
            {
                StartCoroutine(comboYouRock());
            }
            else if (golpesSeguidos == 4)
            {
                StartCoroutine(comboWooho());
            }
            else if (golpesSeguidos == 5)
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

    IEnumerator animacionLose()
    {
        FindObjectOfType<AudioManager>().Play("sfx_lose");
        overlay.GetComponent<ControlOverlay>().animacion(0);
        yield return new WaitForSeconds(3);
        overlay.GetComponent<ControlOverlay>().pasarNivel();
    }

    IEnumerator animacionWin()
    {
        FindObjectOfType<AudioManager>().Play("sfx_win");
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
        if (golpesSeguidos <= 3)
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo_revert");
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
            slowmo = false;
        }
    }

    IEnumerator comboWooho()
    {
        Time.timeScale = 0.4f;
        if (!slowmo)
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo");
            yield return new WaitForSeconds(0.2f);
        }
        slowmo = true;
        FindObjectOfType<AudioManager>().Play("sfx_combo_2");
        yield return new WaitForSeconds(1.5f);
        if (golpesSeguidos <= 4)
        {
            FindObjectOfType<AudioManager>().Play("sfx_slowmo_revert");
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 1f;
            slowmo = false;
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
        FindObjectOfType<AudioManager>().Play("sfx_combo_3");
        yield return new WaitForSeconds(1.5f);
        FindObjectOfType<AudioManager>().Play("sfx_slowmo_revert");
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1f;
        slowmo = false;
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
