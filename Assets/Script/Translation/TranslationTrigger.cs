using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Translation
{

    public class TranslationTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        [SceneName]
        public string newSceneName;
        public Vector3 newPos;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                EventHandler.CallTranslationEvent(newSceneName, newPos);
            }
        }
    }
}

