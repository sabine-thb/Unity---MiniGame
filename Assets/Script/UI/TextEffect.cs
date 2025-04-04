using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextEffect : MonoBehaviour
{
    [SerializeField] TMP_Text textComponent;
    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnParagraph;
    public string[] stringArray;

    public Button buttonToShow; 
    [SerializeField] float fadeDuration = 1.0f;

    int i = 0;
    private bool fadeInStarted = false; 

    public void StartEffect()
    {
        i = 0;
        fadeInStarted = false; 
        textComponent.text = stringArray.Length > 0 ? stringArray[i] : "";
        textComponent.maxVisibleCharacters = 0;
        buttonToShow.gameObject.SetActive(false);
        StopAllCoroutines(); 
        CheckEndParagraph();
    }

    void CheckEndParagraph()
    {
        if (i < stringArray.Length)
        {
            textComponent.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
    }

    private IEnumerator TextVisible()
    {
        textComponent.ForceMeshUpdate();
        int totalVisibleCharacters = textComponent.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            textComponent.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                if (i < stringArray.Length - 1)
                {
                    yield return new WaitForSeconds(timeBtwnParagraph);
                    i += 1;
                    CheckEndParagraph();
                }
                else
                {
                    break; 
                }
            }

            counter += 1;
            yield return new WaitForSeconds(timeBtwnChars);
        }


        if (!fadeInStarted)
        {
            fadeInStarted = true; 
            StartCoroutine(FadeInButton(buttonToShow, fadeDuration));
        }
    }

    void EndCheck()
    {
        CheckEndParagraph();
    }

    public void SkipToLastParagraph()
    {
        StopAllCoroutines();

        i = stringArray.Length - 1;
        fadeInStarted = false; 
        textComponent.text = stringArray[i];

        textComponent.ForceMeshUpdate();
        textComponent.maxVisibleCharacters = textComponent.textInfo.characterCount;
        buttonToShow.gameObject.SetActive(true);

        Debug.Log("SkipToLastParagraph");
    }

    public void ResetEffect()
    {
        StopAllCoroutines();
        StartEffect();
    }

    private IEnumerator FadeInButton(Button button, float duration)
    {
        if (button == null) yield break;

        Image buttonImage = button.GetComponent<Image>();
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

        if (buttonImage == null || buttonText == null)
        {
            Debug.LogWarning("FadeInButton: Button components are missing.");
            yield break;
        }


        Color imageColor = buttonImage.color;
        Color textColor = buttonText.color;
        imageColor.a = 0f;
        textColor.a = 0f;

        buttonImage.color = imageColor;
        buttonText.color = textColor;


        button.gameObject.SetActive(true);
        button.interactable = false; 

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);


            imageColor.a = alpha;
            textColor.a = alpha;
            buttonImage.color = imageColor;
            buttonText.color = textColor;

            yield return null;
        }


        imageColor.a = 1f;
        textColor.a = 1f;
        buttonImage.color = imageColor;
        buttonText.color = textColor;

        button.interactable = true; 
    }
}
