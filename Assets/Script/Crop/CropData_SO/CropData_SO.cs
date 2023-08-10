using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CropData",menuName ="Crop/CropData")]
public class CropData_SO : ScriptableObject
{
    public List<CropDetails> cropDetailsList = new  List<CropDetails>();
   
}
