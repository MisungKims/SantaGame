/**
 * @brief 게임 화면 캡처
 * @author 김미성
 * @date 22-05-08
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;


public class ScreenShot : MonoBehaviour
{
    private int questID = 0;

    /// <summary>
    /// 스크린샷을 찍어 갤러리에 저장
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeScreenShot()
    {
        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.screenShot);     // 효과음 실행

        yield return new WaitForEndOfFrame();

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string fileName = "BRUNCH-SCREENSHOT-" + timestamp + ".png";

        //ScreenCapture.CaptureScreenshot("다운로드" + fileName);       // PC 용

        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        string albumName = "BRUNCH";
        NativeGallery.SaveImageToGallery(texture, albumName, fileName, (success, path) =>
        {
            QuestManager.Instance.Success(questID);        // 퀘스트 완료

            Debug.Log(success);
            Debug.Log(path);
        });

        // cleanup
        Destroy(texture);
    }

    /// <summary>
    /// MakeScreenShot 코루틴을 실행
    /// </summary>
    public void DoMakeScreenShot()
    {
        StartCoroutine(MakeScreenShot());
    }
}
