using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CatChangerUI : MonoBehaviour
{
    public Sprite[] sprites;   
    public Image img;         
    public float interval = 0.2f;

    private int index = 0;
    void Start()
    {

    }
    void OnEnable()
    {
        StartCoroutine(ChangeSprite());
    }


    IEnumerator ChangeSprite()
    {
        while (true)
        {
            img.sprite = sprites[index];
            index = (index + 1) % sprites.Length;
            yield return new WaitForSeconds(interval);
        }
    }
}