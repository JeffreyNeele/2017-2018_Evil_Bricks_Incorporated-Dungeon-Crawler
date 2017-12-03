class Equipment : SpriteGameObject
{
    BaseAttributes equipmentstatistic;
    string ID;
    protected Equipment(string assetName, string id, int sheetIndex, int layer = 0) : base(assetName, layer, id, sheetIndex)
    {
        equipmentstatistic = new BaseAttributes();
    }

    public BaseAttributes Statistics
    {
        get { return equipmentstatistic; }
        protected set { equipmentstatistic = value; }
    }
}
