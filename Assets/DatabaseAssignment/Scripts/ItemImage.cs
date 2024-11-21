using UnityEngine;

public class ItemImage : MonoBehaviour
{
    [SerializeField]
    private string itemID = string.Empty;
    [SerializeField]
    private string itemPos = string.Empty;

    public string ItemID
    { 
        get { return itemID; } 
        set { itemID = value; }
    }
    public string ItemPOS
    {
        get { return itemPos; }
        set { itemPos = value; }
    }
}
