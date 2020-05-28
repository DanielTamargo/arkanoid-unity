using System.Collections;
using Firebase;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine;

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
        //db = FirebaseFirestore.GetInstance(FirebaseApp.DefaultInstance); //<- Bug!
        db = FirebaseFirestore.GetInstance(FirebaseApp.Create());

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

    public void nuevaPuntuacion(int puntos, string nombre)
    {
        // Insertar nueva puntuación
    }

}
