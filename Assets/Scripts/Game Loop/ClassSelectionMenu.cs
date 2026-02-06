using UnityEngine;

public class ClassSelectionMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        ToggleVisibility(false);
    }
    
    public void ToggleVisibility(bool toggle)
    {
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;
        canvasGroup.alpha = toggle ? 1 : 0;
    }
}
