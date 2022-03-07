// ReSharper disable All

using ReedSolomon;

namespace Projet_S4;

public class QRCode : MyImage
{
    private int _version;
    private string _mode ;
    private int _contours;
    private string _paires;
    private char[] _alphanum =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
        'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/',
        ':'
    };
    private string _chainbits = "";
    private int _mask;
    private int _taillemodule;
    private int _nivcorrection;



    public QRCode(int version,int contours, string chainbits, int mask, int taillemodule, int nivcorrection,Pixel[,] imageData)
    {
        _version = version;
        _contours = contours;
        _chainbits = chainbits;
        _mask = mask;
        _taillemodule = taillemodule;
        _nivcorrection = nivcorrection;
        ImageData = imageData;
        

    }

    public QRCode(int taillemodule, int version, int contours)
    {
        int bordsQR = (8 * 2 + (4 * version + 1)) * taillemodule + 2 * contours;
        ImageData = new Pixel[bordsQR,bordsQR];
        MyImage QRCode = new MyImage(bordsQR,bordsQR,ImageData);
        QRCode.FillImageWithWhite();
        _version = version;
        _contours = contours;
        _taillemodule = taillemodule;
        Offset = 54;
        NumberRgb = 24;
        
        
        
    }
    /*
    public void ModulesDeRecherches(int ligne, int colonne)
    {
        var spacing = (_taillemodule - 1);
        for (int i = ligne; i < (_taillemodule * 7) + ligne; i++)
        {
            for (int j = colonne; j < (_taillemodule * 7) + colonne; j++)
            {
                if (i < _taillemodule + ligne)
                {
                    if (j < _taillemodule + colonne)
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                }

                if (i > ligne + 6 * _taillemodule - 1)
                {
                    if (j > colonne + 6 * _taillemodule - 1)
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                }
                
                if(i>ligne + 2*)
            }
        }
    }
    */




    //Modules de Recherche:
    public void ModulesDeRecherches(int ligne,int colonne)
    {
        for (int i = ligne; i < ligne + (7 * _taillemodule); i++)
        {
            for (int j = colonne; j < colonne + (7 * _taillemodule); j++)
            {
                //Partie noire du module
                ImageData[i, j] = new Pixel(0,0,0);
                    
                //Partie blanche du module
                if (((i > ligne + _taillemodule)) && (i < ligne + (2*_taillemodule)) || (i > ligne + (5*_taillemodule)) && (i < ligne + (6*_taillemodule)) || ((j > colonne + _taillemodule) && (j < colonne +(2*_taillemodule)) || (j > colonne + (5*_taillemodule)) && (j < colonne + (6*_taillemodule))))
                {
                    if ((i > ligne + _taillemodule) && (i < ligne + (6*_taillemodule)) && (j > colonne + _taillemodule) && (j < colonne + (6*_taillemodule)))
                    {
                        ImageData[i, j] = new Pixel(255, 255, 255);
                    }
                    
                }
            }
        }
    }
    
    //Séparateurs
    
    //Motifs de synchro

    
}
