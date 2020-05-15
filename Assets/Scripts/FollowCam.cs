using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Посилання на потрібнйи об’єкт // а

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // Бажана координата Z камери
    void Awake()
    {
        camZ = this.transform.position.z;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        //// Однорядкова версія if не потребує фігурних дужок
        //if (POI == null) return; // вийти, якщо нема потрібного об’єкта // b
        //// Отримати позицію потрібного об’єкта
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        // Якщо нема потрібного об’єкта, повернути Р:[ 0, 0, 0 ]
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            // Отримати позицію потрібного об’єкта
            destination = POI.transform.position;
            // Якщо потрібний об’єкт - снаряд, переконатися, що він зупинився
            if (POI.tag == "Projectile")
            {
                // Якщо він стоїть на місці (тобто, не рухається)
                if (POI.GetComponent<Rigidbody>().IsSleeping()){
                    // повернути початкові налаштування поля зору камери
                    POI = null;
                    //в наступному кадрі
                    return;
                }
            }
        }
        // Обмежити X и Y мінімальними значеннями
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        // Визначити точку між існуючим положенням камери и destination
        destination = Vector3.Lerp(transform.position, destination, easing);
        // Примусово встановити значення destination.z рівним camZ, щоб відсунути камеру подалі
        destination.z = camZ;
        // Помістити камеру в позицію destination
        transform.position = destination;
        // Змінтити розмір orthographicSize камери, щоб земля залишалася в полі зору
        Camera.main.orthographicSize = destination.y + 10;
    }
}
