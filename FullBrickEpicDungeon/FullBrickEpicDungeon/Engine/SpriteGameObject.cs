using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteGameObject : GameObject
{
    protected SpriteSheet sprite;
    protected Vector2 origin, cameraOffset;
    protected bool affectedByCamera;
    protected string assetName;
    protected int sheetIndex;
    public bool PerPixelCollisionDetection = true;
    public SpriteGameObject(string assetName, int layer = 0, string id = "", int sheetIndex = 0, bool affectedByCamera = true)
        : base(layer, id)
    {
        this.assetName = assetName;
        this.sheetIndex = sheetIndex;
        this.affectedByCamera = affectedByCamera;
        if (assetName != "")
        {
            sprite = new SpriteSheet(assetName, sheetIndex);
        }
        else
        {
            sprite = null;
        }
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
        if (!visible || sprite == null)
        {
            return;
        }
        
        sprite.Draw(spriteBatch, this.GlobalPosition + cameraOffset, origin);
    }
    public void ChangeSpriteIndex(int index)
    {
        sprite = new SpriteSheet(assetName, index);
    }
    public void ChangeSpriteImage(string assetname)
    {
        sprite = new SpriteSheet(assetname, sheetIndex);
    }
    public SpriteSheet Sprite
    {
        get { return sprite; }
    }

    public Vector2 Center
    {
        get { return new Vector2(Width, Height) / 2; }
    }

    public int Width
    {
        get
        {
            return sprite.Width;
        }
    }

    public int Height
    {
        get
        {
            return sprite.Height;
        }
    }

    public bool Mirror
    {
        get { return sprite.Mirror; }
        set { sprite.Mirror = value; }
    }

    public Vector2 Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    public bool CameraManipulation
    {
        get { return affectedByCamera; }
        set { affectedByCamera = value; }
    }

    public Vector2 CameraOffset
    {
        get { return cameraOffset; }
        set { cameraOffset = value; }
    }
    public override Rectangle BoundingBox
    {
        get
        {
            int left = (int)(GlobalPosition.X - origin.X);
            int top = (int)(GlobalPosition.Y - origin.Y);
            return new Rectangle(left, top, Width, Height);
        }
    }

    public bool CollidesWith(SpriteGameObject obj)
    {
        if (!visible || !obj.visible || !BoundingBox.Intersects(obj.BoundingBox))
        {
            return false;
        }
        if (!PerPixelCollisionDetection)
        {
            return true;
        }
        Rectangle b = Collision.Intersection(BoundingBox, obj.BoundingBox);
        for (int x = 0; x < b.Width; x++)
        {
            for (int y = 0; y < b.Height; y++)
            {
                int thisx = b.X - (int)(GlobalPosition.X - origin.X) + x;
                int thisy = b.Y - (int)(GlobalPosition.Y - origin.Y) + y;
                int objx = b.X - (int)(obj.GlobalPosition.X - obj.origin.X) + x;
                int objy = b.Y - (int)(obj.GlobalPosition.Y - obj.origin.Y) + y;
                if (sprite.IsTranslucent(thisx, thisy) && obj.sprite.IsTranslucent(objx, objy))
                {
                    return true;
                }
            }
        }
        return false;
    }
}

