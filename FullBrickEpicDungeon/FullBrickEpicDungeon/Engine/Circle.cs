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

    public bool CollidesWithRectangle(Rectangle rectangle)
    {
        float closestX = MathHelper.Clamp(origin.X, rectangle.Left, rectangle.Right);
        float closestY = MathHelper.Clamp(origin.Y, rectangle.Top, rectangle.Bottom);
        Vector2 distance = new Vector2(closestX, closestY);
        double distanceSquared = (float)Math.Pow(distance.X, 2) + Math.Pow(distance.Y, 2);
        return distanceSquared < Math.Pow(radius, 2);
    }
}