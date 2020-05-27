using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDisparo : MonoBehaviour
{

    public GameObject particulasExplosionMini;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(particulasExplosionMini, transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("sfx_impact_1");
        Destroy(gameObject);
    }
}
