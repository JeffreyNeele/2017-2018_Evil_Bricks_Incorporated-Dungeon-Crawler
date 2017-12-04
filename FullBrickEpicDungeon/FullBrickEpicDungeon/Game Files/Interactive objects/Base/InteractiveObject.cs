abstract class InteractiveObject : SpriteGameObject
{
    protected bool interacting;
    Character targetCharacter;
    protected InteractiveObject(string assetName, string id, int sheetIndex, int layer = 2) : base(assetName, layer, id, sheetIndex)
    {
        interacting = false;
        targetCharacter = null;
    }

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