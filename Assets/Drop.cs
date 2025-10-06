using TMPro;
using UnityEngine; 
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItem : MonoBehaviour, IPointerClickHandler
{
    public SwordManager swordManager;
    private string swordName;
    public TextMeshProUGUI textMeshPro;
    public Image image;

    public void SetSwordData(string name, Sprite sprite) 
    {
        swordName = name;
        textMeshPro.text = name;
        image.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (swordManager != null)
            swordManager.UpgradeSword(swordName);
        Destroy(gameObject);
    }
}


