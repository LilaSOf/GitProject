using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemFader[] itemFader = collision.GetComponentsInChildren<ItemFader>();
        if (itemFader.Length > 0)
        {
            foreach (ItemFader item in itemFader)
            {
                item.FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ItemFader[] itemFader = collision.GetComponentsInChildren<ItemFader>();
        if (itemFader.Length > 0)
        {
            foreach (ItemFader item in itemFader)
            {
                item.FadeIn();
            }
        }
    }
}
