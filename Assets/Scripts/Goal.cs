using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Статичне поле, доступне будь-якому іншому коду
    static public bool goalMet = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        // Коли в область дії триггера попадає щось, перевірити, чи є це "щось” снарядом
        if (other.gameObject.tag == "Projectile")
        {
            // Якщо це снаряд, присвоїти полю goalMet значення true
            Goal.goalMet = true;
            // Також змінити альфа-канал кольору, щоб збільшити непрозорість
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
