using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonManager : MonoBehaviour
{
    // public Button Login_BT;
    public Image SignImage;
    public Image Errorbox;
    public DBPlayerInfo playerinfo = null;

    private void Awake()
    {
        playerinfo = FindAnyObjectByType<DBPlayerInfo>();
    }

    public void LogoutPanelOn()
    {
        GameObject.Find("Logout_BT").transform.Find("Logout_Panel").transform.gameObject.SetActive(true);
    }
    
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

    public void Popup()
    {
        //panel.SetActive(true);
        
        // TexTTT.InventoryT(string ) //// InventoryInfo_Panel 오브젝트안에 들어있는 TexTTT스크립트의 InventoryT함수를 가져온다.
        // InventoryT 함수는 () 안의 변수를 각 Text란에 출력
    }
}
