using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DropItem : MonoBehaviour, IPointerClickHandler
{
    private int SwordLvl = 1;
    private int Damage = 1;
    private string swordType;

    private Image swordImage;
    private TextMeshProUGUI swordNameText;

    void Awake()
    {
        swordImage = GetComponentInChildren<Image>();
        swordNameText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSwordData(string name, Sprite sprite)
    {
        swordType = name;
        if (swordImage != null) swordImage.sprite = sprite;
        if (swordNameText != null) swordNameText.text = name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Destroy(gameObject);
        SwordLvl += 1;
        Damage += 1;

        if (swordNameText != null)
            swordNameText.text = $"{swordType}\nLvl {SwordLvl}\nDamage {Damage}";
    }
}
