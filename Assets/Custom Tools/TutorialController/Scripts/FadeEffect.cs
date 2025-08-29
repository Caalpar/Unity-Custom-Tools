using System;
using System.Collections;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup targetCanvasGroup;
    public float fadeInOutTime = 3;
    public float duration = 1.0f;
    Coroutine feadeInOutRutine = null;


    private void Start()
    {
      
    }

    public void FadeInOut(Action action)
    {
        if (feadeInOutRutine != null)
            feadeInOutRutine = StartCoroutine(FadeInOutRutine(action));
    }

    public IEnumerator FadeInOutRutine(Action action)
    {
        StartCoroutine(FadeRutine(true));
        yield return new WaitForSeconds(fadeInOutTime/2);
        if(action != null)
            action.Invoke();
        yield return new WaitForSeconds(fadeInOutTime/2);
        StartCoroutine(FadeRutine(false));
        feadeInOutRutine = null;
    }

    public IEnumerator FadeRutine(bool fadeIn)
    {
        float startAlpha = targetCanvasGroup.alpha;
        float endAlpha = fadeIn ? 1 : 0;
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
            targetCanvasGroup.alpha = newAlpha;
            yield return null;
        }
        targetCanvasGroup.alpha = endAlpha;
    }



}