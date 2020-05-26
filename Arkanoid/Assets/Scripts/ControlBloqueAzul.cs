using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBloqueAzul : MonoBehaviour
{

    public GameObject efectoParticulasAzul;

    private int vidas = 1;

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
        }
        return (vidas <= 0);
    }

}
