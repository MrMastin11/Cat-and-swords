using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Attack : MonoBehaviour
{
    [Header("Sprites & UI")]
    public Image img;
    public Graphic targetToFlash;
    public Slider EnemyHealthBar;
    public TextMeshProUGUI EnemyHealthText;
    public GameObject Enemy;
    public GameObject Chest;
    public ChestOpen chestopen;
    public SwordManager swordManager;

    [Header("Settings")]
    public float flashDuration = 0.12f;
    public Vector3 moveOffset = new Vector3(0, 20, 0);
    public int MaxEnemyHealth = 10;
    public int CurentEnemyHealth = 10;

    [HideInInspector]
    public int currentDamage = 1;

    [Header("Debug")]
    public bool enableLogs = false;

    private Coroutine flashRoutine;
    private Coroutine flashColorRoutine;
    private RectTransform rt;
    private Vector2 originalAnchoredPos;
    private Sprite originalSprite;
    private Color originalTargetColor;

    void Awake()
    {
        rt = img.rectTransform;
    }

    void Start()
    {
        originalAnchoredPos = rt.anchoredPosition;
        originalSprite = img.sprite;
        originalTargetColor = targetToFlash.color;

        EnemyHealthBar.maxValue = MaxEnemyHealth;
        EnemyHealthBar.value = CurentEnemyHealth;
        UpdateHealthText();

        Chest.SetActive(false);
    }

    void Update()
    {
        EnemyHealthBar.value = CurentEnemyHealth;
        EnemyHealthBar.maxValue = MaxEnemyHealth;
        UpdateHealthText();

        if (CurentEnemyHealth <= 0 && Enemy.activeSelf)
        {
            Enemy.SetActive(false);
            Chest.SetActive(true);
        }
    }

    public void OnClick()
    {
        if (CurentEnemyHealth <= 0) return;

        CurentEnemyHealth -= currentDamage;
        UpdateHealthText();
        ResetVisuals();

        flashRoutine = StartCoroutine(FlashAndJump());
        flashColorRoutine = StartCoroutine(WhiteFlash(targetToFlash));
    }

    void ResetVisuals()
    {
        if (flashRoutine != null) StopCoroutine(flashRoutine);
        if (flashColorRoutine != null) StopCoroutine(flashColorRoutine);

        rt.anchoredPosition = originalAnchoredPos;

        if (swordManager != null)
        {
            Sprite normalSprite = swordManager.GetCurrentNormalSprite();
            if (normalSprite != null) img.sprite = normalSprite;
        }

        targetToFlash.color = originalTargetColor;
        flashRoutine = flashColorRoutine = null;
    }

    IEnumerator FlashAndJump()
    {
        Sprite attackSprite = swordManager != null ? swordManager.GetAttackSprite() : null;
        Sprite normalSprite = swordManager != null ? swordManager.GetCurrentNormalSprite() : null;

        if (attackSprite != null) img.sprite = attackSprite;

        Vector2 start = rt.anchoredPosition;
        Vector2 target = start + new Vector2(moveOffset.x, moveOffset.y);
        float half = flashDuration * 0.5f;
        float t = 0f;

        while (t < half)
        {
            rt.anchoredPosition = Vector2.Lerp(start, target, t / half);
            t += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = target;
        t = 0f;

        while (t < half)
        {
            rt.anchoredPosition = Vector2.Lerp(target, start, t / half);
            t += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = start;

        if (normalSprite != null) img.sprite = normalSprite;
        flashRoutine = null;
    }

    IEnumerator WhiteFlash(Graphic target)
    {
        Color orig = originalTargetColor;
        target.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        target.color = orig;
        flashColorRoutine = null;
    }

    void UpdateHealthText()
    {
        EnemyHealthText.text = $"{CurentEnemyHealth} / {MaxEnemyHealth}";
    }

    public void NextEnemy()
    {
        Enemy.SetActive(true);
        Chest.SetActive(false);

        MaxEnemyHealth = Mathf.RoundToInt(MaxEnemyHealth * 1.2f);
        CurentEnemyHealth = MaxEnemyHealth;

        UpdateHealthText();
        chestopen.CloseChest();
    }
}
