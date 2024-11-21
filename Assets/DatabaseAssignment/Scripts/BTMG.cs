using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BTMG : MonoBehaviour
{
    public GameObject Logout;
    public GameObject Option;
    public GameObject UserInfoChange;
    public GameObject Unrigister;

   public void LogoutOn()
    {
        Logout.gameObject.SetActive(true);
    }

    public void LogoutOff()
    {
        Logout.gameObject.SetActive(false);
    }

    public void OptionOn()
    {
        Option.gameObject.SetActive(true);
    }

    public void OptionOff()
    {
        Option.gameObject.SetActive(false);
    }


    public void GoLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }


}
