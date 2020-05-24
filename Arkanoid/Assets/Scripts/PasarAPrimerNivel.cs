using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasarAPrimerNivel : MonoBehaviour
{

    private GameObject overlay;

    // Start is called before the first frame update
    void Start()
    {
        overlay = GameObject.Find("Overlay");
        DontDestroyOnLoad(overlay);
        SceneManager.LoadScene("EscenaPrincipal");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
