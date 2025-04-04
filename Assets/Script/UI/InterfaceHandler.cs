using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Events;

public class InterfaceHandler : MonoBehaviour
{
    public TextEffect textEffect;
    public GameObject UI;
    public bool hideByDefault = true;
    private bool canInteract = true;
    private bool isHidden = true;

    [SerializeField] private UnityEvent onXPressed;

    public CameraManager cameraManager; 

    public void Start()
    {
        isHidden = hideByDefault;

        if (onXPressed == null)
            onXPressed = new UnityEvent();

        onXPressed.AddListener(OnXPressed);

        // Trigger text effect immediately if interface is not hidden by default
        if (!hideByDefault)
        {
            textEffect?.StartEffect();
        }

        // Initialize Canvas visibility state
        cameraManager.SetCanvasVisibility(!hideByDefault);

        if (hideByDefault)
        {
            HideUI(false);
        }
        else
        {
            ShowUI(false);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isHidden == false)
        {
            OnXPressed();
        }
    }

    protected virtual void OnXPressed()
    {
        HideUI(true);
        textEffect?.SkipToLastParagraph();
        onXPressed.Invoke();

        Transform interactableTransform = transform.Find("Interactable");
        if (interactableTransform != null)
        {
            interactableTransform.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Le GameObject 'Interactable' n'a pas été trouvé.");
        }
    }

    public void HideUI(bool smooth = true)
    {
        isHidden = true;
        StartCoroutine(FadeObject(UI, false, smooth));
        cameraManager.SetCanvasVisibility(false); 

        Debug.Log("HideUI");
    }

    public void ShowUI(bool smooth = true)
    {
        if (canInteract)
        {
            isHidden = false;
            StartCoroutine(FadeObject(UI, true, smooth));
            cameraManager.SetCanvasVisibility(true); 
            Debug.Log("ShowUI");
        }
    }

    protected virtual void OnFadeFinished(bool opening)
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

        if (!smooth)
        {
            canInteract = true;
            canvasGroup.alpha = endAlpha;
            OnFadeFinished(isOpening);

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
            yield return null;
        }

        canInteract = true;
        canvasGroup.alpha = endAlpha;
        OnFadeFinished(isOpening);

        if (!isOpening)
        {
            obj.SetActive(false);
        }
    }
}
