using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class BTMG : MonoBehaviour
{
    public GameObject Logout;
    public GameObject Option;
    public GameObject UserInfoChange;
    //public GameObject Unrigister;
    public GameObject UnrigisterPanel;
    public GameObject realUnrigister;


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

    public void UnrigisterClick()
    {
        UnrigisterPanel.gameObject.SetActive(true);
    }

    public void RealUnrigister()
    {
        //realUnrigister.gameObject.SetActive(true);
        StartCoroutine("WaitMind");
    }

    IEnumerator WaitMind()
    {
        realUnrigister.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        GoLoginScene();
    }

    public void UnrigisterCancle()
    {
        UnrigisterPanel.gameObject.SetActive(false);
        Option.gameObject.SetActive(false);
    }

}
