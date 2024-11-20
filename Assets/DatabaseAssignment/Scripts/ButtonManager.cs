using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // public Button Login_BT;
    public Image SignImage;
    public Image Errorbox;

    
    public void ErrorBoxOff()
    {
        Errorbox.gameObject.SetActive(false);
    }

    public void ErrorBox()
    {
        Errorbox.gameObject.SetActive(true);
    }

    public void IDCheck()
    {
        GameObject.Find("ID_pan").transform.Find("ID_Check_TMP").transform.gameObject.SetActive(true);
    }

    public void SignUpSetActive() // ȸ������ ��ư ���� �� ȸ������ â Ȱ��ȭ
    {
        SignImage.gameObject.SetActive(true);
    }

    public void ToLoginScene()  // ȸ������ ���� Ȥ�� ��ҽ� �ٽ� �α��� â���� �� �ҷ���
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void ToMainScene()  // �α��� ���� �� üũ ǥ�� ������ main���� �ҷ���
    {
        SceneManager.LoadScene("MainScene");
    }
}
