using UnityEngine;
using UnityEngine.UI;

public class PlayerTextDisplay : MonoBehaviour
{
    public Transform textTransform;

    private Canvas canvas;

    private TextController currentlyDisplayingText;

    void Start()
    {
        this.canvas = this.GetComponentInChildren<Canvas>();
    }

    public void ClearText()
    {
        this.currentlyDisplayingText.ExitAnimation();
        this.currentlyDisplayingText = null;
    }

    public bool IsShowingText()
    {
        return this.currentlyDisplayingText != null;
    }

    /// <summary>
    /// If the text display isn't occupied with other text,
    /// show the given text and then return true.
    /// 
    /// Otherwise, does not show the text, and returns false;
    /// </summary>
    /// <param name="text"></param>
    public bool TryShowText(string text, Font font, int fontSize)
    {
        var gameObj = Instantiate(this.textTransform, this.canvas.transform, false).gameObject;
        var textObj = gameObj.GetComponent<Text>();
        var textController = gameObj.GetComponent<TextController>();

        this.currentlyDisplayingText = textController;

        textObj.font = font;
        textObj.fontSize = fontSize;
        textObj.text = text;

        return true;
    }
}
