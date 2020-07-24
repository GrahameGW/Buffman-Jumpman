using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MenuButtonBehaviorUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Color hoverColor = default;

    private TextMeshProUGUI text;
    private Color defaultColor;
    
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        defaultColor = text.color; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defaultColor;
    }

    public void ResetColor()
    {
        text.color = defaultColor;
    }
}
