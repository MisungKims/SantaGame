/**
 * @brief 게임 로딩
 * @author 김미성
 * @date 22-05-08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoadManager : MonoBehaviour
{
    #region 변수
    public static string nextScene;

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private float speed = 0.1f;        // 로딩 속도
    #endregion

    #region 유니티 함수
    private void Start()
    {
        progressBar.fillAmount = 0;

        if (nextScene == null)
        {
            nextScene = "SantaVillage";
        }

        StartCoroutine(LoadAsyncScene());
    }
    #endregion

    #region 함수
    /// <summary>
    /// 로딩 씬 호출
    /// </summary>
    /// <param name="sceneName"></param>
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;

        SceneManager.LoadScene("GameLoad");
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(nextScene);
        asyncScene.allowSceneActivation = false;

        float timer = 0.0f;

        while (!asyncScene.isDone)
        {
            yield return null;

            timer += Time.deltaTime * speed;
            if (asyncScene.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncScene.progress, timer);
                if (progressBar.fillAmount >= asyncScene.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                    yield break;
                }
            }
        }

    }
    #endregion
}
