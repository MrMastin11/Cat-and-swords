using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ChestOpen : MonoBehaviour
{
    [Header("Chest Settings")]
    public Sprite[] chestSprites;
    public Image Chest;
    public TextMeshProUGUI textMeshProUGUI;

    [Header("Drop Settings")]
    public GameObject dropPrefab;
    public Transform spawnPoint;
    public float height = 100f;
    public float duration = 1f;

    [Header("Sword Sprites")]
    public Sprite woodenSprite;
    public Sprite stoneSprite;
    public Sprite copperSprite;
    public Sprite ironSprite;
    public Sprite silverSprite;
    public Sprite goldSprite;
    public Sprite diamondSprite;
    public Sprite uraniumSprite;
    public Sprite obsidianSprite;
    public Sprite darkSprite;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    [Header("Scripts")]
    public SwordManager swordmanager;
    public Attack attack;
    
    public GameObject canvas;
    public GameObject background;
    public TextMeshProUGUI EndText;

    private bool opened = false;

    private List<string> swordPool = new List<string>();
    private System.Random rng = new System.Random();
    private int currentTier = 1;

    private int wooden = 1;
    private int stone = 0;
    private int copper = 0;
    private int iron = 0;
    private int silver = 0;
    private int gold = 0;
    private int diamond = 0;
    private int obsidian = 0;
    private int uranium = 0;
    private int dark = 0;


    void Start()
    {
        Chest.sprite = chestSprites[0];
        textMeshProUGUI.text = "Click to unlock!";
        background.SetActive(true);
        EndText.text = "";
        RefillSwordPool();
    }

    public void OnChestClick()
    {
        if (opened) return; 
        opened = true;
        StartCoroutine(OpenChestRoutine());
        audioSource.PlayOneShot(clickSound);
    }

    IEnumerator OpenChestRoutine()
    {
        Chest.sprite = chestSprites[1];
        textMeshProUGUI.text = "";

        (string name, Sprite sprite) = GetNextSword();

        GameObject drop = Instantiate(dropPrefab, spawnPoint.position, Quaternion.identity, Chest.transform.parent);
        DropItem dropItem = drop.GetComponent<DropItem>();
        dropItem.SetSwordData(name, sprite);

        switch (name)
        {
            case "Wooden Sword":
                if (currentTier < 2) currentTier = 2; RefillSwordPool();
                break;
            case "Stone Sword":
                if (currentTier < 3) currentTier = 3; RefillSwordPool();
                break;
            case "Copper Sword":
                if (currentTier < 4) currentTier = 4; RefillSwordPool();
                break;
            case "Iron Sword":
                if (currentTier < 5) currentTier = 5; RefillSwordPool();
                break;
            case "Silver Sword":
                if (currentTier < 6) currentTier = 6; RefillSwordPool();
                break;
            case "Gold Sword":
                if (currentTier < 7) currentTier = 7; RefillSwordPool();
                break;
            case "Diamond Sword":
                if (currentTier < 8) currentTier = 8; RefillSwordPool();
                break;
            case "Obsidian Sword":
                if (currentTier < 9) currentTier = 9; RefillSwordPool();
                break;
            case "Uranium Sword":
                if (currentTier < 10) currentTier = 10; RefillSwordPool();
                break;
        }
        yield return StartCoroutine(DropMotion(drop.transform));

    }

    public void CloseChest()
    {
        opened = false; 
        Chest.sprite = chestSprites[0];
        textMeshProUGUI.text = "Click to unlock!";
    }

    (string, Sprite) GetNextSword()
    {
        if (swordPool.Count == 0)
            RefillSwordPool();

        string name = swordPool[0];
        swordPool.RemoveAt(0);

        switch (name)
        {
            case "Wooden Sword": wooden++; if (wooden == 5) attack.DamageMultiplier++; return (name, woodenSprite);
            case "Stone Sword": stone++; if (stone == 5) attack.DamageMultiplier++; return (name, stoneSprite);
            case "Copper Sword": copper++; if (copper == 5) attack.DamageMultiplier++; return (name, copperSprite);
            case "Iron Sword": iron++; if (iron == 5) attack.DamageMultiplier++; return (name, ironSprite);
            case "Silver Sword": silver++; if (silver == 5) attack.DamageMultiplier++; return (name, silverSprite);
            case "Gold Sword": gold++; if (gold == 5) attack.DamageMultiplier++; return (name, goldSprite);
            case "Diamond Sword": diamond++; if (diamond == 5) attack.DamageMultiplier++; return (name, diamondSprite);
            case "Obsidian Sword": obsidian++; if (obsidian == 5) attack.DamageMultiplier++; return (name, obsidianSprite);
            case "Uranium Sword": uranium++; if (uranium == 5) attack.DamageMultiplier++; return (name, uraniumSprite);
            case "Dark Sword": dark++; if (dark == 5) attack.DamageMultiplier++; return (name, darkSprite);
            default: return ("???", copperSprite);
        }
    }

    void RefillSwordPool()
    {
        swordPool.Clear();
        if (currentTier >= 1 && wooden <5) AddSwords("Wooden Sword", 10);
        if (currentTier >= 2 && stone <5) AddSwords("Stone Sword", 9);
        if (currentTier >= 3 && copper <5) AddSwords("Copper Sword", 8);
        if (currentTier >= 4 && iron <5) AddSwords("Iron Sword", 7);
        if (currentTier >= 5 && silver <5) AddSwords("Silver Sword", 6);
        if (currentTier >= 6 && gold <5) AddSwords("Gold Sword", 5);
        if (currentTier >= 7 && diamond <5) AddSwords("Diamond Sword", 4);
        if (currentTier >= 8 && obsidian <5) AddSwords("Obsidian Sword", 3);
        if (currentTier >= 9 && uranium <5) AddSwords("Uranium Sword", 2);
        if (currentTier >= 10 && dark < 5) AddSwords("Dark Sword", 1);
        if (swordPool.Count == 0) { canvas.SetActive(false); background.SetActive(false); EndText.text = "Thanks for playing!"; }



        for (int i = swordPool.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (swordPool[i], swordPool[j]) = (swordPool[j], swordPool[i]);
            }
    }

    void AddSwords(string name, int count)
    {
        for (int i = 0; i < count; i++)
            swordPool.Add(name);
    }

    IEnumerator DropMotion(Transform obj)
    {
        Vector3 startPos = obj.position;
        float upTime = duration * 0.33f;
        float downTime = duration * 0.67f;
        float t = 0f;

        while (t < upTime)
        {
            t += Time.deltaTime;
            float normalized = t / upTime;
            obj.position = startPos + Vector3.up * (height * normalized);
            yield return null;
        }

        Vector3 peakPos = obj.position;
        t = 0f;
        while (t < downTime)
        {
            t += Time.deltaTime;
            float normalized = t / downTime;
            float yOffset = -Mathf.Pow(normalized, 2) * height * 2;
            obj.position = peakPos + new Vector3(0, yOffset, 0);
            yield return null;
        }
    }
}

