using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyChangerUI : MonoBehaviour
{
    public Sprite[] sprites;  
    public Image img;        
    public float interval = 0.2f;

    private int[] currentSet;  
    private int index = 0;

    void OnEnable()
    {
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
                break;
            case 4:
            case 5:
            case 6:
                currentSet = new int[] { 2, 3 };
                break;
            case 7:
            case 8:
                currentSet = new int[] { 4, 5 };
                break;
            case 9:
                currentSet = new int[] { 6, 7 };
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
            yield return new WaitForSeconds(interval);
        }
    }
}
