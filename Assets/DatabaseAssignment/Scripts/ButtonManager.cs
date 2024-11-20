using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // public Button Login_BT;
    public Image SignImage;

    public void IDCheck()
    {
        GameObject.Find("ID_pan").transform.Find("ID_Check_TMP").transform.gameObject.SetActive(true);
    }

    public void SignUpSetActive() // 회원가입 버튼 눌릴 시 회원가입 창 활성화
    {
        SignImage.gameObject.SetActive(true);
    }

    public void ToLoginScene()  // 회원가입 생성 혹은 취소시 다시 로그인 창으로 씬 불러옴
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void ToMainScene()  // 로그인 성공 후 체크 표시 누르면 main씬을 불러옴
    {
        SceneManager.LoadScene("MainScene");
    }
}
