// Class for an object that interacts with the player when it touches the object
using Microsoft.Xna.Framework;

abstract class InteractiveObject : SpriteGameObject
{
    protected bool interacting;
    Character targetCharacter;
    protected InteractiveObject(string assetName, string id, int sheetIndex, int layer = 2) : base(assetName, layer, id, sheetIndex)
    {
        interacting = false;
        targetCharacter = null;
    }
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