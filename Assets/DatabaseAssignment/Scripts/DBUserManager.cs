using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBUserManager : MonoBehaviour
{
    public class UserInfo
    {
        public string signNickname;
        public string signID;
        public string signPW;
        public string signPhoneNum;
        public string signEmail;
        public string pgold;
        public string plevel;
        public string pexp;
    }

    [SerializeField] private DBPlayerInfo playerinfo;
    [SerializeField] private string pID = string.Empty;
    [SerializeField] private int gold, exp = 0;
    [SerializeField] private int level = 1;

    [SerializeField] private TextMeshProUGUI nickTmp = null;
    [SerializeField] private TextMeshProUGUI goldTmp = null;
    [SerializeField] private TextMeshProUGUI lvlTmp = null;
    [SerializeField] private TextMeshProUGUI expTmp = null;


    private UserInfo userinfo;


    private void Start()
    {
        playerinfo = FindAnyObjectByType<DBPlayerInfo>();
        pID = playerinfo.uid;
        StartCoroutine(TakeUserInfoCoroutine(pID));
    }

    private void UIUpdate()
    {
        nickTmp.text = userinfo.signNickname;
        goldTmp.text = userinfo.pgold;
        lvlTmp.text = userinfo.plevel;
        expTmp.text = userinfo.pexp;
    }

    private IEnumerator TakeUserInfoCoroutine(string _id)
    {
        string uri = "http://127.0.0.1/DAGetInfo.php";

        WWWForm form = new WWWForm();
        form.AddField("id", _id);


        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                string data = www.downloadHandler.text;

                userinfo = JsonUtility.FromJson<UserInfo>(data);

                UIUpdate();
            }
        }
    }

    public void PrintUserInfo()
    {
        Debug.LogFormat("NickName: {0}, Gold: {1}, Level: {2}, Email: {3}",
            userinfo.signNickname, userinfo.pgold, userinfo.plevel, userinfo.pexp);
    }
}
