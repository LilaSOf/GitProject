using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShow : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer spriteRenderer;
    private SpriteRenderer shadowRenderer;

    private void Awake()
    {
        shadowRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        shadowRenderer.sprite = spriteRenderer.sprite;
        shadowRenderer.color = new Color(0, 0, 0, 0.3f);
    }
}
