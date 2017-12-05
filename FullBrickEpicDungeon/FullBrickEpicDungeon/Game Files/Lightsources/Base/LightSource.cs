using Microsoft.Xna.Framework;
// A light source with a radius and a colour (more methods will be added as shaders will be done)
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
