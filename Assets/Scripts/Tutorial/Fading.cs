using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Fading : MonoBehaviour
{
    [SerializeField] private float fadeSpeedIn = 1f;
    [SerializeField] private float fadeSpeedOut = 1f;

    private Image image;
    
    // Start
    private void Start()
    {
        image = GetComponent<Image>();
    }
    
    // Fade In
    [ContextMenu("Fade In")]
    public void FadeIn()
    {
        this.GetComponent<Image>().enabled = true;
        StartCoroutine(FadeTo(1.0f, fadeSpeedIn, true));
    }
    
    // Fade Out
    [ContextMenu("Fade Out")]
    public void FadeOut()
    {
        StartCoroutine(FadeTo(0.0f, fadeSpeedOut, false));
    }
    
    private IEnumerator FadeTo(float targetAlpha, float speed, bool isFadingIn)
    {
        float elapsedTime = 0;
        Color startColor = image.color;

        while (elapsedTime < speed)
        {
            Color newColor = image.color;
            newColor.a = Mathf.Lerp(startColor.a, targetAlpha, (elapsedTime / speed));
            image.color = newColor;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        this.GetComponent<Image>().enabled =  isFadingIn;
    }
}
