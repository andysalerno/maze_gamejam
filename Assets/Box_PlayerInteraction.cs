using UnityEngine;

public abstract class Box_PlayerInteraction : PlayerInteractionAction, HeadNodDetector.IHeadNodCallback
{
    /// <summary>
    /// Set to the child's implemented RootSaying in Start()
    /// </summary>
    protected Saying currentSaying;

    // Fonts available for child use
    protected static Font Amatic { get; private set; }
    protected static Font Unipix { get; private set; }

    private static int FontSize(Font font)
    {
        if (font == Amatic)
        {
            return 48;
        }
        else if (font == Unipix)
        {
            return 32;
        }
        else
        {
            return 32;
        }
    }

    /// <summary>
    /// Subclass must give a root to its Saying tree
    /// </summary>
    protected abstract Saying RootSaying { get; }

    public void Start()
    {
        if (Amatic == null)
        {
            //Arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
            Amatic = Resources.Load<Font>("Amatic-Bold");
            Unipix = Resources.Load<Font>("Unipix");
        }

        this.currentSaying = RootSaying;
    }

    public override float DistanceActionable { get; } = 1000f;

    public override void Action(PlayerInteract source)
    {
        if (this.currentSaying == null)
        {
            return;
        }

        if (source.TryShowText(this.currentSaying.Text, this.currentSaying.Font, FontSize(this.currentSaying.Font)))
        {
            if (this.currentSaying.sayingCallback != null)
            {
                this.currentSaying.sayingCallback.callBackMethod(source);
            }
            if (!this.currentSaying.IsBranchingSaying)
            {
                this.currentSaying = this.currentSaying.NextSaying;
            }
            else
            {
                // this saying requests a yes/no answer from the player
                // the callback with advance to the next Saying based on response
                source.TryRegisterHeadNodCallback(this);
            }
        }
    }

    // TODO: callback should trigger next dialog??
    public void HeadNodCallback(bool wasHeadYes)
    {
        this.currentSaying = wasHeadYes ? this.currentSaying.YesSaying : this.currentSaying.NoSaying;
        Debug.Log("Head detected: " + wasHeadYes);
    }

    protected class Saying
    {
        public interface ISayingCallback
        {
            void callBackMethod(PlayerInteract playerInteract);
        }

        public string Text { get; private set; }
        public Font Font { get; private set; }

        public ISayingCallback sayingCallback { get; private set; }

        public bool IsBranchingSaying { get; private set; }

        /// <summary>
        /// The next Saying to use
        /// if this saying is NOT a branching saying
        /// </summary>
        public Saying NextSaying { get; private set; }

        /// <summary>
        /// The next Saying to use
        /// if this is a branching saying
        /// and the branch condition is "yes"
        /// </summary>
        public Saying YesSaying { get; private set; }

        /// <summary>
        /// The next Saying to use
        /// if this is a branching saying
        /// and the branch condition is "no"
        /// </summary>
        public Saying NoSaying { get; private set; }

        public Saying SetNextSaying(Saying nextSaying)
        {
            this.IsBranchingSaying = false;
            this.NextSaying = nextSaying;

            this.NoSaying = null;
            this.YesSaying = null;

            return nextSaying;
        }

        /// <summary>
        /// Same as <see cref="SetNextSaying(Saying)"/>,
        /// but it returns the parent for convenience,
        /// instead of the child
        /// </summary>
        /// <param name="nextSaying"></param>
        /// <returns></returns>
        public Saying SetNextSaying_Parent(Saying nextSaying)
        {
            this.IsBranchingSaying = false;
            this.NextSaying = nextSaying;

            this.NoSaying = null;
            this.YesSaying = null;

            return this;
        }

        public Saying SetYesSaying(Saying yesSaying)
        {
            this.IsBranchingSaying = true;
            this.YesSaying = yesSaying;

            this.NextSaying = null;

            return this;
        }

        public Saying SetNoSaying(Saying noSaying)
        {
            this.IsBranchingSaying = true;
            this.NoSaying = noSaying;

            this.NextSaying = null;

            return this;
        }

        public Saying(string text, Font font)
        {
            this.Text = text;
            this.Font = font;
        }

        public Saying Loop()
        {
            this.SetNextSaying(this);

            return this;
        }

        /// <summary>
        /// Traverse through all "NextSayings"
        /// and return the very last one.
        /// Throws if the end is a yes/no branch instead
        /// of an empty NextSaying.
        /// </summary>
        public Saying GetLast()
        {
            Saying cursor = this;

            while (cursor.NextSaying != null)
            {
                cursor = cursor.NextSaying;
            }

            // if we reached the end but hit a yes/no branch instead, throw

            if (cursor.YesSaying != null || cursor.NoSaying != null)
            {
                throw new System.Exception("reached an unexpected yes/no branch!");
            }

            return cursor;
        }

        /// <summary>
        /// Get the very next yes/no branch node.
        /// This node has a yes/no branching option.
        /// Throws if none found.
        /// </summary>
        /// <returns></returns>
        public Saying GetYesNo()
        {
            Saying cursor = this;

            while (cursor.NextSaying != null)
            {
                cursor = cursor.NextSaying;
            }

            // if we reached the end but hit a yes/no branch instead, throw

            if (cursor.YesSaying != null || cursor.NoSaying != null)
            {
                // found a yes/no branching path.
                return cursor;
            }
            else
            {
                // throw
            }

            return cursor;
        }

        public Saying(string text, Font font, ISayingCallback callback)
        {
            this.Text = text;
            this.Font = font;
            this.sayingCallback = callback;
        }
    }
}
