using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SwordManager : MonoBehaviour
{
    [System.Serializable]
    public class SwordData
    {
        public string name;

        [Header("Stats")]
        public int level = 0;        
        public int baseDamage = 1;     
        public int damage = 0;        

        [Header("UI Elements")]
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

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private SwordData currentSword;

    void Start()
    {
        foreach (var sword in swords)
        {
            bool isCopper = sword.name == "Wooden Sword";
            SetVisible(sword, isCopper);

            if (isCopper)
            {
                sword.level = 1;
                sword.damage = sword.baseDamage;
                UpdateUI(sword);
                SelectSword(sword.name);
            }
        }

        UpdateAttackDamage();
    }

    public void UpgradeSword(string swordName)
    {
        foreach (var sword in swords)
        {
            if (sword.name == swordName)
            {
                if (sword.level == 0)
                {
                    sword.level = 1;
                    sword.damage = sword.baseDamage;
                    SetVisible(sword, true);
                }
                else
                {
                    sword.level += 1;
                    sword.damage = sword.level * sword.baseDamage;
                }

                UpdateUI(sword);

                if (currentSword == sword)
                    UpdateAttackDamage();

                if (attack != null)
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
                audioSource.PlayOneShot(clickSound);
                UpdateAttackDamage();
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
        if (sword.uiText != null)
            sword.uiText.text = $"{sword.name}\nLvl {sword.level}\nDamage {sword.damage}";
    }

    private void SetVisible(SwordData sword, bool visible)
    {
        if (sword.uiText != null) sword.uiText.gameObject.SetActive(visible);
        if (sword.icon != null) sword.icon.gameObject.SetActive(visible);
    }

    private void UpdateAttackDamage()
    {
        if (attack != null)
            attack.currentDamage = GetCurrentDamage();
    }
}


