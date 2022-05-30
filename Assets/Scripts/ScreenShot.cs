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

    private bool isCapture = false;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private void OnEnable()
    {
        isCapture = false;
    }

    /// <summary>
    /// 스크린샷을 찍어 갤러리에 저장
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeScreenShot()
    {
        isCapture = true;

        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.screenShot);     // 효과음 실행

        UIManager.Instance.cameraPanel.SetActive(false);       // ui를 숨김

        yield return new WaitForEndOfFrame();


        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string fileName = "SantaGame_" + timestamp + ".png";

        //ScreenCapture.CaptureScreenshot("다운로드" + fileName);       // PC 용

        //Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        string albumName = "SantaGame";
        NativeGallery.SaveImageToGallery(texture, albumName, fileName, (success, path) =>
        {
            QuestManager.Instance.Success(questID);        // 사진을 찍어 갤러리에 저장하면 퀘스트 완료
        });

       
        UIManager.Instance.cameraPanel.SetActive(true);       // ui를 숨김
        isCapture = false;

        // cleanup
        Destroy(texture);
    }

    /// <summary>
    /// MakeScreenShot 코루틴을 실행
    /// </summary>
    public void DoMakeScreenShot()
    {
        if(!isCapture) StartCoroutine(MakeScreenShot());
    }
}
