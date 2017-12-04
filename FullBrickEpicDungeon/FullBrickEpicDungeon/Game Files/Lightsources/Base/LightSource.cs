using Microsoft.Xna.Framework;
abstract class LightSource : SpriteGameObject
{
    protected float lightRadius;
    protected Color lightColor;
    protected LightSource(string assetName, float lightRadius = 10) : base(assetName)
    {
        this.lightRadius = lightRadius;
    }

    public float LightRadius
    {
        get { return lightRadius; }
        protected set { lightRadius = value; }
    }

    public Color LightColour
    {
        get { return lightColor; }
        protected set { lightColor = value; }
    }
}
