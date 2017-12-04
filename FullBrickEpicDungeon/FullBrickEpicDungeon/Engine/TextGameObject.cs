using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class TextGameObject : GameObject
{
    protected SpriteFont spriteFont;
    protected Color color;
    protected string text;
    protected Vector2 cameraOffset;
    protected bool affectedByCamera;
    public TextGameObject(string assetname, int layer = 0, string id = "")
        : base(layer, id)
    {
        spriteFont = GameEnvironment.AssetManager.Content.Load<SpriteFont>(assetname);
        color = Color.White;
        cameraOffset = Vector2.Zero;
        affectedByCamera = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (affectedByCamera)
        {
            cameraOffset = GameEnvironment.CameraHelper.CameraOffset;
        }
        base.Update(gameTime);
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (visible)
        {
            spriteBatch.DrawString(spriteFont, text, GlobalPosition + cameraOffset, color);
        }
    }

    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public Vector2 Size
    {
        get
        { return spriteFont.MeasureString(text); }
    }
}