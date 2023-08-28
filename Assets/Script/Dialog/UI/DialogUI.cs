using MFarm.Dialog;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class DialogUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject DialogPanel;
    public Image LeftImg;
    public Image RightImg;
    public Text LeftName;
    public Text RightName;
    public GameObject Promapt;
    public Text dialogDescribe;
    private void Awake()
    {
        Promapt.SetActive(false);
    }
    private void OnEnable()
    {
        EventHandler.ShowDialogEvent += OnShowDialogEvent;
    }

    private void OnShowDialogEvent(DialogDetails details)
    {
        StartCoroutine(ShowDialog(details));
    }

    private void OnDisable()
    {
        EventHandler.ShowDialogEvent -= OnShowDialogEvent;
    }
    private IEnumerator ShowDialog(DialogDetails details)
    {
        if(details!=null)
        {
            details.isDone = false;
            DialogPanel.SetActive(true);
            Promapt.SetActive(false);
            dialogDescribe.text = string.Empty;
            if (details.Name != string.Empty)
            {
                if (details.onLeft)
                {
                    LeftImg.gameObject.SetActive(true);
                    RightImg.gameObject.SetActive(false);
                    LeftImg.sprite = details.faceImage;
                    LeftName.text = details.Name;
                }
                else
                {
                    LeftImg.gameObject.SetActive(false);
                    RightImg.gameObject.SetActive(true);
                    LeftImg.sprite = details.faceImage;
                    LeftName.text = details.Name;
                }
            }
            else
            {
                LeftImg.gameObject.SetActive(false);
                RightImg.gameObject.SetActive(false);
                LeftName.gameObject.SetActive(false);
                RightName.gameObject.SetActive(false);
            }
            yield return dialogDescribe.DOText(details.dialogText, 1f).WaitForCompletion();

            details.isDone = true; 
            if(details.hasToPause&& details.isDone)
                Promapt.SetActive(true);

        }
        else
        {
            DialogPanel.SetActive(false);
            yield break;
        }
    }
}
