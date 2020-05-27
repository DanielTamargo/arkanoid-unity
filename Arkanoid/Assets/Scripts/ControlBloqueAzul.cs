using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBloqueAzul : MonoBehaviour
{

    public GameObject efectoParticulasAzul;
    public GameObject efectoParticulasMini;
    public int tipo = 1; //1 = Azul, 2 = Morado, 3 = Rojo, 4 = Verde


    // El "jefe final" se mueve
    private float velocidad = 5f;
    private enum direccion { IZQ, DER };
    private direccion rumbo = direccion.DER;

    private int vidas;
    void Start()
    {
        if (tipo == 1)
            vidas = 1;
        else if (tipo == 2)
            vidas = 2;
        else if (tipo == 3)
            vidas = 3;
        else
            vidas = 8;
    }

    private void Update()
    {
        if (tipo == 4)
        {
            if (rumbo == direccion.DER)
                transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            else
                transform.Translate(Vector2.left * velocidad * Time.deltaTime);
        }
    }

    // Devuelve true si el bloque ha explotado
    // Sirve para que la bola se de cuenta de que lo ha roto y sume los puntos
    // (lo hago así porque hay bonus de puntos según cuántos bloques seguidos rompa la bola)
    public bool bloqueMuere()
    {
        // Recibe un bloque
        vidas -= 1;
        if (vidas <= 0)
        {
            FindObjectOfType<AudioManager>().Play("sfx_exp_short");
            Instantiate(efectoParticulasAzul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        } else
        {
            agrietarBloque();
            Instantiate(efectoParticulasMini, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("sfx_impact_1");
        }
        return (vidas <= 0);
    }

    // Agrietamos el bloque según vaya recibiendo golpes (en caso de tener más de 1 golpe de vida)
    private void agrietarBloque()
    {
        //1 = Azul, 2 = Morado, 3 = Rojo, 4 = Verde
        if (tipo == 2)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("purple_1");
        } else if (tipo == 3)
        {
            if (vidas == 2)
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("red_1");
            else
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("red_2");
        } else
        {
            if (vidas == 6)
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("green_1");
            else if (vidas == 4)
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("green_2");
            else if (vidas == 2)
                this.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("green_3");
        }

    }


    // Cambiamos rumbo del boss
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tipo == 4)
        {
            if (collision.gameObject.tag != "BolaNormal")
            {
                if (rumbo == direccion.DER)
                    rumbo = direccion.IZQ;
                else
                    rumbo = direccion.DER;
            }
        }
    }

}
