using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasarAPrimerNivel : MonoBehaviour
{

    public GameObject overlay;
    //private GameObject overlay;

    void Start()
    {
        //overlay = GameObject.Find("Overlay");
        DontDestroyOnLoad(overlay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
