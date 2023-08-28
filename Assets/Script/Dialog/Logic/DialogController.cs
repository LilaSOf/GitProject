using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MFarm.Dialog
{
    [RequireComponent(typeof(NPCMovement))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class DialogController : MonoBehaviour
    {
        private NPCMovement npcMovement => GetComponent<NPCMovement>();
        public List<DialogDetails> dialogDetailsList = new List<DialogDetails>();
        [Header("����ʱҪ���õ��¼�")]
        public UnityEvent OnFinshEvent;

        private Stack<DialogDetails> dialogueStack = new Stack<DialogDetails>();

        public bool canTalk;
        [Header("�Ի���ʶ")]
        private GameObject Sign;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
               
                canTalk = npcMovement.interactable && !npcMovement.isMoving;

            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            canTalk = false;
            EventHandler.CallShowDialogEvent(null);
        }
        private void Awake()
        {
            Sign = transform.GetChild(1).gameObject;
            FillStack();
        }
        private void FillStack()
        {
            for (int i = dialogDetailsList.Count - 1; i > -1; i--)
            {
                dialogDetailsList[i].isDone = false;
                dialogueStack.Push(dialogDetailsList[i]);
            }
        }
        private void Update()
        {
            Sign.SetActive(canTalk);
            if(canTalk && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DialogRoutine());
            }
        }
        private IEnumerator DialogRoutine()
        {
            canTalk = false;
            if (dialogueStack.TryPop(out DialogDetails result))
            {
                EventHandler.CallShowDialogEvent(result);
                //����UI��ʾ�Ի�
                yield return new WaitUntil(() => result.isDone == true);
                canTalk = true;
            }
            else
            {
                FillStack();
                canTalk = true;
                EventHandler.CallShowDialogEvent(null);
                OnFinshEvent?.Invoke();
            }
        }
    }
}
