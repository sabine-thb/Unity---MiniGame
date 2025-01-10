using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class InterfaceHandler : MonoBehaviour
{
    public TextEffect textEffect;
    public GameObject UI;
    private float focalLengthStart = 60f;
    private float focalLengthEnd = 0f;
    public bool hideByDefault = true;
    private bool canInteract = true;
    private bool isHidden = true;

    private DepthOfField depthOfField;
    private UnityEvent onXPressed;

    public CameraManager cameraManager; // Assurez-vous de lier ceci dans l'inspecteur

    public void Start()
    {
        isHidden = hideByDefault;
        if (onXPressed == null)
            onXPressed = new UnityEvent();

        onXPressed.AddListener(OnXPressed);

        // Déclencher l'effet de texte immédiatement si l'interface n'est pas cachée par défaut
        if (!hideByDefault)
        {
            textEffect?.StartEffect();
        }

        // Initialiser l'état de visibilité du Canvas
        cameraManager.SetCanvasVisibility(!hideByDefault);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isHidden == false)
        {
            OnXPressed();
        }
    }

    private void OnSceneInitialized()
    {

        if (hideByDefault)
        {
            hideUI(false);
        }
        else
        {
            showUI(false);
        }
    }

    protected virtual void OnXPressed()
    {
        hideUI(true);
        textEffect?.SkipToLastParagraph();

        Transform interactableTransform = transform.Find("Interactable");
        if (interactableTransform != null)
        {
            interactableTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Le GameObject appelé 'interactable' n'a pas été trouvé parmi les enfants.");
        }
    }

    public void hideUI(bool smooth = true)
    {
        if (canInteract)
        {
            isHidden = true;
            StartCoroutine(FadeObject(UI, false, smooth));
            cameraManager.SetCanvasVisibility(false); // Mettre à jour l'état du Canvas
        }
    }

    public void showUI(bool smooth = true)
    {
        if (canInteract)
        {
            isHidden = false;
            StartCoroutine(FadeObject(UI, true, smooth));
            cameraManager.SetCanvasVisibility(true); // Mettre à jour l'état du Canvas
        }
    }

    protected virtual void onFadeFinished(bool opening)
    {
        if (opening)
        {
            textEffect?.StartEffect();
        }
    }

    private IEnumerator FadeObject(GameObject obj, bool isOpening, bool smooth)
    {
        if (isOpening)
        {
            obj.SetActive(true);
        }

        canInteract = false;

        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>() ?? obj.AddComponent<CanvasGroup>();

        float duration = 0.6f;
        float elapsedTime = 0f;

        float startAlpha = isOpening ? 0f : 1f;
        float endAlpha = isOpening ? 1f : 0f;

        canvasGroup.alpha = startAlpha;

        if (depthOfField != null)
        {
            depthOfField.focalLength.value = isOpening ? focalLengthEnd : focalLengthStart;
        }

        if (!smooth)
        {
            canInteract = true;
            canvasGroup.alpha = endAlpha;
            onFadeFinished(isOpening);

            if (depthOfField != null)
            {
                depthOfField.focalLength.value = isOpening ? focalLengthStart : focalLengthEnd;
            }

            if (!isOpening)
            {
                obj.SetActive(false);
            }

            yield break;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            if (depthOfField != null)
            {
                depthOfField.focalLength.value = Mathf.Lerp(
                    isOpening ? focalLengthEnd : focalLengthStart,
                    isOpening ? focalLengthStart : focalLengthEnd,
                    elapsedTime / duration);
            }
            yield return null;
        }

        canInteract = true;
        canvasGroup.alpha = endAlpha;
        onFadeFinished(isOpening);

        if (depthOfField != null)
        {
            depthOfField.focalLength.value = isOpening ? focalLengthStart : focalLengthEnd;
        }

        if (!isOpening)
        {
            obj.SetActive(false);
        }
    }
}
