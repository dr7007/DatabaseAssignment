using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.UI;


public class SignupManager : MonoBehaviour
{
    public class UserInfo
    {
        public string id { get; set; }
        public string pw { get; set; }
    }

    [SerializeField]
    private string id = string.Empty;
    [SerializeField]
    private string password = string.Empty;

    [SerializeField]
    private Button loginButton = null;


    private void Start()
    {

    }

    public void OnValueChangedID(string _id)
    {
        id = _id;
    }
    public void OnValueChangedPW(string _pw)
    {
        password = _pw;
    }

    public void ClickLogin()
    {
        StartCoroutine(SignUpCoroutine(id, password));
    }

    public void ClickUserInfo()
    {
        StartCoroutine(GetInfoCoroutine());
    }

    private IEnumerator SignUpCoroutine(string _id, string _password)
    {
        string uri = "http://127.0.0.1/DASignup.php";

        WWWForm form = new WWWForm();
        form.AddField("id", _id);
        form.AddField("password", _password);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("SignUp Success : " + _id + "(" + _password + ")");
            }
        }
    }

    private IEnumerator GetInfoCoroutine()
    {
        string uri = "http://127.0.0.1/DAGetInfo.php";

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(uri, string.Empty))
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

                List<UserInfo> userinfos = JsonUtility.FromJson<List<UserInfo>>(data);
                

                foreach (UserInfo userinfo in userinfos)
                {
                    Debug.Log("ID : " + userinfo.id + ", PW : " + userinfo.pw);
                }
            }
        }
    }

}
