using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public int maxScreenHeightPercent = 50;
    public int exitTimeMs = 1000;

    /// <summary>
    /// How long the text is allowed to survive
    /// on the Canvas before it destroys itself
    /// </summary>
    public float maxAliveTimeMs = 3000;

    private float exitDistance;
    private RectTransform rectTransform;

    Text text;

    private bool doExiting = false;
    private float exitingTimeMs = 0;

    private void Start()
    {
        this.rectTransform = this.GetComponent<RectTransform>();
        this.exitDistance = ((float)Screen.height * ((float)this.maxScreenHeightPercent / 100f)) - this.rectTransform.offsetMax.y;

        this.text = this.GetComponent<Text>();
    }

    public void ExitAnimation()
    {
        this.doExiting = true;
    }

    void Update()
    {
        float deltaTimeMs = Time.deltaTime * 1000;
        this.maxAliveTimeMs -= deltaTimeMs;

        if (this.maxAliveTimeMs <= 0)
        {
            this.doExiting = true;
        }

        if (!doExiting)
        {
            return;
        }

        this.exitingTimeMs += deltaTimeMs;

        if (this.exitingTimeMs < this.exitTimeMs)
        {
            // during this time, the text should float upward, and lose opacity 
            float distance = ((float)deltaTimeMs / this.exitTimeMs) * this.exitDistance;
            float opacityReduction = ((float)deltaTimeMs / this.exitTimeMs);

            this.rectTransform.Translate(0, distance, 0);
            this.text.color = new Color(this.text.color.r, this.text.color.g, this.text.color.b, this.text.color.a - opacityReduction);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
