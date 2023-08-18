using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractive : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isAni;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAni)
        {
            if (transform.position.x < collision.transform.position.x)
            {
                StartCoroutine(TrunLeft());
            }
            else
            {
                StartCoroutine(TrunRight());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isAni)
        {
            if (transform.position.x > collision.transform.position.x)
            {
                StartCoroutine(TrunLeft());
            }
            else
            {
                StartCoroutine(TrunRight());
            }
        }
    }

    IEnumerator TrunLeft()
    {
        isAni = true;
        for(int i = 0; i < 4;i++)
        {
            transform.GetChild(0).Rotate(0, 0, 2);
            yield return new WaitForSeconds(0.04f);
        }

        for (int i = 0; i <5; i++)
        {
            transform.GetChild(0).Rotate(0, 0, -2);
            yield return new WaitForSeconds(0.04f);
        }
        transform.GetChild(0).Rotate(0, 0, 0);
        isAni = false;
    }
    IEnumerator TrunRight()
    {
        isAni = true;
        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(0).Rotate(0, 0, -2);
            yield return new WaitForSeconds(0.04f);
        }

        for (int i = 0; i < 5; i++)
        {
            transform.GetChild(0).Rotate(0, 0, 2);
            yield return new WaitForSeconds(0.04f);
        }
        transform.GetChild(0).Rotate(0, 0, 0);
        isAni = false;
    }
}
