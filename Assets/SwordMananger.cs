using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SwordManager : MonoBehaviour
{
    [System.Serializable]
    public class SwordData
    {
        public string name;
        public int level = 0;
        public int damage = 0;
        public TextMeshProUGUI uiText;
        public Image icon;

        [Header("Sprites")]
        public Sprite normalSprite;
        public Sprite attackSprite;
    }

    public SwordData[] swords;
    public Attack attack;

    [Header("Player Sword Settings")]
    public Image playerSwordImage;

    private SwordData currentSword;

    void Start()
    {
        foreach (var sword in swords)
        {
            bool isCopper = sword.name == "Copper Sword";
            SetVisible(sword, isCopper);

            if (isCopper)
            {
                sword.level = 1;
                sword.damage = 1;
                UpdateUI(sword);
                SelectSword(sword.name);
            }
        }
    }

    public void UpgradeSword(string swordName, int bonus)
    {
        foreach (var sword in swords)
        {
            if (sword.name == swordName)
            {
                if (sword.level == 0)
                {
                    sword.level = 1;
                    sword.damage = bonus;
                    SetVisible(sword, true);
                }
                else
                {
                    sword.level++;
                    sword.damage += bonus;
                }

                UpdateUI(sword);
                attack.NextEnemy();
                return;
            }
        }
    }

    public void SelectSword(string swordName)
    {
        foreach (var sword in swords)
        {
            if (sword.name == swordName && sword.level > 0)
            {
                currentSword = sword;
                playerSwordImage.sprite = sword.normalSprite;
                return;
            }
        }
    }

    public int GetCurrentDamage()
    {
        return currentSword != null ? currentSword.damage : 1;
    }

    public Sprite GetAttackSprite()
    {
        return currentSword != null ? currentSword.attackSprite : null;
    }

    public Sprite GetCurrentNormalSprite()
    {
        return currentSword != null ? currentSword.normalSprite : null;
    }

    private void UpdateUI(SwordData sword)
    {
        sword.uiText.text = $"{sword.name}\nLvl {sword.level}\nDamage {sword.damage}";
    }

    private void SetVisible(SwordData sword, bool visible)
    {
        sword.uiText.gameObject.SetActive(visible);
        sword.icon.gameObject.SetActive(visible);
    }
}
