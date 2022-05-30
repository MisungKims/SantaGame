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

    private bool isCapture = false;

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

    private void OnEnable()
    {
        isCapture = false;
    }

    /// <summary>
    /// ��ũ������ ��� �������� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeScreenShot()
    {
        isCapture = true;

        SoundManager.Instance.PlaySoundEffect(ESoundEffectType.screenShot);     // ȿ���� ����

        UIManager.Instance.cameraPanel.SetActive(false);       // ui�� ����

        yield return new WaitForEndOfFrame();


        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string fileName = "SantaGame_" + timestamp + ".png";

        //ScreenCapture.CaptureScreenshot("�ٿ�ε�" + fileName);       // PC ��

        //Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        string albumName = "SantaGame";
        NativeGallery.SaveImageToGallery(texture, albumName, fileName, (success, path) =>
        {
            QuestManager.Instance.Success(questID);        // ������ ��� �������� �����ϸ� ����Ʈ �Ϸ�
        });

       
        UIManager.Instance.cameraPanel.SetActive(true);       // ui�� ����
        isCapture = false;

        // cleanup
        Destroy(texture);
    }

    /// <summary>
    /// MakeScreenShot �ڷ�ƾ�� ����
    /// </summary>
    public void DoMakeScreenShot()
    {
        if(!isCapture) StartCoroutine(MakeScreenShot());
    }
}
