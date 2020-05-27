using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoImagenMovimiento : MonoBehaviour
{
    public float velocidadScroll;
    public GameObject imagen;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x == 0) {
            pos = new Vector3(41, transform.position.y, transform.position.z);
            GameObject generar = (GameObject)Instantiate(imagen, pos, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * velocidadScroll * Time.deltaTime);
        if (transform.position.x <= -41) {
            pos = new Vector3(41, transform.position.y, transform.position.z);
            GameObject generar = (GameObject)Instantiate(imagen, pos, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
