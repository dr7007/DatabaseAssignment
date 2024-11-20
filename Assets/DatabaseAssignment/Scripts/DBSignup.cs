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
    [SerializeField] private Image signuppopup;                         //���� �˾�
    [SerializeField] private Button signuppopupBtn;                     //���� �˾� ��ư
    [SerializeField] private Button signupBtn;                          //���� ��ư
    [SerializeField] private Button signCancelBtn;                      //��� ��ư

    //Ȯ�ο� ����
    private string signCorrectPW;     //��й�ȣ Ȯ�� �� ���

    private UserInfo userInfo = new UserInfo();

    private enum SignupError    //���� �� ���� �߻�
    {
        None,               //�Է�â �ϳ��� ���������
        DuplicateNickname,  //�ߺ� �г���
        DuplicateID,        //�ߺ� ID
        SignIncorrectPW     //��й�ȣ�� �ٸ� �� (��й�ȣ Ȯ��)
    }

    private void SignErrorMessages(SignupError error)
    {
        switch(error)
        {
            case SignupError.DuplicateNickname:
                signErrorMessages[0].text = "�̹� ��� ���� �̸��Դϴ�.";
                break;
            case SignupError.DuplicateID:
                signErrorMessages[1].text = "�̹� ��� ���� ID�Դϴ�.";
                break;
            case SignupError.SignIncorrectPW:
                signErrorMessages[2].text = "��й�ȣ�� ���� �ʽ��ϴ�.";
                break;
            case SignupError.None:
                signErrorMessages[3].text = "��� ä���־��ּ���.";
                break;
        }
    }

    public void OnClickSignupCanvasButton()
    {
        signuppopup.gameObject.SetActive(true);
    }

    public void OnClickSignupButton()
    {
        StartCoroutine(SignUpCoroutine(userInfo));
    }

    public void OnClickSignCancelButton()
    {
        signuppopup.gameObject.SetActive(false);
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
        form.AddField("Nick", _userInfo.signNickname);
        form.AddField("id", _userInfo.signID);
        form.AddField("password", _userInfo.signPW);
        form.AddField("phonenum", _userInfo.signPhoneNum);
        form.AddField("Email", _userInfo.signEmail);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ProtocolError)  //Protocol ����
            {
                signErrorMessages[3].text = www.error;
            }   
            else //�α��� ����
            {
                signErrorMessages[3].text = "���ԵǼ̽��ϴ�.";
                signuppopup.gameObject.SetActive(false);
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