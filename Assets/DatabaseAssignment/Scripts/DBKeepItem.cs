using UnityEngine.UI;
using UnityEngine;

using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabItem;       // 아이템 Prefab
    [SerializeField] private List<RectTransform> inventorySlots; // 슬롯 Transform 배열
    [SerializeField] private Sprite[] imageSprites;       // 추가된 이미지 배열 (직접 에디터에서 설정 가능)
    [SerializeField] private List<TextMeshProUGUI> keepitems;   //보유량 표시 텍스트



    private void Start()
    {
        // 이미지 배열을 자동으로 로드 (선택적으로 사용)
        LoadImagesFromResources();
    }

    // 버튼 클릭 시 호출
    public void OnAddItemButtonClicked()
    {
        AddItemToFirstEmptySlot();
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

            // 랜덤하게 이미지 부여
            AssignRandomImageToItem(newItem);

            Debug.Log($"아이템이 슬롯 {slotIndex}에 추가되었습니다!");
        }
        else
        {
            Debug.Log("해당 슬롯이 이미 차있습니다.");
        }
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
        int randomIndex = Random.Range(0, imageSprites.Length);
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