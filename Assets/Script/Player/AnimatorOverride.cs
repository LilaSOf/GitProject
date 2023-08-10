using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator[] animators;
    [SerializeField]private SpriteRenderer spriteRenderer;
    [SerializeField]
    private List<AnimatorType> animatorType = new List<AnimatorType>();
    
    private Dictionary<string,Animator> animatorNameDir = new Dictionary<string,Animator>();    
    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach(var item in animators)
        {
            animatorNameDir.Add(item.name, item);
        }
    }
    private void OnEnable()
    {
        EventHandler.ItemSecletEvent += OnIsSecletEvent;
    }

    private void OnIsSecletEvent(ItemDetails itemDetails, bool isSeclet)
    {
        //WORKFLOW:不同的道具在这里补全类型
        PartType par = itemDetails.itemType switch 
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            ItemType.HoeTool => PartType.Hoe,
            ItemType.WaterTool => PartType.Water,
            _ => PartType.None,
        };
        if(!isSeclet)
        {
            par = PartType.None;
            spriteRenderer.enabled = false;
        }
        else if(itemDetails.canCarried)
        {
            spriteRenderer.sprite = itemDetails.itemOnWorldSprite;
            spriteRenderer.enabled = isSeclet;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
        SwitchAniamtor(par);
    }

    private void OnDisable()
    {
        EventHandler.ItemSecletEvent -= OnIsSecletEvent;
    }

    private void SwitchAniamtor(PartType partType)
    {
        foreach (var item in animatorType)
        {
            if(item.partType == partType)
            {
                animatorNameDir[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
