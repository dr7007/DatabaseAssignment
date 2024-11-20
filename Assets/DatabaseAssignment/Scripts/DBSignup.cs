using System.Collections.Generic;
using System.Collections;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBSignup : MonoBehaviour
{
    public class UserInfo
    {
        public string signNickname { get; set; }
        public string signID { get; set; }
        public string signPW { get; set; }
        public string signPhoneNum { get; set; }
        public string signEmail { get; set; }
    }

    [SerializeField] private TMP_InputField signNicknametmp;            //���� �г���
    [SerializeField] private TMP_InputField signIDtmp;                  //���� ID
    [SerializeField] private TMP_InputField signPWtmp;                  //���� ��й�ȣ
    [SerializeField] private TMP_InputField signCorrectPWtmp;           //���� ��й�ȣ Ȯ��
    [SerializeField] private TMP_InputField signPhoneNumtmp;            //���� ��ȭ��ȣ
    [SerializeField] private TMP_InputField signEmailtmp;               //���� �̸���
    [SerializeField] private List<TextMeshProUGUI> signErrorMessages = null;   //���� ���� �ؽ�Ʈ
    [SerializeField] private Image signErrorpopup;                      //���� ���� �˾�
    [SerializeField] private Image signuppopup;                         //���� �˾�
    [SerializeField] private Button signuppopupBtn;                     //���� �˾� ��ư
    [SerializeField] private Button signupBtn;                          //���� ��ư

    //Ȯ�ο� ����
    private string signCorrectPW;     //��й�ȣ Ȯ�� �� ���

    private UserInfo userInfo = new UserInfo();

    private enum SignupError    //���� �� ���� �߻�
    {
        DuplicateID,        //�ߺ� ID
        SignIncorrectPW,    //��й�ȣ�� �ٸ� �� (��й�ȣ Ȯ��)
        None                //�Է�â�� �� ���� ���� ���
    }

    private void SignErrorMessages(SignupError error)
    {
        switch(error)
        {
            case SignupError.DuplicateID:
                signErrorMessages[0].text = "��� ���� ID�Դϴ�.";
                signErrorMessages[2].text = "��� ���� ID�Դϴ�.";
                signErrorMessages[0].gameObject.SetActive(true);
                break;
            case SignupError.SignIncorrectPW:
                signErrorMessages[1].text = "��й�ȣ�� ���� �ʽ��ϴ�.";
                signErrorMessages[2].text = "��й�ȣ�� ���� �ʽ��ϴ�.";
                signErrorMessages[1].gameObject.SetActive(true);
                break;
            case SignupError.None:
                signErrorMessages[2].text = "��� ä���־��ּ���.";
                break;
        }

        //���� ���� �� 2�� �� �˾� ��Ȱ��ȭ
        StartCoroutine(FailSignupPopupDelay());
    }

    private IEnumerator FailSignupPopupDelay()
    {
        yield return new WaitForSeconds(2f);

        signErrorpopup.gameObject.SetActive(false);
    }

    public void OnClickSignupButton()
    {
        StartCoroutine(SignUpCoroutine(userInfo));
    }

    private IEnumerator SignUpCoroutine(UserInfo _userInfo)
    {
        _userInfo.signNickname = signNicknametmp.text;
        _userInfo.signID = signIDtmp.text;
        _userInfo.signPW= signPWtmp.text;
        _userInfo.signPhoneNum = signPhoneNumtmp.text; 
        _userInfo.signEmail = signEmailtmp.text;

        signCorrectPW = signCorrectPWtmp.text;

        string uri = "http://127.0.0.1/DASignup.php";

        WWWForm form = new WWWForm();
        form.AddField("Ni ck", _userInfo.signNickname);
        form.AddField("id", _userInfo.signID);
        form.AddField("password", _userInfo.signPW);
        form.AddField("phonenum", _userInfo.signPhoneNum);
        form.AddField("Email", _userInfo.signEmail);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ProtocolError)  //Protocol ����
            {
                signErrorMessages[2].text = www.error;
                yield break;
            }

            string signResponse = www.downloadHandler.text; //���� ���� �ޱ�

            if(signResponse == "DuplicateID")   //�ߺ� ID�� ��
            {
                SignErrorMessages(SignupError.DuplicateID);
            }
            else if(signResponse == "SignSuccess")
            {
                signErrorMessages[2].text = "���� �Ϸ�";
            }
            else
            {
                SignErrorMessages(SignupError.None);
            }
        }
    }

    //ȸ�� ���� �α�
    //private IEnumerator GetInfoCoroutine()
    //{
    //    string uri = "http://127.0.0.1/DAGetInfo.php";

    //    using (UnityWebRequest www = UnityWebRequest.PostWwwForm(uri, string.Empty))
    //    {
    //        yield return www.SendWebRequest();

    //        if(www.result == UnityWebRequest.Result.ProtocolError)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            Debug.Log(www.downloadHandler.text);
    //            string data = www.downloadHandler.text;

    //            List<UserInfo> userinfos = JsonUtility.FromJson<List<UserInfo>>(data);

    //            foreach(UserInfo userInfo in userinfos)
    //            {
    //                Debug.Log("ID: " + userInfo.signID + ", PW: " + userInfo.signPW + 
    //                    ", PhoneNumber: " + userInfo.signPhoneNum + ", E-Mail: " + userInfo.signEmail);
    //            }
    //        }
    //    }
    //}
}