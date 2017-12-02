
class Character
{
    protected Attributes attributes;
    public Character()
    {
        attributes = new Attributes();
    }
    protected  struct Attributes
    {
        public int hp, armour, gold;
    }

    public void Method()
    {

    }
    public Character.Attributes d
    {
        get { return attributes; }
        set { attributes = value; }
    }
}