using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DBLogin : MonoBehaviour
{
    public GameObject Login_Access;
    public GameObject Login_Fail;

    public TMP_InputField ID_Input;
    public TMP_InputField PW_Input;
    public Button Sign_up_BT;

    // 임의 변수
    private string user = "D";
    private string password = "1";

    public void LoginButtonClick()
    {
        if(ID_Input.text == user && PW_Input.text == password)
        {
            Debug.Log("로그인 성공");
            Login_Access.SetActive(true);
        }
        else
        {
            Debug.Log("로그인 실패");
            Login_Fail.SetActive(true);
        }
    }
    public void OnCheck()
    {
        Debug.Log("확인");
        transform.parent.gameObject.SetActive(false);
    }
}
