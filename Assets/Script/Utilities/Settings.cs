using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    // Start is called before the first frame update
    public const float itemFadeDuration = 0.35f;
    public const float fadeAlaph = 0.45f;

    //时间相关
    public const float secondTreahsHold = 0.05f;
    public const int secondHold = 59;
    public const int minuteHold = 59;
    public const int hourHold = 23;
    public const int dayHold = 30;
    public const int seasonHold = 3;

    //场景切换
    public const float FadeDuration = 1.5f;

    //NPC网格坐标
    public const float gridCellSize = 1f;
    public const float gridCellDiagonalSize = 1.41f;

    public const float pixelSize = 0.05f; //像素大小

    public const float animationChange = 3f;//间隔多长时间后播放动画

    public const int maxCellSize = 999;//最大网格坐标
}
