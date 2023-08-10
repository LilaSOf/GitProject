using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MFarm.Translation
{
    public class TranslationManage : MonoBehaviour
    {
        // Start is called before the first frame update
 
        [SerializeField] private Transform player_Tran;
        private CanvasGroup FadeCanvasGroup;
        private bool isFade;
        [SerializeField]
        [Header("当前场景的名称")]
        public string NowSceneName;
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
    
        private IEnumerator Start()
        {
            EventHandler.CallSceneNameTransfer(NowSceneName);
            FadeCanvasGroup = FindObjectOfType<CanvasGroup>();
            yield return null;
            EventHandler.CallAfterFade("");
           
        }
        /// <summary>
        /// 切换场景
        /// </summary>
        /// <param name="sceneName">目标场景</param>
        /// <param name="targetPos">场景位置</param>
        /// <returns></returns>
        IEnumerator TranlationScene(string sceneName,Vector3 targetPos)
        {
            EventHandler.CallBeforeFade(NowSceneName);
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(NowSceneName));
            NowSceneName =sceneName;
            EventHandler.CallSceneNameTransfer(NowSceneName);
            yield return LoadSceneSetActive(sceneName);
            player_Tran.position = targetPos;
            EventHandler.CallAfterFade(NowSceneName);
            yield return Fade(0);
        }
        /// <summary>
        /// 加载一个新的场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <returns></returns>
        IEnumerator LoadSceneSetActive(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
            Scene newScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(newScene);
        }
        /// <summary>
        /// 场景遮罩效果
        /// </summary>
        /// <param name="targetAlpha">0为变白，1为变黑</param>
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