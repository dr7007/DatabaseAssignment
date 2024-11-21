using UnityEngine.UI;
using UnityEngine;

using System.Collections.Generic;
using TMPro;
using System;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Windows;
using System.Text.RegularExpressions;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabItem;       // ������ Prefab
    [SerializeField] private List<RectTransform> inventorySlots; // ���� Transform �迭
    [SerializeField] private Sprite[] imageSprites;       // �߰��� �̹��� �迭 (���� �����Ϳ��� ���� ����)
    [SerializeField] private List<TextMeshProUGUI> keepitems;   //������ ǥ�� �ؽ�Ʈ

    [SerializeField] private string itemHash = string.Empty;


    private void Start()
    {
        StartCoroutine(SetInvenCoroutine(FindAnyObjectByType<DBPlayerInfo>().uid));
        // �̹��� �迭�� �ڵ����� �ε� (���������� ���)
        LoadImagesFromResources();
    }

    private IEnumerator SetInvenCoroutine(string _id)
    {
        string uri = "http://127.0.0.1/DAGetInven.php";

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
                string response = www.downloadHandler.text;
                Debug.Log(response);

                if (response == "NewInvenIDCreat")
                {
                    itemHash = string.Empty;
                }
                else
                {
                    itemHash += response;
                    StartCoroutine(StartGenerateItem(itemHash));
                }
            }
        }
    }

    private IEnumerator UpdateInvenCoroutine(string _id, string _itemHash)
    {
        string uri = "http://127.0.0.1/DAUpdateInven.php";

        WWWForm form = new WWWForm();
        form.AddField("id", _id);
        form.AddField("iteminfo", _itemHash);


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
            }
        }
    }
    private IEnumerator StartGenerateItem(string _itemHash)
    {
        // "_"�� �и��� �׸���� ó��
        string[] pairs = _itemHash.Split('_');
        foreach (string pair in pairs)
        {
            // "-"�� �и��Ͽ� Item�� Slot���� ������
            string[] parts = pair.Split('-');
            if (parts.Length == 2)
            {
                string item = parts[0];
                string slot = parts[1];

                int itemNumber = ExtractNumber(item);
                int slotNumber = ExtractNumber(slot);

                AddItemToSlotAndType(itemNumber,slotNumber);
                yield return null;
            } 
        }
    }

    private int ExtractNumber(string input)
    {
        string number = Regex.Replace(input, @"\D", ""); // ���ڸ� ����
        return int.TryParse(number, out int result) ? result : 0; // ���ڷ� ��ȯ
    }

    // ��ư Ŭ�� �� ȣ��
    public void OnAddItemButtonClicked()
    {
        AddItemToFirstEmptySlot();
    }

    // ���� �������� Ư�� ���Կ� ��ġ
    public void AddItemToSlotAndType(int itemIndex, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("�߸��� ���� �ε����Դϴ�.");
            return;
        }

        RectTransform targetSlot = inventorySlots[slotIndex];
        Transform targetSlotTransform = targetSlot.transform;

        if (targetSlotTransform.childCount == 0) // ������ ����ִ� ��츸 ������ �߰�
        {
            GameObject newItem = Instantiate(prefabItem, targetSlotTransform);
            newItem.transform.localPosition = Vector3.zero; // ���� �߽����� ��ġ �ʱ�ȭ
            newItem.transform.localScale = Vector3.one;     // ������ �ʱ�ȭ

            Sprite selectImage = imageSprites[itemIndex];

            // ������ �������� Source Image ������Ʈ�� �Ҵ�
            Image itemImage = newItem.GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = selectImage;
                Debug.Log("���� �̹����� �����ۿ� ����Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogWarning("�����ۿ� Image ������Ʈ�� �����ϴ�.");
            }

            Debug.Log($"������{itemIndex}�� ���� {slotIndex}�� �߰��Ǿ����ϴ�!");
        }
        else
        {
            Debug.Log("�ش� ������ �̹� ���ֽ��ϴ�.");
        }
    }

    // �������� Ư�� ���Կ� ��ġ
    public void AddItemToSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("�߸��� ���� �ε����Դϴ�.");
            return;
        }

        RectTransform targetSlot = inventorySlots[slotIndex];
        Transform targetSlotTransform = targetSlot.transform;

        if (targetSlotTransform.childCount == 0) // ������ ����ִ� ��츸 ������ �߰�
        {
            GameObject newItem = Instantiate(prefabItem, targetSlotTransform);
            newItem.transform.localPosition = Vector3.zero; // ���� �߽����� ��ġ �ʱ�ȭ
            newItem.transform.localScale = Vector3.one;     // ������ �ʱ�ȭ

            ItemImage image = newItem.GetComponentInChildren<ItemImage>();
            // �����ϰ� �̹��� �ο�
            AssignRandomImageToItem(newItem);
            image.ItemPOS = "Slot" + slotIndex.ToString() + "_";
            SetItemHash(image);
            Debug.Log(itemHash);

            Debug.Log($"�������� ���� {slotIndex}�� �߰��Ǿ����ϴ�!");
        }
        else
        {
            Debug.Log("�ش� ������ �̹� ���ֽ��ϴ�.");
        }
    }

    private void SetItemHash(ItemImage _itemimage)
    {
        itemHash += (_itemimage.ItemID + _itemimage.ItemPOS);
        StartCoroutine(UpdateInvenCoroutine(FindAnyObjectByType<DBPlayerInfo>().uid, itemHash));
    }

    // ù ��° �� ���Կ� ������ �߰�
    public void AddItemToFirstEmptySlot()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Transform slotTransform = inventorySlots[i].transform;

            if (slotTransform.childCount == 0) // �� ���� ã��
            {
                AddItemToSlot(i);
                return;
            }
        }

        Debug.Log("�� ������ �����ϴ�.");
    }

    // �̹����� Resources/texture �������� �ε��Ͽ� imageSprites �迭�� �ڵ����� �Ҵ�
    private void LoadImagesFromResources()
    {
        imageSprites = Resources.LoadAll<Sprite>("textures"); // Resources/texture �������� ��� ��������Ʈ �ε�
    }

    // �����ۿ� �������� �̹����� �ο�
    private void AssignRandomImageToItem(GameObject item)
    {
        if (imageSprites.Length == 0)
        {
            Debug.LogWarning("texture ������ �̹����� �����ϴ�!");
            return;
        }

        // �������� �̹��� ����
        int randomIndex = UnityEngine.Random.Range(0, imageSprites.Length);
        item.GetComponentInChildren<ItemImage>().ItemID = "Item" + randomIndex.ToString() + "-";
        Sprite randomImage = imageSprites[randomIndex];

        // ������ �������� Source Image ������Ʈ�� �Ҵ�
        Image itemImage = item.GetComponentInChildren<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = randomImage;
            Debug.Log("���� �̹����� �����ۿ� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogWarning("�����ۿ� Image ������Ʈ�� �����ϴ�.");
        }
    }
}