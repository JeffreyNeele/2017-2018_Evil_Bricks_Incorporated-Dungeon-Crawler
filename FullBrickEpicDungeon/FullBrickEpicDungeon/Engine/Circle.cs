using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

// Self made class that defines a simple circle, used for the Line of sight (LOS) checker
class Circle
{
    // variables for radius and origin
    private double radius;
    private Vector2 origin;

    public Circle(double radius, Vector2 origin)
    {
        this.radius = radius;
        this.origin = origin;
    }

    // returns diamater
    public double Diameter
    {
        get { return radius * 2; }
    }

    // returns area
    public double Area
    {
        get
        {
            return Math.PI * Math.Pow(radius, 2);
        }
    }

    // returns circumference
    public double Circumference
    {
        get { return 2 * Math.PI * radius; }
    }

    public double Radius
    {
        get { return radius; }
    }

    public Vector2 Origin
    {
        get { return origin; }
    }

    // method that checks if the circle collides with a rectangle
    public bool CollidesWithRectangle(SpriteGameObject target, SpriteGameObject owner)
    {
        float targetCenterX = target.Position.X + target.Width / 2;
        float targetCenterY = target.Position.Y + target.Width / 2;
        Vector2 distanceVector = new Vector2(owner.Position.X - targetCenterX, owner.Position.Y - targetCenterY);
        double distance = (float)Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y + distanceVector.Y);
        return distance < radius;
    }
}