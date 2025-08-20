using System.Collections;
using TMPro;
using UnityEngine;

public class Text : MonoBehaviour
{
    TextMeshProUGUI _renderer;
    int r = 255;
    int g;
    int b;
    bool aaa;
    

    private void Awake()
    {
        _renderer = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ColorChange());
    }
    private void Update()
    {
        _renderer.color = new Color(r / 255f, g / 255f, b / 255f, 1);        
        //g ¥√∞Ì r ¡Ÿ∞Ì b ¥√∞Ì g ¡Ÿ∞Ì r ¥√∞Ì b ¡Ÿ∞Ì
    }

    IEnumerator ColorChange()
    {
        while (g <=255)
        {
            g+=2;
            yield return new WaitForSeconds(0.01f);
        }
        while (r >= 0)
        {
            r-=2;
            yield return new WaitForSeconds(0.01f);
        }
        while (b <= 255)
        {
            b+=2;
            yield return new WaitForSeconds(0.01f);
        }
        while (g >= 0)
        {
            g-=2;
            yield return new WaitForSeconds(0.01f);
        }
        while (r <=255)
        {
            r += 2;
            yield return new WaitForSeconds(0.01f);
        }
        while (b >= 0)
        {
            b -= 2;
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(ColorChange());
        yield break;
        
    }
}
