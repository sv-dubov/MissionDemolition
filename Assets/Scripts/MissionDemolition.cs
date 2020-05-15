using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameMode
{
    idle,
    playing,
    levelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // прихований об’єкт-одиночка

    [Header("Set in Inspector")]
    public Text uitLevel;   // Посилання на об’єкт UIText_Level
    public Text uitShots;   // Посилання на об’єкт UIText_Shots
    public Text uitButton;  // Посилання на дочірній об’єкт Text в UIButton_View
    public Vector3 castlePos;   // Розміщення замка
    public GameObject[] castles;    // Масив замків

    [Header("Set Dynamically")]
    public int level; // Поточний рівень
    public int levelMax; // Кількість рівнів
    public int shotsTaken;
    public GameObject castle; // Поточний замок
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // Режим FollowCam
    // Start is called before the first frame update
    void Start()
    {
        S = this; // Визначити об’єкт-одиночку
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    void StartLevel()
    {
        // Знищити попередній замок, якщо він існує
        if (castle != null)
        {
            Destroy(castle);
        }
        // Знищити попередні снаряди, якщо вони існують
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }
        // Створити новий замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        // Перевстановити камеру в початкову позицію
        SwitchView("Show Both");
        ProjectileLine.S.Clear();
        // Скинути ціль
        Goal.goalMet = false;
        UpdateGUI();
        mode = GameMode.playing;
    }
    void UpdateGUI()
    {
        // Показати дані в елементах ПІ
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
        // Перевірити завершення рівня
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            // Змінити режим, щоб зупнити перевірку завершення рівня
            mode = GameMode.levelEnd;
            // Зменшити масштаб
            SwitchView("Show Both");
            // Почати новий рівень через 3 секунди
            Invoke("NextLevel", 3f);
        }
    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }
    // Статичний метод, що дозволяє з будь-якого кода збільшити shotsTaken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
