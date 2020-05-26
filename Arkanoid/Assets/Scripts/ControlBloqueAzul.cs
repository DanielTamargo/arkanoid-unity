using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBloqueAzul : MonoBehaviour
{

    public GameObject efectoParticulasAzul;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter2D(Collision2D coll)
    {

        // Necesitamos saber contra qué hemos chocado
        if (coll.gameObject.tag == "BolaNormal")
        {
            FindObjectOfType<AudioManager>().Play("sfx_exp_short");
            Instantiate(efectoParticulasAzul, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }


    }

}
