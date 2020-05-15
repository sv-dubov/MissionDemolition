using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 35;  // Кількість хмаринок
    public GameObject cloudPrefab;  // Шаблон для хмар
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; // Мін. масштаб кожної хмарки
    public float cloudScaleMax = 3; // Макс. масштаб кожної хмарки
    public float cloudSpeedMult = 0.5f; // Коефіцієнт швидкості хмарки
    private GameObject[] cloudinstances;
    void Awake()
    {
        // Створити масив для зберігання всіх екземплярів хмаринок
        cloudinstances = new GameObject[numClouds];
        // Знайти батьківський ігровий об’єкт CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        // Створити в циклі задану кількість хмаринок
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            // Створити екземпляр cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            // Вибрати розміщення для хмаринки
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Масштабувати хмарку
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            // Менші хмарки (з меншим значеннямм scaleU) мають бути ближче до землі
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            // Менші хмарки мають бути далі
            cPos.z = 100 - 90 * scaleU;
            // Застосувати отримані значення координат і масштаба до хмарки
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Зроити хмарку дочірньою відносно anchor
            cloud.transform.SetParent(anchor.transform);
            cloudinstances[i] = cloud;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        // Пройти в циклі всі створені хмарки
        foreach (GameObject cloud in cloudinstances)
        {
            // Отримати масштаб і координати хмарки
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Збільшити швидкість для ближніх хмарок
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // Якщо хмарка змістилася занадто далеко влево...
            if (cPos.x <= cloudPosMin.x)
            {
                // Перемістити її далеко вправо
                cPos.x = cloudPosMax.x;
            }
            // Застосувати нові координати до хмарки
            cloud.transform.position = cPos;
        }
    }
}
