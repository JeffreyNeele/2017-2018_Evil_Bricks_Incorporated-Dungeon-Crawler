using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Healthbar : SpriteGameObject
{   
    Character Owner;
    Vector2 healthBarPosition, healthBarOffset;
    Texture2D healthTexture;
    Rectangle healthBarRectangle;
    Color healthColor;
    /// <summary>
    /// Class that defines a healthbar that shows a red rectangle in relation to the player
    /// </summary>
    /// <param name="Owner">Owner of this health bar</param>
    public Healthbar(Character Owner) : base("", 5, "")
    {
        healthTexture = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Shieldmaiden/HealthBar");
        this.Owner = Owner;
        healthBarOffset = new Vector2(-50, -60);
        healthBarPosition = Owner.Position + healthBarOffset;
        healthColor = Color.Red;
        healthBarRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, Owner.Attributes.HP, healthTexture.Height);
    }

    // updates the current health amount and the current location of the healthbar
    public override void Update(GameTime gameTime)
    {
        if (Owner.Attributes.HP > 0)
        {
            healthBarPosition = Owner.Position + healthBarOffset;           
            healthBarRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, Owner.Attributes.HP, healthTexture.Height);
        }
        base.Update(gameTime);
    }

    // draws the healthbar at the desired location
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (Owner.Attributes.HP > 0)
        {
            spriteBatch.Draw(healthTexture, healthBarPosition, healthBarRectangle, healthColor);
        }

        base.Draw(gameTime, spriteBatch);
    }

}