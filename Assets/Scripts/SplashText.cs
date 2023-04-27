using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SplashText : MonoBehaviour
{
    private TMP_Text splashText;

    public void ShowText(string text) {
        splashText = this.GetComponent<TMP_Text>();
        splashText.text = text;
        StartCoroutine(ShowText());
    }

    public IEnumerator ShowText() {
        splashText.color = new Vector4(1,1,1,1);
        yield return new WaitForSeconds(8);
        while(splashText.color.a > 0) {
            splashText.color = new Vector4(1,1,1,splashText.color.a - 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        yield break;
    }

}
