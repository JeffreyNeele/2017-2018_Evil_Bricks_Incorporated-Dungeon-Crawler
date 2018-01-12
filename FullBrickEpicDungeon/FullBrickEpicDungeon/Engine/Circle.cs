using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class Circle
{
    private double radius;
    private Vector2 origin;
    public Circle(double radius, Vector2 origin)
    {
        this.radius = radius;
        this.origin = origin;
    }

    public double Diameter
    {
        get { return radius * 2; }
    }

    public double Area
    {
        get
        {
            return Math.PI * Math.Pow(radius, 2);
        }
    }

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

    public bool CollidesWithRectangle(SpriteGameObject target, SpriteGameObject owner)
    {
        float targetCenterX = target.Position.X + target.Width / 2;
        float targetCenterY = target.Position.Y + target.Width / 2;
        Vector2 distanceVector = new Vector2(owner.Position.X - targetCenterX, owner.Position.Y - targetCenterY);
        double distance = (float)Math.Sqrt(distanceVector.X * distanceVector.X + distanceVector.Y + distanceVector.Y);
        return distance < radius;
    }
}