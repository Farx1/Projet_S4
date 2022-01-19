namespace Projet_S4;

public class Pixel
{
    private int _red;
    private int _green;
    private int _blue;


    public Pixel(int red, int green, int blue)
    {
        _red = red;
        this._green = green;
        this._blue = blue;
    }

    public int Red
    {
        get => _red;
        set => _red = value;
    }

    public int Green
    {
        get => _green;
        set => _green = value;
    }

    public int Blue
    {
        get => _blue;
        set => _blue = value;
    }
}
