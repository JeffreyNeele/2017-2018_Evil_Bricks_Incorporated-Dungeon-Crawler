using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// functions of a healthbar that shows the current health of the player

class Healthbar : SpriteGameObject
{   
    Character Owner;
    Vector2 healthBarPosition;
    Vector2 healthBarOffset;
    Texture2D healthTexture;
    Rectangle healthBarRectangle;
    Color healthColor;
    int currentHealth;

    // initializes the healthbar
    public Healthbar(Character characterOwner) : base("", 5, "")
    {
        healthTexture = GameEnvironment.AssetManager.GetSprite("Assets/Sprites/Shieldmaiden/HealthBar");
        Owner = characterOwner;
        healthBarOffset = new Vector2(-50, -60);
        healthBarPosition = Owner.Position + healthBarOffset;
        currentHealth = Owner.Attributes.HP;
        healthColor = Color.Red;
        healthBarRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, currentHealth, healthTexture.Height);
    }

    // updates the current health amount and the current location of the healthbar
    public override void Update(GameTime gameTime)
    {
        currentHealth = Owner.Attributes.HP;
        if (currentHealth > 0)
        {
            healthBarPosition = Owner.Position + healthBarOffset;           
            healthBarRectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, currentHealth, healthTexture.Height);
        }
        base.Update(gameTime);
    }

    // draws the healthbar at the desired location
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        if (currentHealth > 0)
        {
            spriteBatch.Draw(healthTexture, healthBarPosition, healthBarRectangle, healthColor);
        }
        base.Draw(gameTime, spriteBatch);
    }

}