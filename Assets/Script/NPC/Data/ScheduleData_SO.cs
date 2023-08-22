using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Schedule" , menuName ="Schedule/Schedule_SO")]
public class ScheduleData_SO : ScriptableObject
{
    // Start is called before the first frame update
  
    public List<NPCDetails> nPCDetails = new List<NPCDetails>();
}
