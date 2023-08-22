using System;
using UnityEngine;

[Serializable]
public class NPCDetails:IComparable<NPCDetails>
{
    public int hour, minute, day;
    public Season season;
    public string targetName;
    public Vector3Int targetGridPosition;

    public AnimationClip targetClip;
    public int priority;//事件执行的优先级(越小优先级越高)
    public bool interactable;

    public NPCDetails(int hour, int minute, int day, Season season, string targetName, Vector3Int targetGridPosition, AnimationClip targetClip, int priority, bool interactable)
    {
        this.hour = hour;
        this.minute = minute;
        this.day = day;
        this.season = season;
        this.targetName = targetName;
        this.targetGridPosition = targetGridPosition;
        this.targetClip = targetClip;
        this.priority = priority;
        this.interactable = interactable;
    }
    public int Time => hour * 100 + minute;
    public int CompareTo(NPCDetails other)
    {
        if(Time == other.Time)
        {
            if (priority > other.priority)
            {
                return 1;
            }
            else
                return -1;
        }
        else if(Time > other.Time)
        {
            return 1;
        }
        else if(Time < other.Time)
        {
            return -1;
        }

        return 0;
    }
}
