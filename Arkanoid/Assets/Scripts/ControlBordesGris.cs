using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBordesGris : MonoBehaviour
{
    public GameObject grayWallsRota;
    public GameObject efectoExplosion;
    private bool explosion = false;

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("BolaNormal(Clone)"))
        {
            if (!explosion)
            {
                explosion = true;
                Vector3 explosionIzq = new Vector3((this.transform.position.x - 17.76f), (this.transform.position.y - 7.64f), this.transform.position.z);
                Vector3 explosionDer = new Vector3((this.transform.position.x + 17.76f), (this.transform.position.y - 7.64f), this.transform.position.z);


                Instantiate(grayWallsRota, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                Instantiate(efectoExplosion, explosionIzq, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("sfx_exp_long");
                Instantiate(efectoExplosion, explosionDer, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("sfx_exp_long");
            }
        }
    }
}
