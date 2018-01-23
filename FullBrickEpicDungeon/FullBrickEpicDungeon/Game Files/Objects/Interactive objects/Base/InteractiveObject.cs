using Microsoft.Xna.Framework;

/// <summary>
/// Class for an object that interacts with the player when it touches the object
/// </summary>
abstract class InteractiveObject : SpriteGameObject
{
    protected bool interacting;
    protected Character targetCharacter;
    protected Level currentlevel;

    /// <param name="assetName">Path to the sprite to load in the object</param>
    /// <param name="id">id to be able to find the object</param>
    /// <param name="sheetIndex">Defines which picture of the animation will be shown</param>
    /// <param name="layer">The layer where the interactive object will be placed</param>
    protected InteractiveObject(string assetName, Level currentlevel, string id, int sheetIndex, int layer = 2) : base(assetName, layer, id, sheetIndex)
    {
        this.currentlevel = currentlevel;
        interacting = false;
        targetCharacter = null;
    }

    //Update when a target is interacting
    public override void Update(GameTime gameTime)
    {
        if (IsInteracting)
        {
            this.Interact(targetCharacter);
        }
        base.Update(gameTime);
    }

    protected abstract void Interact(Character character);
    
    // Properties that say if the object is currently interacting and the target character for this interaction
    public Character TargetCharacter
    {
        get { return targetCharacter; }
        set { targetCharacter = value; }
    }
    public bool IsInteracting
    {
        get { return interacting; }
        set { interacting = value; }
    }
}