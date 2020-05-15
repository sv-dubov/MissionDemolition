using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //Одиночка

    [Header("Set in Inspector")]
    public float minDist = 0.1f;
    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;
    void Awake()
    {
        S = this; // Встановити посилання на об’єкт-одиночку
        // Отримати посилання на LineRenderer
        line = GetComponent<LineRenderer>();
        // Вимкнути LineRenderer, доки він не знадобиться
        line.enabled = false;
        // Ініціалізувати список точок
        points = new List<Vector3>();
    }
    // Це властивість (тобто метод, який маскується під поле)
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null ) {
                // Якщо поле _poi містить дійсне посилання, то скинути решту параметрів у початковий стан
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    // Цей метод можна викликати безпосередньо, щоб стерти лінію
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        // Викликається для дододаваня точки в лінії
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            // Якщо точка недостатньо далека від попередньої, то просто вийти
            return;
        }
        if (points.Count == 0) { // Якщо це точка запуска...
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Для визначення
            // ...додати додатковий фрагмент лінії, щоб допомогти краще прицілитися в подальшому
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Встановити перші дві точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // Включити LineRenderer
            line.enabled = true;
        }
        else
        {
            // Звичайна послідовність додавання точки
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    // Повертає розміщення останньої доданої точки
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                // Якщо точок нема, то повернути Vector3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
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
        if (poi == null)
        {
            // Якщо властивість poi містить порожнє значення, то знайти потрібний об’єкт
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; // Вийти, якщо потрібний об’єкт не знайдений
                }
            }
            else
            {
                return; // Вийти, якщо потрібний об’єкт не знайдений
            }
        }
        // Якщо потрібний об’єкт знайдений, то спробувати додати точку з його координатами в кожному FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            // Якщо FollowCam.POI містить null, записати nulll в poi
            poi = null;
        }
    }
}
