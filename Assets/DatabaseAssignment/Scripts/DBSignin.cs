using System.Collections;

using UnityEngine.SceneManagement;
using UnityEngine;

using TMPro;

using UnityEngine.Networking;
using UnityEngine.UI;

public class DBSignin : MonoBehaviour
{
    [SerializeField] private TMP_InputField idtmp;   //���̵� �Է� UI InputField
    [SerializeField] private TMP_InputField passwordtmp;   //��й�ȣ �Է� UI InputField
    [SerializeField] private TextMeshProUGUI errortmp;  //�α��� ���� �� ���� ����� �ؽ�Ʈ
    [SerializeField] private Image loginpopup;  //�α��� ����, ���п� ���� �˾�

    private string id;  //idtmp�� �Է��� ���� ����ϱ� ����
    private string password;    //passwordtmp�� �Է��� ���� ����ϱ� ����

    private const string loginUri = "http://127.0.0.1/DASignin.php";   //�� ������ php����

    private enum LoginError //�α��� ���� �߻� ��
    {
        None,   //idtmp, passwordtmp �Ѵ� or �� �� �ϳ��� ���� ���� ���
        IDNotFound, //idtmp ���� DB�� ID���� �������� �ʴ� ���
        IncorrectPassword,  //passwordtmp ���� DB�� Password���� �������� �ʴ� ���
        ConnectionError //���� ���� ��ü�� ������ ���
    }

    private void ErrorMessage(LoginError error) //�� ��Ȳ�� ���� ���� �޼���
    {
        switch (error)
        {
            case LoginError.None:
                errortmp.text = "ID �Ǵ� ��й�ȣ�� �Է����� �ʾҽ��ϴ�.";
                break;
            case LoginError.IDNotFound:
                errortmp.text = "���̵� �������� �ʽ��ϴ�.";
                break;
            case LoginError.IncorrectPassword:
                errortmp.text = "��й�ȣ�� ���� �ʽ��ϴ�.";
                break;
            case LoginError.ConnectionError:
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
        StartCoroutine(LoginCoroutine(id, password));
        errortmp.text = "�α��� ��...";
        loginpopup.gameObject.SetActive(true);
    }

    private IEnumerator LoginCoroutine(string id, string password)  //�α��� �ڷ�ƾ
    {
        id = idtmp.text;    //id�� ���� �ʱ�ȭ
        password = passwordtmp.text;    //password���� �ʱ�ȭ

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(password))  //���� ����� ��
        {
            ErrorMessage(LoginError.None);  //None �����޼��� ǥ��
            yield break;
        }

        WWWForm loginform = new WWWForm();   //WWWForm Ŭ����: Unity���� HTTP ��û ���� �� �� ������ ���� ���
        loginform.AddField("loginID", id);   //ID �ǿ� idtmp���� �߰�
        loginform.AddField("loginPass", password);   //PASSWORD �ǿ� passwordtmp���� �߰�

        using (UnityWebRequest www = UnityWebRequest.Post(loginUri, loginform))
        {
            yield return www.SendWebRequest();  //��û ������

            //���� ���� �߻� ��
            if (www.result == UnityWebRequest.Result.ConnectionError ||
                www.result == UnityWebRequest.Result.DataProcessingError)
            {
                ErrorMessage(LoginError.ConnectionError);
                yield break;
            }

            //���� ���� ���� ��
            string response = www.downloadHandler.text.Trim();  //���� ���� �ޱ�

            if (response == "IDNotFound")    //ID�� �������� ���� ��
            {
                ErrorMessage(LoginError.IDNotFound);
            }
            else if (response == "IncorrectPassword")   //��й�ȣ�� �������� ���� ��
            {
                ErrorMessage(LoginError.IncorrectPassword);
            }
            else if (response == "Success") //�α��� ���� ��
            {
                errortmp.text = "�α��� ����";
            }
            else
            {
                ErrorMessage(LoginError.None);
            }
        }
    }
}