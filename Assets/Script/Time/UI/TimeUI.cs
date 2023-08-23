using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class TimeUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private RectTransform dayNightImage;
    [SerializeField] private RectTransform clockParent;
    [SerializeField] private TextMeshProUGUI gameData;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image seasonImage;

    [SerializeField] private Sprite[] seasonSprite;

    private List<GameObject> clockChild = new List<GameObject>();
    private void Awake()
    {
        for(int i = 0;i<clockParent.childCount;i++)
        {
            clockChild.Add(clockParent.GetChild(i).gameObject);
            clockParent.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateEvent += OnGameDateEvent;
    }
    private void OnDisable()
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateEvent -= OnGameDateEvent;
    }

    private void OnGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        gameData.text = year.ToString() +"Äê"+month.ToString()+"ÔÂ"+day.ToString()+"ÈÕ";
        seasonImage.sprite = seasonSprite[(int)season];

        SwithHourImage(hour);
        SwithDayNightImage(hour);
    }

    private void OnGameMinuteEvent(int minute, int hour, int day, Season season)
    {
       timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
    }
    private void SwithHourImage(int hour)
    {
        int index = hour / 4;
        if(index == 0)
        {
            foreach(var it in clockChild)
            {
                it.gameObject.SetActive(false);
            }
            clockChild[0].gameObject.SetActive(true);
        }
       else
        {
            for(int i = 0;i< clockChild.Count; i++)
            {
                if(i < index+1)
                {
                    clockChild[i].gameObject.SetActive(true);
                }
                else
                {
                    clockChild[i].gameObject.SetActive(false);
                }
            }
        }
    }
    private void SwithDayNightImage(int hour)
    {
        Vector3 newRotate = new Vector3(0, 0, hour*15-90);
        dayNightImage.DORotate(newRotate, 1f, RotateMode.Fast);
    }
}
