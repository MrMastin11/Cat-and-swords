using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Sprite cuperSprite;
    public Sprite ironSprite;
    public Sprite goldSprite;
    public Sprite diamondSprite;
    public Sprite obsidianSprite;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip clickSound;

    private bool opened = false;

    private List<string> swordPool = new List<string>();
    private System.Random rng = new System.Random();

    void Start()
    {
        Chest.sprite = chestSprites[0];
        textMeshProUGUI.text = "Click to unlock!";
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
            case "Copper Sword": return (name, cuperSprite);
            case "Iron Sword": return (name, ironSprite);
            case "Gold Sword": return (name, goldSprite);
            case "Diamond Sword": return (name, diamondSprite);
            case "Obsidian Sword": return (name, obsidianSprite);
            default: return ("???", cuperSprite);
        }
    }

    void RefillSwordPool()
    {
        swordPool.Clear();

        AddSwords("Copper Sword", 15);
        AddSwords("Iron Sword", 12);
        AddSwords("Gold Sword", 9);
        AddSwords("Diamond Sword", 6);
        AddSwords("Obsidian Sword", 3);

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

