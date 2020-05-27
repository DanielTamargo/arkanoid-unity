using UnityEngine;
using System.Collections;
using System.IO;

public class ControlPlataforma : MonoBehaviour
{
    // Controlará si se ha iniciado o no, mientras no se inicie, la posición X de la bola será la misma que la de la plataforma
    private bool iniciado = false;
    
    // Controlará que ya se haya comenzado la cuenta atrás en el móvil
    private bool primerTouch = false;
    
    // Controlará si estamos aturdidos o no (el jefe dispara)
    private bool aturdido = false;
    
    public Rigidbody2D bola;
    public Rigidbody2D bolaConFisicas;
    
    private int numBotes = 0;

    // Velocidad a la que se desplaza la plataforma (medido en u/s)
    private float velocidad = 20f;

    // Velocidad a la que se desplazan la bola (medido en u/s)
    //private float velocidadBola = 8f; //facil

    // Fuerza de lanzamiento del rebote
    private float fuerza = 4f;

    void Start()
    {
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_tocaParaEmpezar.SetActive(true);

        //Conseguimos la bola
        Rigidbody2D d = (Rigidbody2D)Instantiate(bola, transform.position, transform.rotation);
        bola = d;
        bola.transform.Translate(Vector2.up * 3.0f);
    }

    void Update()
    {
        float tecladoHorizontal = Input.GetAxisRaw("Horizontal");
        //Debug.Log(transform.position.y);

        transform.position = new Vector2(transform.position.x, -6.5f);

        // Calculamos la anchura visible de la cámara en pantalla
        float distanciaHorizontal = Camera.main.orthographicSize * Screen.width / Screen.height;

        // Calculamos el límite izquierdo y el derecho de la pantalla
        float limiteIzq = -1.0f * distanciaHorizontal;
        float limiteDer = 1.0f * distanciaHorizontal;

        if (!aturdido) // Aturdido no se puede mover, solo entrará si NO estamos aturdidos
        {
            if (Input.touchCount > 0)
            {
                if (!primerTouch)
                {
                    primerTouch = true;
                    StartCoroutine(animacionComenzarPartida(true));
                }
                else
                {
                    Touch touch = Input.GetTouch(0);
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    //touchPosition.z = transform.position.z;
                    //touchPosition.y = transform.position.y;
                    if (touchPosition.x < 0)
                    {
                        moverALaIzquierda(limiteIzq, limiteDer);
                    }
                    if (touchPosition.x > 0)
                    {
                        moverALaDerecha(limiteIzq, limiteDer);
                    }
                }

            }


            // Teclas: A, Flecha Izquierda
            if (tecladoHorizontal < 0)
            {
                moverALaIzquierda(limiteIzq, limiteDer);
            }

            // Teclas: D, Flecha Derecha
            if (tecladoHorizontal > 0)
            {
                moverALaDerecha(limiteIzq, limiteDer);
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
        
    }

    IEnumerator animacionComenzarPartida(bool entera)
    {
        GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_tocaParaEmpezar.SetActive(false);
        if (entera) // Por si acaso utiliza un teclado por usb y le da al espacio
        {
            GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_3.SetActive(true);
            yield return new WaitForSeconds(1);
            GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_3.SetActive(false);
            GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_2.SetActive(true);
            yield return new WaitForSeconds(1);
            GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_2.SetActive(false);
            GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_1.SetActive(true);
            yield return new WaitForSeconds(1);
            GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_1.SetActive(false);
        }
        GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_go.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        barraEspaciadora();
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Overlay").GetComponent<ControlOverlay>().o_go.SetActive(false);
    }

    void moverALaIzquierda(float limiteIzq, float limiteDer)
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

    void moverALaDerecha(float limiteIzq, float limiteDer)
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

    void barraEspaciadora()
    {
        //Debug.Log(Screen.width);
        //Debug.Log(Screen.height);
        if (!iniciado)
        {
            if (!primerTouch)
                StartCoroutine(animacionComenzarPartida(false)); // Solo sale el 'Go!'
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
        else if (coll.gameObject.tag == "Disparo")
            aturdir();
    }

    IEnumerator aturdir()
    {
        //reproducir sonido stuneado
        aturdido = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("paddleRed_stun");
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("paddleRed");
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("paddleRed_stun");
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("paddleRed");
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("paddleRed_stun");
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("paddleRed");
        aturdido = false;
    }

}