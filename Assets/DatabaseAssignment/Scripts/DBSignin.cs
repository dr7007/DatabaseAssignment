using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBSignin : MonoBehaviour
{
    [SerializeField] private TMP_InputField idtmp;   //���̵� �Է� UI InputField
    [SerializeField] private TMP_InputField pwtmp;   //��й�ȣ �Է� UI InputField
    [SerializeField] private TextMeshProUGUI errortmp;  //�α��� ���� �� ���� ����� �ؽ�Ʈ
    [SerializeField] private Image loginpopup;  //�α��� ����, ���п� ���� �˾�
    [SerializeField] private Button transformsceneBtn;  //mainscene �̵� ��ư

    private DBPlayerInfo playerInfo = null;

    private string id;  //idtmp�� �Է��� ���� ����ϱ� ����
    private string pw;    //passwordtmp�� �Է��� ���� ����ϱ� ����

    private const string loginUri = "http://127.0.0.1/DASignin.php";   //�� ������ php����

    private void Start()
    {
        playerInfo = FindAnyObjectByType<DBPlayerInfo>();
    }

    private enum SigninError //�α��� ���� �߻� ��
    {
        None,   //idtmp, passwordtmp �Ѵ� or �� �� �ϳ��� ���� ���� ���
        IDNotFound, //idtmp ���� DB�� ID���� �������� �ʴ� ���
        IncorrectPW,  //passwordtmp ���� DB�� Password���� �������� �ʴ� ���
        ConnectionError //���� ���� ��ü�� ������ ���
    }

    private void ErrorMessage(SigninError error) //�� ��Ȳ�� ���� ���� �޼���
    {
        switch (error)
        {
            case SigninError.None:
                errortmp.text = "ID �Ǵ� ��й�ȣ�� �Է����� �ʾҽ��ϴ�.";
                break;
            case SigninError.IDNotFound:
                errortmp.text = "���̵� �������� �ʽ��ϴ�.";
                break;
            case SigninError.IncorrectPW:
                errortmp.text = "��й�ȣ�� ���� �ʽ��ϴ�.";
                break;
            case SigninError.ConnectionError:
                errortmp.text = "������ ����Ǿ����� �ʽ��ϴ�.";
                break;
            default:
                errortmp.text = "";
                break;
        }

        //�α��� ���� �� 1�� �� �˾� ��Ȱ��ȭ
        StartCoroutine(FailLoginPopupDelay());
    }

    private IEnumerator FailLoginPopupDelay()
    {
        yield return new WaitForSeconds(2f);

        loginpopup.gameObject.SetActive(false);
    }

    public void OnLoginButtonClicked()  //Login��ư Ŭ�� ��
    {
        errortmp.text = "�α��� ��...";
        StartCoroutine(LoginCoroutine(id, pw));
        loginpopup.gameObject.SetActive(true);
        transformsceneBtn.gameObject.SetActive(false);
    }

    private IEnumerator LoginCoroutine(string id, string password)  //�α��� �ڷ�ƾ
    {
        id = idtmp.text;    //id�� ���� �ʱ�ȭ
        password = pwtmp.text;    //password���� �ʱ�ȭ

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))  //���� ����� ��
        {
            ErrorMessage(SigninError.None);  //None �����޼��� ǥ��
            yield break;
        }

        WWWForm loginform = new WWWForm();   //WWWForm Ŭ����: Unity���� HTTP ��û ���� �� �� ������ ���� ���
        loginform.AddField("id", id);   //ID �ǿ� idtmp���� �߰�
        loginform.AddField("pw", password);   //PASSWORD �ǿ� passwordtmp���� �߰�

        using (UnityWebRequest www = UnityWebRequest.Post(loginUri, loginform))
        {
            yield return www.SendWebRequest();  //��û ������

            //���� ���� �߻� ��
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.DataProcessingError)
            {
                ErrorMessage(SigninError.ConnectionError);
                yield break;
            }

            //���� ���� ���� ��
            string response = www.downloadHandler.text;  //���� ���� �ޱ�

            if (response == "IDNotFound")    //ID�� �������� ���� ��
            {
                ErrorMessage(SigninError.IDNotFound);
            }
            else if (response == "IncorrectPassword")   //��й�ȣ�� �������� ���� ��
            {
                ErrorMessage(SigninError.IncorrectPW);
            }
            else if (response == "Success") //�α��� ���� ��
            {
                errortmp.text = "�α��� ����";
                playerInfo.uid = id;
                transformsceneBtn.gameObject.SetActive(true);
            }
            else
            {
                ErrorMessage(SigninError.None);
            }
        }
    }
}