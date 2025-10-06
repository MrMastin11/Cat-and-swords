using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EnemyChangerUI : MonoBehaviour
{
    [Header("Enemy Animation")]
    public Sprite[] sprites;
    public Image img;
    public float interval = 0.2f;

    private int[] currentSet;
    private int index = 0;

    [Header("Player Settings")]
    public int playerMaxHP = 100;
    private int playerHP;
    public Slider playerHealthBar;
    public TextMeshProUGUI playerHPText;
    public GameObject playerObject;

    [Header("UI / Restart")]
    public GameObject restartButton;

    [Header("Enemy Damage")]
    private int enemyDamage;

    void OnEnable()
    {
        restartButton.SetActive(false);
        playerHP = playerMaxHP;
        UpdateHPUI();

        ChooseRandomSet();
        StartCoroutine(ChangeSprite());
    }

    void ChooseRandomSet()
    {
        int random = Random.Range(0, 10);
        switch (random)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                currentSet = new int[] { 0, 1 };
                enemyDamage = 1;
                break;
            case 4:
            case 5:
            case 6:
                currentSet = new int[] { 2, 3 };
                enemyDamage = 2;
                break;
            case 7:
            case 8:
                currentSet = new int[] { 4, 5 };
                enemyDamage = 3;
                break;
            case 9:
                currentSet = new int[] { 6, 7 };
                enemyDamage = 4;
                break;
        }
        index = 0;
    }

    IEnumerator ChangeSprite()
    {
        while (true)
        {
            img.sprite = sprites[currentSet[index]];
            index = (index + 1) % currentSet.Length;

            TakeDamage(enemyDamage);

            yield return new WaitForSeconds(interval);
        }
    }

    void TakeDamage(int dmg)
    {
        if (playerHP <= 0) return;

        playerHP -= dmg;
        UpdateHPUI();

        if (playerHP <= 0)
        {
            playerHP = 0;
            UpdateHPUI();
            PlayerDeath();
            StopCoroutine(ChangeSprite());
        }
    }

    void UpdateHPUI()
    {
        playerHealthBar.value = (float)playerHP / playerMaxHP;
        playerHPText.text = $"{playerHP} / 100";
    }

    void PlayerDeath()
    {
        if (playerObject != null)
            playerObject.SetActive(false);

        if (restartButton != null)
            restartButton.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

