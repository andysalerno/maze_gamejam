using UnityEngine;

public abstract class ABox_PlayerInteraction : APlayerInteractionAction, HeadNodDetector.IHeadNodCallback
{
    /// <summary>
    /// Set to the child's implemented RootSaying in Start()
    /// </summary>
    protected Saying currentSaying;

    // Fonts available for child use
    protected static Font Amatic { get; private set; }
    protected static Font Unipix { get; private set; }
    protected static Font Ostrich { get; private set; }

    protected Font DefaultFont { get; set; }

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
    /// If you really need to, you can set the saying manually through here
    /// In most cases, better to let the tree walk itself, however
    /// </summary>
    /// <param name="saying"></param>
    public void SetCurrentSaying(Saying saying)
    {
        this.currentSaying = saying;
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
            Ostrich = Resources.Load<Font>("OstrichSans-Heavy");
        }

        this.ResetFromDialogTree();
    }

    /// <summary>
    /// Sets the current dialog state
    /// by checking the game conditions
    /// to determine which dialog tree to initialize
    /// </summary>
    private void ResetFromDialogTree()
    {
        var tipOfTree = this.DialogTree;
        if (tipOfTree != null)
        {
            this.currentSaying = tipOfTree;
        }
    }

    public override float DistanceActionable { get; } = 1000f;

    /// <summary>
    /// Dialog works like this:
    /// 
    /// We start by pulling a root node from the DialogTree,
    /// which has its own logic to determine the root node.
    /// 
    /// We walk down the dialog tree,
    /// performing all callbacks BEFORE showing the current Saying
    /// 
    /// After displaying a Saying node, we check if it was a EndBranch node,
    /// and if it was, we reset the CurrentSaying by pulling from the DialogTree again.
    /// 
    /// If the dialog tree has nothing for us and gives null, we carry on without modifying CurrentSaying.
    /// </summary>
    /// <param name="player"></param>
    public override void Action(PlayerInteract player)
    {
        if (this.currentSaying == null)
        {
            return;
        }

        // before anything else, perform the callback
        if (this.currentSaying.sayingCallback != null)
        {
            this.currentSaying.sayingCallback.callBackMethod(player, this);
        }

        player.ShowText(this.currentSaying.Text, this.currentSaying.Font, FontSize(this.currentSaying.Font));

        if (this.currentSaying.IsEndBranch && this.DialogTree != null)
        {
            this.currentSaying = this.DialogTree;
        }
        else if (!this.currentSaying.DoesSayingBranch)
        {
            this.currentSaying = this.currentSaying.NextSaying;
        }
        else
        {
            // this saying requests a yes/no answer from the player
            // the callback with advance to the next Saying based on response
            player.TryRegisterHeadNodCallback(this);
        }
    }

    // TODO: callback should trigger next dialog??
    public void HeadNodCallback(bool wasHeadYes)
    {
        this.currentSaying = wasHeadYes ? this.currentSaying.YesSaying : this.currentSaying.NoSaying;
        Debug.Log("Head detected: " + wasHeadYes);
    }

    public class Saying
    {
        public interface ISayingCallback
        {
            void callBackMethod(PlayerInteract playerInteract, ABox_PlayerInteraction interactee);
        }

        public string Text { get; private set; }
        public Font Font { get; private set; }

        public ISayingCallback sayingCallback { get; private set; }

        //public bool DoesSayingBranch { get; private set; }
        public bool DoesSayingBranch => this.YesSaying != null || this.NoSaying != null;

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
            // this.DoesSayingBranch = false;
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
            // this.DoesSayingBranch = false;
            this.NextSaying = nextSaying;

            this.NoSaying = null;
            this.YesSaying = null;

            return this;
        }

        public Saying SetYesSaying(Saying yesSaying)
        {
            // this.DoesSayingBranch = true;
            this.YesSaying = yesSaying;

            this.NextSaying = null;

            return this;
        }

        public Saying SetNoSaying(Saying noSaying)
        {
            // this.DoesSayingBranch = true;
            this.NoSaying = noSaying;

            this.NextSaying = null;

            return this;
        }

        public Saying EndBranch()
        {
            this.IsEndBranch = true;

            return this;
        }

        public bool IsEndBranch { get; private set; }

        ///// <summary>
        ///// True if this is a leaf node,
        ///// *including if it refers to itself*,
        ///// but NOT if it is part of a cycle larger than just itself
        ///// </summary>
        ///// <returns></returns>
        //public bool IsLeaf()
        //{
        //    // bit hacky, but we might want to force
        //    // a saying to be a transition saying
        //    // even if it has children
        //    if (this.IsForcedLeaf)
        //    {
        //        return true;
        //    }

        //    return
        //    (this.NextSaying == null || this.NextSaying == this)
        //    && (this.YesSaying == null || this.YesSaying == this)
        //    && (this.NoSaying == null || this.NoSaying == this);
        //}

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
