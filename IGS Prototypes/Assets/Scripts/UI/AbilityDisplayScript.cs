using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityDisplayScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI abilityText;

    private Color orginalColor;
    private Button button;

    private bool mouseHovering = false;

    private void Start()
    {
        orginalColor = GetComponent<Image>().color;
        button = GetComponent<Button>();
    }

    public void SetText(string text)
    {
        abilityText.text = text;
    }

    public void Unblock()
    {
        button.GetComponent<Image>().color = orginalColor;
        button.GetComponent<Button>().enabled = true;
    }

    public void Block()
    {
        button.GetComponent<Image>().color = Color.gray;
        button.GetComponent<Button>().enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseHovering = false;
    }

    public bool IsMouseHovering()
    {
        return mouseHovering;
    }
}
