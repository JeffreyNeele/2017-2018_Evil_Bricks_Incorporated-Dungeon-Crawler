using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

class Healthbar : SpriteGameObject
{
    Texture2D healthTexture;
    Vector2 healthBarPosition;
    Color healthColor;
    int fullHealth;
    int currentHealth;
    Rectangle healthrectangle;

    //               assetname + layer + id
    public Healthbar(ContentManager content, Vector2 playerlocation) : base("", 5, "")
    {
        healthBarPosition = playerlocation;
        LoadContent(content);
        fullHealth = healthTexture.Width;
        currentHealth = fullHealth;
        healthColor = Color.Green;
        healthrectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, currentHealth, healthTexture.Height);
    }
        

    private void LoadContent(ContentManager content)
    {
        healthTexture = content.Load<Texture2D>("Assets/Sprites/Shieldmaiden/HealthBar");
    }

    public override void Update(GameTime gameTime)
    {
        currentHealth = 
        if (currentHealth >= 0)
        healthrectangle = new Rectangle((int)healthBarPosition.X, (int)healthBarPosition.Y, currentHealth, healthTexture.Height);
        HealthColor();
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(healthTexture, healthBarPosition, healthrectangle ,healthColor);

        base.Draw(gameTime, spriteBatch);
    }

    public void HealthColor()
    {
        if (currentHealth >= (fullHealth*0.75))
        {
            healthColor = Color.Green;
        } else if (currentHealth >= (fullHealth*0.5))
        {
            healthColor = Color.Yellow;
        } else if (currentHealth >= (fullHealth * 0.25))
        {
            healthColor = Color.Orange;
        } else 
        {
            healthColor = Color.Red;
        }



    }





    //public void UpdateHealthBar()
    //{
    //    int maxhealth = baseattributes.HP;
    //    int health = attributes.HP;
    //    int displayedhealth = ((health / maxhealth) * 60);
    //    healthbarposition = new Vector2(position.X - (Width / 2), position.Y - (Height / 2 + 5));
    //    healthrectangle = new Rectangle((int)healthbarposition.X, (int)healthbarposition.Y, displayedhealth, healthbar.Height);
    //    healthtexture = GameEnvironment.ContentManager.Load<Texture2D>("Assets/Sprites/Shieldmaiden/HealthBar");
    //    healthbar.Position = healthbarposition;

    //    //not sure how to make the healthbar scale down to account for damage taken.
    //    }





}

