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
    #region ����
    private int questID = 0;

    private bool isCapture = false;

    // ĳ��
    private UIManager uIManager;
    private SoundManager soundManager;
    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    #endregion

    #region ����Ƽ �Լ�
    private void Awake()
    {
        uIManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
    }

    private void OnEnable()
    {
        isCapture = false;
    }
    #endregion

    #region �Լ�
    /// <summary>
    /// ��ũ������ ��� �������� ����
    /// </summary>
    /// <returns></returns>
    private IEnumerator MakeScreenShot()
    {
        isCapture = true;

        soundManager.PlaySoundEffect(ESoundEffectType.screenShot);     // ȿ���� ����

        uIManager.mainCanvas.enabled = false;       // ui�� ����

        yield return waitForEndOfFrame;

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string fileName = "SantaGame_" + timestamp + ".png";

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        string albumName = "SantaGame";
        NativeGallery.SaveImageToGallery(texture, albumName, fileName, (success, path) =>
        {
            QuestManager.Instance.Success(questID);        // ������ ��� �������� �����ϸ� ����Ʈ �Ϸ�
        });

        uIManager.mainCanvas.enabled = true;
        isCapture = false;

        // cleanup
        Destroy(texture);
    }

    /// <summary>
    /// MakeScreenShot �ڷ�ƾ�� ���� (�ν����Ϳ��� ȣ��)
    /// </summary>
    public void DoMakeScreenShot()
    {
        if (!isCapture) StartCoroutine(MakeScreenShot());
    }
    #endregion
}
