using UnityEngine;

public abstract class PlayerInteractionAction : MonoBehaviour
{
    public abstract float DistanceActionable { get; }

    public abstract void Action(PlayerInteract source);
}
