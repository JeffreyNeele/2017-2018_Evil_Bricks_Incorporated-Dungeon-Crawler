using Microsoft.Xna.Framework;

abstract class AImonster : Monster
{
    protected Vector2 previousPos;
    protected BaseAI AI;
    /// <summary>
    /// Class that defines a Monster with Artificial intelligence (for more info on the AI, see the BaseAI class)
    /// </summary>
    /// <param name="movementSpeed">How fast the AI of this monster moves</param>
    /// <param name="currentLevel">The Current level this monster is in</param>
    /// <param name="id">ID of the monster</param>
    public AImonster(float movementSpeed, Level currentLevel, string id, float SightRange) : base(id, currentLevel)
    {
        // Make a new AI
        AI = new BaseAI(this, movementSpeed, currentLevel, true, SightRange);
    }

    /// <summary>
    /// Updates the AI Monster
    /// </summary>
    /// <param name="gameTime">Current game time</param>
    public override void Update(GameTime gameTime)
    {
        previousPos = position;
        // Updates the AI
        AI.Update(gameTime);

        base.Update(gameTime);
    }
    
}
