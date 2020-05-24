using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GeneradorBloques : MonoBehaviour
{

    // Publicamos la variable para conectarla desde el editor
    public Rigidbody2D prefabBloqueAzul;

    // Referencia para guardar una matriz de objetos
    private Rigidbody2D[,] bloques;

    // Tamaño de la rejilla de bloques
    private const int FILAS = 3;
    private const int COLUMNAS = 6;

    // Enumeración para expresar el sentido del movimiento
    private enum direccion { IZQ, DER };

    // Rumbo que lleva el pack de bloques
    //private direccion rumbo = direccion.DER;

    // Posición vertical de la horda (lo iremos restando de la .y de cada bloque)
    //private float altura = 0.5f;

    // Límites de la pantalla
    //private float limiteIzq;
    //private float limiteDer;

    // Velocidad a la que se desplazan los bloques (medido en u/s)
    //private float velocidad = 5f;

    // Use this for initialization
    void Start()
    {
        // Rejilla de 4x7 bloques
        generarBloques(FILAS, COLUMNAS, 4.0f, 2.0f);

        // Calculamos la anchura visible de la cámara en pantalla
        //float distanciaHorizontal = Camera.main.orthographicSize * Screen.width / Screen.height;

        // Calculamos el límite izquierdo y el derecho de la pantalla (añadimos una unidad a cada lado como margen)
        //limiteIzq = -1.0f * distanciaHorizontal + 1;
        //limiteDer = 1.0f * distanciaHorizontal - 1;
    }

    // Update is called once per frame
    void Update()
    {
        // Contador para saber si hemos terminado
        int numBloques = 0;

        // Variable para saber si al menos un bloque ha llegado al borde
        //bool limiteAlcanzado = false;

        // Recorremos la cuadricula de bloques
        for (int i = 0; i < FILAS; i++)
        {
            for (int j = 0; j < COLUMNAS; j++)
            {
                // Comprobamos que haya objeto, para cuando nos empiecen a disparar
                if (bloques[i, j] != null)
                {
                    // Un bloque más
                    numBloques += 1;

                    /*// ¿Vamos a izquierda o derecha? //Los bloques no se mueven
                    if (rumbo == direccion.DER)
                    {

                        // Nos movemos a la derecha (todos los aliens que queden)
                        aliens[i, j].transform.Translate(Vector2.right * velocidad * Time.deltaTime);

                        // Comprobamos si hemos tocado el borde
                        if (aliens[i, j].transform.position.x > limiteDer)
                        {
                            limiteAlcanzado = true;
                        }
                    }
                    else
                    {

                        // Nos movemos a la derecha (todos los aliens que queden)
                        aliens[i, j].transform.Translate(Vector2.left * velocidad * Time.deltaTime);

                        // Comprobamos si hemos tocado el borde
                        if (aliens[i, j].transform.position.x < limiteIzq)
                        {
                            limiteAlcanzado = true;
                        }
                    }*/
                }
            }
        }

        /*// Si no quedan bloques, hemos terminado
        if (numBloques == 0)
        {
            SceneManager.LoadScene("Nivel1");
        }*/

        /*// Si al menos un bloque ha tocado el borde, todo el pack cambia de rumbo //Los bloques no se mueven
        if (limiteAlcanzado == true)
        {
            for (int i = 0; i < FILAS; i++)
            {
                for (int j = 0; j < COLUMNAS; j++)
                {

                    // Comprobamos que haya objeto, para cuando nos empiecen a disparar
                    if (bloques[i, j] != null)
                    {
                        bloques[i, j].transform.Translate(Vector2.down * altura);
                    }
                }
            }


            if (rumbo == direccion.DER)
            {
                rumbo = direccion.IZQ;
            }
            else
            {
                rumbo = direccion.DER;
            }
        }*/
    }

    void generarBloques(int filas, int columnas, float espacioH, float espacioV, float escala = 1.0f)
    {
        /* Creamos una rejilla de aliens a partir del punto de origen
		 * 
		 * Ejemplo (2,5):
		 *   A A A A A
		 *   A A O A A
		 */

        // Calculamos el punto de origen de la rejilla
        Vector2 origen = new Vector2(transform.position.x - (columnas / 2.0f) * espacioH + (espacioH / 2), transform.position.y);

        // Instanciamos el array de referencias
        bloques = new Rigidbody2D[filas, columnas];

        // Fabricamos un bloque en cada posición del array
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {

                // Posición de cada bloque
                Vector2 posicion = new Vector2(origen.x + (espacioH * j), origen.y + (espacioV * i));

                // Instanciamos el objeto partiendo del prefab
                Rigidbody2D bloque = (Rigidbody2D)Instantiate(prefabBloqueAzul, posicion, transform.rotation);

                // Guardamos el bloque en el array (la rejilla)
                bloques[i, j] = bloque;

                /*// Escala opcional, por defecto 1.0f (sin escala)
                // Nota: El prefab original ya está escalado a 0.2f
                bloque.transform.localScale = new Vector2(0.2f * escala, 0.2f * escala);*/
            }
        }

    }

}