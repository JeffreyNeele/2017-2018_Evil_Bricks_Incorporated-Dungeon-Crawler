using System;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Teacup : InteractiveObject
{
    int objectnumber = 0;

    public Teacup(string assetname, string id, int sheetIndex) : base(assetname, id, sheetIndex)
    {
    }

    public override void Update(GameTime gametime)
    {
        if (!TeacupSet)
            PushTeacup();
        SolidCollisionChecker();
    }

    protected override void Interact(Character targetCharacter)
    {

    }

    public void PushTeacup()
    {

        GameObjectList players = GameWorld.Find("playerLIST") as GameObjectList;
        foreach (Character player in players.Children)
        {
            if (player.CollidesWith(this))
            {
                Vector2 offset = Vector2.Zero;
                if (Math.Abs(player.PreviousDirection.X) >= Math.Abs(player.PreviousDirection.Y))
                {
                    if (player.PreviousDirection.X < 0)
                        offset.X -= player.PreviousDirection.X;
                    else if (player.PreviousDirection.X > 0)
                        offset.X += player.PreviousDirection.X;
                }
                else if (Math.Abs(player.PreviousDirection.X) < Math.Abs(player.PreviousDirection.Y))
                {
                    if (player.PreviousDirection.Y < 0)
                        offset.Y -= player.PreviousDirection.Y;
                    else if (player.PreviousDirection.Y > 0)
                        offset.Y += player.PreviousDirection.Y;
                }
                else
                    continue;
                Vector2 teacupgoal = this.position + offset;

                if (this != null && (SolidCollisionChecker()))
                {
                    this.position = teacupgoal;
                }
            }
        }
    }

    private bool SolidCollisionChecker()
    {
        GameObjectGrid Field = GameWorld.Find("TileField") as GameObjectGrid;
        // Define a quarter bounding box (the feet plus part of the legs) for isometric collision
        Rectangle BoundingBox = new Rectangle((int)this.BoundingBox.X, (int)(this.BoundingBox.Y), this.Width, (int)(this.Height));
        foreach (Tile tile in Field.Objects)
        {
            if (tile.IsSolid && BoundingBox.Intersects(tile.BoundingBox))
            {
                return false;
            }
            if (tile is VerticalDoor)
                if (BoundingBox.Intersects(((VerticalDoor)tile).BoundingBox2))
                    return false;
        }
        return true;
    }

    public bool TeacupSet
    {
        get
        {
            GameObjectGrid Field = GameWorld.Find("TileField") as GameObjectGrid;
            Vector2 middle = new Vector2((int)(this.BoundingBox.X + this.Width / 2), (int)(this.BoundingBox.Y + this.Height / 2));
            foreach (Tile tile in Field.Objects)
            {
                if (tile.IsTeacupGoal && tile.BoundingBox.Contains(middle))
                    return true;
            }
            return false;
        }
    }

    public int Objectnumber
    {
        get { return this.objectnumber; }
        set { objectnumber = value; }
    }

}





