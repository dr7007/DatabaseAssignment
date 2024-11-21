using UnityEngine.UI;
using UnityEngine;

using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabItem;       // ������ Prefab
    [SerializeField] private List<RectTransform> inventorySlots; // ���� Transform �迭
    [SerializeField] private Sprite[] imageSprites;       // �߰��� �̹��� �迭 (���� �����Ϳ��� ���� ����)
    [SerializeField] private List<TextMeshProUGUI> keepitems;   //������ ǥ�� �ؽ�Ʈ



    private void Start()
    {
        // �̹��� �迭�� �ڵ����� �ε� (���������� ���)
        LoadImagesFromResources();
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

            Debug.Log($"�������� ���� {slotIndex}�� �߰��Ǿ����ϴ�!");
        }
        else
        {
            Debug.Log("�ش� ������ �̹� ���ֽ��ϴ�.");
        }
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
        int randomIndex = Random.Range(0, imageSprites.Length);
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