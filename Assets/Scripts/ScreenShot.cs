/**
 * @brief ���� ȭ�� ĸó
 * @author ��̼�
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
    /// ��ũ������ ��� �������� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeScreenShot()
    {
        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.screenShot);     // ȿ���� ����

        yield return new WaitForEndOfFrame();

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string fileName = "BRUNCH-SCREENSHOT-" + timestamp + ".png";

        //ScreenCapture.CaptureScreenshot("�ٿ�ε�" + fileName);       // PC ��

        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();

        string albumName = "BRUNCH";
        NativeGallery.SaveImageToGallery(texture, albumName, fileName, (success, path) =>
        {
            QuestManager.Instance.Success(questID);        // ����Ʈ �Ϸ�

            Debug.Log(success);
            Debug.Log(path);
        });

        // cleanup
        Destroy(texture);
    }

    /// <summary>
    /// MakeScreenShot �ڷ�ƾ�� ����
    /// </summary>
    public void DoMakeScreenShot()
    {
        StartCoroutine(MakeScreenShot());
    }
}
