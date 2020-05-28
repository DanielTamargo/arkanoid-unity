using System.Collections;
using Firebase;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BaseDeDatos : MonoBehaviour
{
    private FirebaseFirestore db;
    public static BaseDeDatos instance;
    public List<Puntuacion> listaPuntuaciones = new List<Puntuacion>();

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Conectamos con la base de datos
        try
        {
            db = FirebaseFirestore.GetInstance(FirebaseApp.Create()); // Se cierra también :/
        }
        catch
        {
            db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance); //<- Bug!
        }

        // Creamos una referencia a una colección (es decir, a una """tabla""" para luego trabajar con los datos)
        CollectionReference puntuaciones = db.Collection("puntuaciones");
        
        // Preparamos la Query
        Query query = puntuaciones.OrderByDescending("puntos").Limit(10);
        
        // Creamos un listener con la Query, es decir, recogemos los datos pero no solo eso
        // sino que también, este listener se ejecutará cada vez que haya un cambio en la BBDD
        ListenerRegistration listener = query.Listen(snapshot =>
        {
            List<Puntuacion> cargandoPuntuaciones = new List<Puntuacion>();
            //Guardo continuamente los jugadores en una lista para enseñarla cuando muestre la tabla de puntuaciones
            foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
            {
                Dictionary<string, object> punt = documentSnapshot.ToDictionary();
                Puntuacion puntuacion = new Puntuacion();
                puntuacion.nombre = $"{punt["nombre"]}";
                string puntosStr = $"{punt["puntos"]}";
                puntuacion.puntos = int.Parse(puntosStr);
                cargandoPuntuaciones.Add(puntuacion);
            }
            listaPuntuaciones = cargandoPuntuaciones;
        });
    }

    public void testNuevaPuntuacion()
    {
        // Datos fijos para el test
        int puntos = 2000;
        string nombre = "Test";

        try
        {
            // Creamos un id aleatorio para la colección
            string id = db.Collection("collection_name").Document().Id;

            // Creamos el documento con el id aleatorio
            DocumentReference docRef = db.Collection("puntuaciones").Document(id);

            // Le implementamos los datos 
            // (esto puede ir incluso vacío, pero mejor lo construyo ya como nueva puntuación)
            var datos = new Dictionary<string, object>
            {
                {"nombre", nombre},
                {"puntos", puntos}
            };

            // Lo sincronizamos
            docRef.SetAsync(datos);
            Debug.Log("¡Test insertado!");
        } catch
        {
            Debug.LogWarning("Fallo al insertar");
        }
    }

    public void nuevaPuntuacion(int puntos, string nombre)
    {
        try
        {
            // Creamos un id aleatorio para la colección
            string id = db.Collection("collection_name").Document().Id;

            // Creamos el documento con el id aleatorio
            DocumentReference docRef = db.Collection("puntuaciones").Document(id);

            // Le implementamos los datos 
            // (esto puede ir incluso vacío, pero mejor lo construyo ya como nueva puntuación)
            var datos = new Dictionary<string, object>
            {
                {"nombre", nombre},
                {"puntos", puntos}
            };

            // Lo sincronizamos
            docRef.SetAsync(datos);
            Debug.Log("¡Puntuación insertada!");
        }
        catch
        {
            Debug.LogWarning("Fallo al insertar la puntuación, ¡¡¡nooooo!!!");
        }
    }

}
