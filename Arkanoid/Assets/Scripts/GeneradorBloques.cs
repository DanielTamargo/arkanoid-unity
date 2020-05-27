using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GeneradorBloques : MonoBehaviour
{

    // Publicamos la variable para conectarla desde el editor
    public int nivel = 1;
    public Rigidbody2D prefabBloqueAzul;
    public Rigidbody2D prefabBloqueMorado;
    public Rigidbody2D prefabBloqueRojo;
    public Rigidbody2D prefabBloqueVerde;

    // Referencia para guardar una matriz de objetos
    private Rigidbody2D[,] bloques;

    // Tamaño de la rejilla de bloques
    private const int FILAS = 2;
    private const int COLUMNAS = 6;

    // Enumeración para expresar el sentido del movimiento
    private enum direccion { IZQ, DER };

    // Use this for initialization
    void Start()
    {
        //          (FILAS, COLUMNAS, ESPACIO HORIZONTAL, ESPACIO VERTICAL);
        if (nivel == 1)
            generarBloquesNivel1(2, 6, 4.0f, 2.0f);
        else if (nivel == 2)
            generarBloquesNivel2(3, 5, 5.0f, 2.0f);
        else if (nivel == 3)
            generarBloquesNivel3(2, 7, 3.7f, 2.0f);
        else
            generarBloquesNivel4(3, 6, 4.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void generarBloquesNivel1(int filas, int columnas, float espacioH, float espacioV, float escala = 1.0f)
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

        Destroy(gameObject);

    }

    void generarBloquesNivel2(int filas, int columnas, float espacioH, float espacioV, float escala = 1.0f)
    {
        Vector2 origen = new Vector2(transform.position.x - (columnas / 2.0f) * espacioH + (espacioH / 2), transform.position.y);
        bloques = new Rigidbody2D[filas, columnas];
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Vector2 posicion = new Vector2(origen.x + (espacioH * j), origen.y + (espacioV * i));

                Rigidbody2D bloque;
                if (i == 0)
                    bloque = (Rigidbody2D)Instantiate(prefabBloqueAzul, posicion, transform.rotation);
                else if ((i + j) % 2 == 0)
                {
                    if (i == 2 && j == 2)
                        bloque = (Rigidbody2D)Instantiate(prefabBloqueMorado, posicion, transform.rotation);
                    else
                        bloque = (Rigidbody2D)Instantiate(prefabBloqueAzul, posicion, transform.rotation);
                }
                else
                    bloque = (Rigidbody2D)Instantiate(prefabBloqueMorado, posicion, transform.rotation);
                bloques[i, j] = bloque;
            }
        }
        Destroy(gameObject);
    }

    void generarBloquesNivel3(int filas, int columnas, float espacioH, float espacioV, float escala = 1.0f)
    {
        Vector2 origen = new Vector2(transform.position.x - (columnas / 2.0f) * espacioH + (espacioH / 2), transform.position.y);
        bloques = new Rigidbody2D[filas, columnas];
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Vector2 posicion = new Vector2(origen.x + (espacioH * j), origen.y + (espacioV * i));

                Rigidbody2D bloque;
                if ((i + j) % 3 == 0)
                    bloque = (Rigidbody2D)Instantiate(prefabBloqueRojo, posicion, transform.rotation);
                else if ((i + j) % 2 == 0)
                    bloque = (Rigidbody2D)Instantiate(prefabBloqueMorado, posicion, transform.rotation);
                else
                    bloque = (Rigidbody2D)Instantiate(prefabBloqueAzul, posicion, transform.rotation);
                bloques[i, j] = bloque;
            }
        }
        Destroy(gameObject);
    }

    void generarBloquesNivel4(int filas, int columnas, float espacioH, float espacioV, float escala = 1.0f)
    {
        Vector2 origen = new Vector2(transform.position.x - (columnas / 2.0f) * espacioH + (espacioH / 2), transform.position.y);
        bloques = new Rigidbody2D[filas, columnas];
        for (int i = 0; i < filas; i++)
        {
            for (int j = 0; j < columnas; j++)
            {
                Vector2 posicion = new Vector2(origen.x + (espacioH * j), origen.y + (espacioV * i));
                Rigidbody2D bloque;
                if (i == 2)
                {
                    if (j == 2)
                    {
                        bloque = (Rigidbody2D)Instantiate(prefabBloqueVerde, posicion, transform.rotation);
                        bloques[i, j] = bloque;
                    }
                } else
                {
                    if (i == 1)
                        bloque = (Rigidbody2D)Instantiate(prefabBloqueRojo, posicion, transform.rotation);
                    else if ((i + j) % 2 == 0)
                        bloque = (Rigidbody2D)Instantiate(prefabBloqueMorado, posicion, transform.rotation);
                    else
                        bloque = (Rigidbody2D)Instantiate(prefabBloqueAzul, posicion, transform.rotation);
                    bloques[i, j] = bloque;
                }
            }
        }
        Destroy(gameObject);
    }

}