namespace Projet_S4;

public class Pixel
{
    private byte _red;
    private byte _green;
    private byte _blue;


    public Pixel(byte red, byte green, byte blue)
    {
        _red = red;
        this._green = green;
        this._blue = blue;
    }

    public byte Red
    {
        get => _red;
        set => _red = value;
    }

    public byte Green
    {
        get => _green;
        set => _green = value;
    }

    public byte Blue
    {
        get => _blue;
        set => _blue = value;
    }

    public string toString()
    {
        return _red + " " + _green + " " + _blue + " ";
    }
}
