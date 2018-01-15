using UnityEngine;

public class BabbleCorruptionScript : MonoBehaviour
{
    private const string Corrupt = "[SYSTEM_CORRUPT]";
    private readonly char[] hexChars = "0123456789ABCDEF".ToCharArray();

    private const int randStringLen = 8;

    private System.Random random;

    public int timeToLiveMs = 3000;
    private float timeAlive = 0;

    public int timePerBabbleMs = 50;
    private float timeSinceLastBabbleMs = 0;

    public const float showCorruptOdds = 0.05f;

    private PlayerInteract playerInteract;

    private Font font;

    void Start()
    {
        this.playerInteract = this.GetComponent<PlayerInteract>();
        this.font = Resources.Load<Font>("Unipix");
        this.random = new System.Random();

        if (this.font == null)
        {
            Debug.Log("error finding font");
        }
    }

    void Update()
    {
        float deltaTimeMs = Time.deltaTime * 1000;

        this.timeAlive += deltaTimeMs;
        this.timeSinceLastBabbleMs += deltaTimeMs;

        if (this.timeAlive > this.timeToLiveMs)
        {
            Destroy(this);
        }

        if (this.timeSinceLastBabbleMs > timePerBabbleMs)
        {
            this.Babble();
            this.timeSinceLastBabbleMs = 0;
        }
    }

    private void Babble()
    {
        Debug.Log("babbling");

        var message = Corrupt.ToCharArray();

        for (int i = 0; i < message.Length; i++)
        {
            float rand = Random.value;

            if (rand < showCorruptOdds)
            {
                message[i] = hexChars[this.random.Next(hexChars.Length)];
            }
        }

        this.playerInteract.ForceShowText(new string(message), font, 32);
    }
}
