using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MFarm.Translation
{
    public class TranslationManage : MonoBehaviour
    {
        // Start is called before the first frame update
      public string NowSceneName = string.Empty;
        [SerializeField] private Transform player_Tran;
        private CanvasGroup FadeCanvasGroup;
        private bool isFade;
        private void Awake()
        {
            StartCoroutine(LoadSceneSetActive(NowSceneName));
        }
        private void OnEnable()
        {
            EventHandler.TranslationEvent += OnTranslationEvent;
        }
        private void OnDisable()
        {
            EventHandler.TranslationEvent -= OnTranslationEvent;
        }

        private void OnTranslationEvent(string newSceneName, Vector3 targetPos)
        {
            StartCoroutine(TranlationScene(newSceneName, targetPos));
        }

        private void Start()
        {
            
            FadeCanvasGroup = FindObjectOfType<CanvasGroup>();
        }
        /// <summary>
        /// �л�����
        /// </summary>
        /// <param name="sceneName">Ŀ�곡��</param>
        /// <param name="targetPos">����λ��</param>
        /// <returns></returns>
        IEnumerator TranlationScene(string sceneName,Vector3 targetPos)
        {
            EventHandler.CallBeforeFade(NowSceneName);
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(NowSceneName));
            NowSceneName =sceneName;
            yield return LoadSceneSetActive(sceneName);
            player_Tran.position = targetPos;
            EventHandler.CallAfterFade(NowSceneName);
            yield return Fade(0);
        }
        /// <summary>
        /// ����һ���µĳ���
        /// </summary>
        /// <param name="sceneName">������</param>
        /// <returns></returns>
        IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(newScene);
        }
        /// <summary>
        /// ��������Ч��
        /// </summary>
        /// <param name="targetAlpha">0Ϊ��ף�1Ϊ���</param>
        /// <returns></returns>
        IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            FadeCanvasGroup.blocksRaycasts = true;
            float speed = Mathf.Abs(targetAlpha-FadeCanvasGroup.alpha)/Settings.FadeDuration;
            while(!Mathf.Approximately(FadeCanvasGroup.alpha,targetAlpha))
            {
                FadeCanvasGroup.alpha = Mathf.MoveTowards(FadeCanvasGroup.alpha, targetAlpha, speed *Time.deltaTime);
                yield return null;
            }
            isFade = false;
            FadeCanvasGroup.blocksRaycasts = false;
        }
    }
}