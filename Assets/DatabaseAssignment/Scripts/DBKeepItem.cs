using UnityEngine.UI;
using UnityEngine;

using System.Collections.Generic;
using TMPro;
using System;
using System.Collections;
using UnityEngine.Networking;

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
        StartCoroutine(StartGenerateItem(itemHash));
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
                Debug.Log(www.downloadHandler.text);
                string response = www.downloadHandler.text;

                if (response == "NewInvenSuccess")
                {
                    itemHash = string.Empty;
                }
                else
                {
                    itemHash += response;
                }
            }
        }
    }
    private IEnumerator StartGenerateItem(string _itemHash)
    {
        yield return null;
    }

    // ��ư Ŭ�� �� ȣ��
    public void OnAddItemButtonClicked()
    {
        AddItemToFirstEmptySlot();
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

            // �����ϰ� �̹��� �ο�
            AssignRandomImageToItem(newItem);
            newItem.GetComponentInChildren<ItemImage>().ItemPOS = "Slot" + slotIndex.ToString();
            SetItemHash(newItem.GetComponentInChildren<ItemImage>());

            Debug.Log($"�������� ���� {slotIndex}�� �߰��Ǿ����ϴ�!");
        }
        else
        {
            Debug.Log("�ش� ������ �̹� ���ֽ��ϴ�.");
        }
    }

    private void SetItemHash(ItemImage _item)
    {

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
        item.GetComponentInChildren<ItemImage>().ItemID = "Item" + randomIndex.ToString();
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