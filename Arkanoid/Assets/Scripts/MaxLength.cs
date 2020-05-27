using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MaxLength : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<TextMeshProUGUI>().text.Length > 5)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<TextMeshProUGUI>().text.Substring(0, 5);
        }
    }
}
