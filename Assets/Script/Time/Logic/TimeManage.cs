using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MFarm.TimeManage
{
    public class TimeManage : Singleton<TimeManage>
    {
        // Start is called before the first frame update
        private int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
        public bool gameClockPause;
        private Season gameSeason = Season.´ºÌì;
        private int mouthInSeason = 3;
        private float timeHold;

        public TimeSpan GameTime => new TimeSpan(gameHour,gameMinute,gameSecond);
        protected override void  Awake()
        {
            base.Awake();
            NewGameTime();
        }
        private void Start()
        {
            EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour,gameDay,gameSeason);
        }
        private void Update()
        {
            if (!gameClockPause)
            {
                timeHold = Time.time;
                if (timeHold >= Settings.secondTreahsHold)
                {
                    timeHold -= Settings.secondTreahsHold;
                    UpdateGameTime();
                }
            }
            if (Input.GetKey(KeyCode.T))
            {
                for (int i = 0; i < 60; i++)
                {
                    UpdateGameTime();
                }
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                gameDay++;

                EventHandler.CallGameDayEvent(gameDay, gameSeason);
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
        }

        private void NewGameTime()
        {
            gameSecond = 0;
            gameMinute = 0;
            gameHour = 7;
            gameDay = 1;
            gameMonth = 1;
            gameYear = 2023;
            gameSeason = Season.´ºÌì;
        }
        public void UpdateGameTime()
        {
            gameSecond++;
            if (gameSecond > Settings.secondHold)
            {
                gameSecond = 0;
                gameMinute++;
                if (gameMinute > Settings.minuteHold)
                {
                    gameMinute = 0;
                    gameHour++;
                    if (gameHour > Settings.hourHold)
                    {
                        gameHour = 0;
                        gameDay++;
                        if (gameDay > Settings.dayHold)
                        {
                            gameDay = 1;
                            gameMonth++;
                            if (gameMonth > 12)
                            {
                                gameMonth = 1;
                            }
                            mouthInSeason--;
                            if (mouthInSeason == 0)
                            {
                                mouthInSeason = 3;
                                int seasonNumber = (int)gameSeason;
                                seasonNumber++;
                                if (seasonNumber > Settings.seasonHold)
                                {
                                    gameYear++;
                                    seasonNumber = 0;
                                }
                                gameSeason = (Season)seasonNumber;
                            }
                        }
                        EventHandler.CallGameDayEvent(gameDay, gameSeason);
                    }
                    EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
                }
                EventHandler.CallGameMinuteEvent(gameMinute, gameHour,gameDay,gameSeason);
            }
        }
    }
}