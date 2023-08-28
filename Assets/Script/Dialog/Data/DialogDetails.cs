using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MFarm.Dialog
{
    [System.Serializable]
    public class DialogDetails
    {
        [Header("∂‘ª∞œÍ«È")]
        public Sprite faceImage;
        public bool onLeft;
        public string Name;

        [TextArea]
        public string dialogText;
        public bool hasToPause;
        public bool isDone;

        public UnityEvent afterTalkEvent;
    }
}
