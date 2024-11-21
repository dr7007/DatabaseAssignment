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
    [SerializeField] private GameObject prefabItem;       // 아이템 Prefab
    [SerializeField] private List<RectTransform> inventorySlots; // 슬롯 Transform 배열
    [SerializeField] private Sprite[] imageSprites;       // 추가된 이미지 배열 (직접 에디터에서 설정 가능)
    [SerializeField] private List<TextMeshProUGUI> keepitems;   //보유량 표시 텍스트

    [SerializeField] private string itemHash = string.Empty;


    private void Start()
    {
        StartCoroutine(SetInvenCoroutine(FindAnyObjectByType<DBPlayerInfo>().uid));
        // 이미지 배열을 자동으로 로드 (선택적으로 사용)
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
        // "_"로 분리된 항목들을 처리
        string[] pairs = _itemHash.Split('_');
        foreach (string pair in pairs)
        {
            // "-"로 분리하여 Item과 Slot으로 나누기
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
        string number = Regex.Replace(input, @"\D", ""); // 문자를 제거
        return int.TryParse(number, out int result) ? result : 0; // 숫자로 변환
    }

    // 버튼 클릭 시 호출
    public void OnAddItemButtonClicked()
    {
        AddItemToFirstEmptySlot();
    }

    // 시작 아이템을 특정 슬롯에 배치
    public void AddItemToSlotAndType(int itemIndex, int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("잘못된 슬롯 인덱스입니다.");
            return;
        }

        RectTransform targetSlot = inventorySlots[slotIndex];
        Transform targetSlotTransform = targetSlot.transform;

        if (targetSlotTransform.childCount == 0) // 슬롯이 비어있는 경우만 아이템 추가
        {
            GameObject newItem = Instantiate(prefabItem, targetSlotTransform);
            newItem.transform.localPosition = Vector3.zero; // 슬롯 중심으로 위치 초기화
            newItem.transform.localScale = Vector3.one;     // 스케일 초기화

            Sprite selectImage = imageSprites[itemIndex];

            // 생성된 아이템의 Source Image 컴포넌트에 할당
            Image itemImage = newItem.GetComponentInChildren<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = selectImage;
                Debug.Log("선택 이미지가 아이템에 적용되었습니다.");
            }
            else
            {
                Debug.LogWarning("아이템에 Image 컴포넌트가 없습니다.");
            }

            Debug.Log($"아이템{itemIndex}이 슬롯 {slotIndex}에 추가되었습니다!");
        }
        else
        {
            Debug.Log("해당 슬롯이 이미 차있습니다.");
        }
    }

    // 아이템을 특정 슬롯에 배치
    public void AddItemToSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventorySlots.Count)
        {
            Debug.LogWarning("잘못된 슬롯 인덱스입니다.");
            return;
        }

        RectTransform targetSlot = inventorySlots[slotIndex];
        Transform targetSlotTransform = targetSlot.transform;

        if (targetSlotTransform.childCount == 0) // 슬롯이 비어있는 경우만 아이템 추가
        {
            GameObject newItem = Instantiate(prefabItem, targetSlotTransform);
            newItem.transform.localPosition = Vector3.zero; // 슬롯 중심으로 위치 초기화
            newItem.transform.localScale = Vector3.one;     // 스케일 초기화

            ItemImage image = newItem.GetComponentInChildren<ItemImage>();
            // 랜덤하게 이미지 부여
            AssignRandomImageToItem(newItem);
            image.ItemPOS = "Slot" + slotIndex.ToString() + "_";
            SetItemHash(image);
            Debug.Log(itemHash);

            Debug.Log($"아이템이 슬롯 {slotIndex}에 추가되었습니다!");
        }
        else
        {
            Debug.Log("해당 슬롯이 이미 차있습니다.");
        }
    }

    private void SetItemHash(ItemImage _itemimage)
    {
        itemHash += (_itemimage.ItemID + _itemimage.ItemPOS);
        StartCoroutine(UpdateInvenCoroutine(FindAnyObjectByType<DBPlayerInfo>().uid, itemHash));
    }

    // 첫 번째 빈 슬롯에 아이템 추가
    public void AddItemToFirstEmptySlot()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            Transform slotTransform = inventorySlots[i].transform;

            if (slotTransform.childCount == 0) // 빈 슬롯 찾기
            {
                AddItemToSlot(i);
                return;
            }
        }

        Debug.Log("빈 슬롯이 없습니다.");
    }

    // 이미지를 Resources/texture 폴더에서 로드하여 imageSprites 배열에 자동으로 할당
    private void LoadImagesFromResources()
    {
        imageSprites = Resources.LoadAll<Sprite>("textures"); // Resources/texture 폴더에서 모든 스프라이트 로드
    }

    // 아이템에 랜덤으로 이미지를 부여
    private void AssignRandomImageToItem(GameObject item)
    {
        if (imageSprites.Length == 0)
        {
            Debug.LogWarning("texture 폴더에 이미지가 없습니다!");
            return;
        }

        // 랜덤으로 이미지 선택
        int randomIndex = UnityEngine.Random.Range(0, imageSprites.Length);
        item.GetComponentInChildren<ItemImage>().ItemID = "Item" + randomIndex.ToString() + "-";
        Sprite randomImage = imageSprites[randomIndex];

        // 생성된 아이템의 Source Image 컴포넌트에 할당
        Image itemImage = item.GetComponentInChildren<Image>();
        if (itemImage != null)
        {
            itemImage.sprite = randomImage;
            Debug.Log("랜덤 이미지가 아이템에 적용되었습니다.");
        }
        else
        {
            Debug.LogWarning("아이템에 Image 컴포넌트가 없습니다.");
        }
    }
}