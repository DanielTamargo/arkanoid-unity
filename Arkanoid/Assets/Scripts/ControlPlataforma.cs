using UnityEngine;
using System.Collections;
using System.IO;

public class ControlPlataforma : MonoBehaviour
{
    // Esta variable controlará si se ha iniciado o no, mientras no se inicie, la posición X de la bola será la misma que la de la plataforma
    private bool iniciado = false;
    
    public Rigidbody2D bola;
    public Rigidbody2D bolaConFisicas;
    
    private int numBotes = 0;

    // Velocidad a la que se desplaza la plataforma (medido en u/s)
    private float velocidad = 20f;

    // Velocidad a la que se desplazan la bola (medido en u/s)
    //private float velocidadBola = 8f; //facil

    // Fuerza de lanzamiento del rebote
    private float fuerza = 4f;

    private float limiteMuroIzq = -1000.0f;
    private float limiteMuroDer = 1000.0f;

    void Start()
    {
        //Conseguimos la bola
        Rigidbody2D d = (Rigidbody2D)Instantiate(bola, transform.position, transform.rotation);
        bola = d;
        bola.transform.Translate(Vector2.up * 3.0f);
    }

    void Update()
    {

        //Debug.Log(transform.position.y);

        transform.position = new Vector2(transform.position.x, -6.5f);

        // Calculamos la anchura visible de la cámara en pantalla
        float distanciaHorizontal = Camera.main.orthographicSize * Screen.width / Screen.height;

        // Calculamos el límite izquierdo y el derecho de la pantalla
        float limiteIzq = -1.0f * distanciaHorizontal;
        float limiteDer = 1.0f * distanciaHorizontal;

        // Tecla: Izquierda
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x > limiteMuroIzq)
            {
                // Nos movemos a la izquierda hasta llegar al límite para entrar por el otro lado
                if (transform.position.x > limiteIzq)
                {
                    transform.Translate(Vector2.left * velocidad * Time.deltaTime);
                    if (!iniciado)
                    {
                        if (bola != null)
                        {
                            bola.transform.Translate(Vector2.left * velocidad * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    transform.position = new Vector2(limiteDer, transform.position.y);
                    if (!iniciado)
                    {
                        if (bola != null)
                        {
                            bola.transform.position = new Vector2(limiteDer, bola.position.y);
                        }
                    }

                }
            }
        }

        // Tecla: Derecha
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x < limiteMuroDer)
            {
                // Nos movemos a la derecha hasta llegar al límite para entrar por el otro lado
                if (transform.position.x < limiteDer)
                {
                    transform.Translate(Vector2.right * velocidad * Time.deltaTime);
                    if (!iniciado)
                    {
                        if (bola != null)
                        {
                            bola.transform.Translate(Vector2.right * velocidad * Time.deltaTime);
                        }
                    }
                }
                else
                {
                    transform.position = new Vector2(limiteIzq, transform.position.y);
                    if (!iniciado)
                    {
                        if (bola != null)
                        {
                            bola.transform.position = new Vector2(limiteIzq, bola.position.y);
                        }
                    }
                }
            }
        }

        

        // Iniciar con la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            barraEspaciadora();
        }

        if (bola != null)
        {
            if (bola.transform.position.y < -8.0f)
            {
                Destroy(bola.gameObject);
                bola = null;
            }
        }


    }

    void barraEspaciadora()
    {
        Debug.Log(Screen.width);
        Debug.Log(Screen.height);
        if (!iniciado)
        {
            iniciado = true;
            if (bola != null)
            {
                Destroy(bola.gameObject);
                bola = null;
            }
            Rigidbody2D d = (Rigidbody2D)Instantiate(bolaConFisicas, transform.position, transform.rotation);
            d.gravityScale = 0;
            d.transform.Translate(Vector2.up * 1.0f);
            //d.AddForce(Vector2.up * velocidadBola, ForceMode2D.Impulse);
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {

        // Necesitamos saber contra qué hemos chocado
        if (coll.gameObject.tag == "BolaNormal")
        {
            if (!iniciado)
            {
                if (bola != null)
                {
                    if (numBotes < 3)
                    {
                        bola.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
                        fuerza = fuerza - 1f;
                        numBotes++;
                    }
                }
            }

        }
    }

}