using System.Collections;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBSignup : MonoBehaviour
{
    [SerializeField] private TMP_InputField signNicknametmp;        //���� �г���
    [SerializeField] private TMP_InputField signIDtmp;              //���� ID
    [SerializeField] private TMP_InputField signPasswordtmp;        //���� ��й�ȣ
    [SerializeField] private TMP_InputField signCorrectpasswordtmp; //���� ��й�ȣ Ȯ��
    [SerializeField] private TMP_InputField signPhoneNumtmp;        //���� ��ȭ��ȣ
    [SerializeField] private TMP_InputField signEmailtmp;           //���� �̸���
    [SerializeField] private TextMeshProUGUI signerrorMessage;      //���� ���� �ؽ�Ʈ
    [SerializeField] private Button signupBtn;                      //���� ��ư

    //Ȯ�ο� ����
    private string signNickname;            //�ߺ� �г���
    private string signID;                  //�ߺ� ���̵�
    private string signPassword;            //��й�ȣ Ȯ�� �� ���
    private string signCorrectPassword;     //��й�ȣ Ȯ�� �� ���
    private string signPhoneNum;            //�ߺ� ��ȭ��ȣ
    private string signEmail;               //�ߺ� �̸���
    
    private void OnClickSignupButton()
    {
        StartCoroutine(SignUpCoroutine(
            signNickname, signID, signPassword, signCorrectPassword, signPhoneNum, signEmail));
    }

    private IEnumerator SignUpCoroutine(
        string signNickname, string signID, string signPassword, string signCorrectPassword, string signPhoneNum, string signEmail)
    {
        signNickname = signNicknametmp.text;
        signID = signIDtmp.text;
        signPassword = signPasswordtmp.text;
        signCorrectPassword = signCorrectpasswordtmp.text;
        signPhoneNum = signPhoneNumtmp.text;
        signEmail = signEmailtmp.text;

        string uri = "http://127.0.0.1/DASignup.php";

        WWWForm form = new WWWForm();
        form.AddField("Nick", signNickname);
        form.AddField("id", signID);
        form.AddField("password", signPassword);
        form.AddField("phonenum", signPhoneNum);
        form.AddField("Email", signEmail);

        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
            }
            else
            {

            }
        }
    }
}