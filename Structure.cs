using System.Diagnostics.CodeAnalysis;

system

namespace Projet;

public static class Structure
{
    public static int Convertir_Endian_To_Int(byte[] tab)
    {
        int result = 0;
        for (int i = 0; i < tab.Length; i++)
        {
            result = (int) (result + tab[i] * Math.Pow(256, i));
        }
        return result;
    }

    public static byte[] Convertir_Int_To_Endian(int val)
    {
        byte[] newone = new byte[4];
        for (int i = 3; i >=0; i--)
        {
            newone[i]= Convert.ToByte(val % Math.Pow(256, i));
            val-= (int) (val % Math.Pow(256, i));
        }
        return newone;
    }

    public static MyImage ReadBMP(string pov)
    {
        byte[] myfile = File.ReadAllBytes(pov);
        var image = new MyImage();

        byte[] bMP = {myfile(0), myfile(1)};
        byte[] 
        
    }