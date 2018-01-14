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
    /// The dialog tree that will execute
    /// </summary>
    protected abstract Saying DialogTree { get; }

    public void Start()
    {
        if (Amatic == null)
        {
            //Arial = Resources.GetBuiltinResource<Font>("Arial.ttf");
            Amatic = Resources.Load<Font>("Amatic-Bold");
            Unipix = Resources.Load<Font>("Unipix");
        }

        this.UpdateDialogTree();
    }

    /// <summary>
    /// Sets the current dialog state
    /// by checking the game conditions
    /// to determine which dialog tree to initialize
    /// </summary>
    private void UpdateDialogTree()
    {
        var tipOfTree = this.DialogTree;
        if (tipOfTree != null)
        {
            // DialogTree may be null in cases
            // where the current looping leaf of one tree
            // should continue looping until some trigger event
            // allows us to move on
            this.currentSaying = tipOfTree;
        }
    }

    public override float DistanceActionable { get; } = 1000f;

    public override void Action(PlayerInteract source)
    {
        if (this.currentSaying == null)
        {
            return;
        }

        // before anything else, perform the callback
        if (this.currentSaying.sayingCallback != null)
        {
            this.currentSaying.sayingCallback.callBackMethod(source);
        }

        if (this.currentSaying.IsLeaf())
        {
            this.UpdateDialogTree();
        }

        if (source.TryShowText(this.currentSaying.Text, this.currentSaying.Font, FontSize(this.currentSaying.Font)))
        {
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

        /// <summary>
        /// True if this is a leaf node,
        /// *including if it refers to itself*,
        /// but NOT if it is part of a cycle larger than just itself
        /// </summary>
        /// <returns></returns>
        public bool IsLeaf()
        {
            return
            (this.NextSaying == null || this.NextSaying == this)
            && (this.YesSaying == null || this.YesSaying == this)
            && (this.NoSaying == null || this.NoSaying == this);
        }

        /// <summary>
        /// For convenience,
        /// walk down the tree from this node
        /// and give every leaf branch found the given callback
        /// </summary>
        /// <param name="callback"></param>
        public void GiveAllEndingsCallback(ISayingCallback callback)
        {
            bool foundAnyChildren = false;
            if (this.NextSaying != null && this.NextSaying != this)
            {
                this.NextSaying.GiveAllEndingsCallback(callback);
                foundAnyChildren = true;
            }
            if (this.YesSaying != null && this.YesSaying != this)
            {
                this.YesSaying.GiveAllEndingsCallback(callback);
                foundAnyChildren = true;
            }
            if (this.NoSaying != null && this.NoSaying != this)
            {
                this.NoSaying.GiveAllEndingsCallback(callback);
                foundAnyChildren = true;
            }

            if (!foundAnyChildren)
            {
                this.sayingCallback = callback;
            }
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
        /// <remarks>This will buffer overflow if you have a loop! </remarks>
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
        /// <remarks>This will buffer overflow if you have a loop! </remarks>
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
