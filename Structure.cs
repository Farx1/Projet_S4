using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;

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
        for (int i = 3; i >= 0; i--)
        {
            newone[i] = Convert.ToByte(val % Math.Pow(256, i));
            val -= (int) (val % Math.Pow(256, i));
        }

        return newone;
    }

    public static MyImage ReadBMP(string pov)
    {
        byte[] myfile = File.ReadAllBytes(pov);

        if (myfile[0] == 66 && myfile[1] == 77)
        {

        }
        // pour le type je sais pas comment faire pcq t'as mit que c'était un int mais pour moi c'est un string
        /*
         j'aurais fait ca : 
        if (myfile[0] == 66 && myfile[1] == 77)
        {
            string type = BMP;
        }*/
        byte[] bMP = new byte[] {myfile[0], myfile[1]};
        byte[] tabLargeur = new byte[] {myfile[18], myfile[19], myfile[20], myfile[21]};
        int largeur = Convertir_Endian_To_Int(tabLargeur);
        byte[] tabHauteur = new byte[] {myfile[22], myfile[23], myfile[24], myfile[25]};
        int hauteur = Convertir_Endian_To_Int(tabHauteur);
        byte[] tabTaille = new byte[] {myfile[2], myfile[3], myfile[4], myfile[5]};
        int taille = Convertir_Endian_To_Int(tabTaille);
        byte[] tabBits = new byte[] {myfile[28], myfile[29]};
        int bits_par_couleur = Convertir_Endian_To_Int(tabBits);
        int offset = 54;
        // pour l'imageData jsp non plus

        var image = new MyImage(type, hauteur, largeur, taille, bits_par_couleur, offset,);
        return image;
    }

    public void From_Image_To_File(string file)
    {
        
    }

}