// ReSharper disable All

using ReedSolomon;

namespace Projet_S4;

public class QRCode
{
    private int _taille = 152;
    private int _version = 1;
    private string _mode = "0010";
    private string _nbrbin;
    private string _paires;
    private char[] _alphanum =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
        'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/',
        ':'
    };
    private string _chainbits = "";
    
    #region Fonctions d'usages
    
    public string Poids(string paires)//Calcule le poids des lettres de chaque paire
    {
        int index1;
        int index2 = 0;
        int pad ;

        if (paires.Length > 1)
        {
            index1 = Array.IndexOf(_alphanum, paires[0]) * 45;
            index2 = Array.IndexOf(_alphanum, paires[1]);
            pad = 11;
        }
        else
        {
            pad = 6;
            index1 = Array.IndexOf(_alphanum, paires[0]);
        }
        string somme = Convert.ToString(index1 + index2, 2);
        somme = somme.PadLeft(pad, '0');
        return somme;
    }

    public byte BinaryToByte(string chaine)//Convertis un octet en byte
    {
        int bytes = 0;
        for (int i = chaine.Length - 1; i >= 0; i--)
        {
            if (chaine[i].ToString() != "0")
            {
                bytes = (int) (bytes + Math.Pow(2,chaine.Length - i - 1));
            }
        }

        return Convert.ToByte(bytes);
    }
   
    #endregion


    public QRCode(string input)
    {
        if (input.Length >= 25)
        {
            _version = 2;
        }

        string poids = "";
        _nbrbin = Convert.ToString(input.Length, 2);
        _nbrbin = _nbrbin.PadLeft(9, '0');//reste à vérifier si nbrbin est bien de 8 avant

        for (int i = 0; i < input.Length; i += 2)
        {
            int pas = 1;
            if (i < input.Length - 1)
            {
                pas = 2;
            }

            _paires = input.Substring(i, pas);
            poids += Poids(_paires);
        }

        _chainbits = _chainbits + _mode + _nbrbin + poids + "0000";

        while (_chainbits.Length % 8 != 0)
        {
            _chainbits += "0";
        }

        if (_version == 2)
        {
            _taille = 272;
        }

        while (_chainbits.Length >= _taille)
        {
            _chainbits += "11101100";
            if (_chainbits.Length >= _taille)
            {
                break;
            }

            _chainbits += "00010001";
        }
        //chaine peut ne pas être assez longue

        int reedversionRS = 7;
        if (_version == 2)
        {
            reedversionRS = 10;
        }

        byte[] tabBytes = new byte[_chainbits.Length / 8];
        int count = 0;
        for (int i = 0; i < _chainbits.Length; i += 8)
        {
            tabBytes[count] = BinaryToByte(_chainbits.Substring(i, 8));
            count++;
        }

        byte[] sortie = ReedSolomonAlgorithm.Encode(tabBytes, reedversionRS,ErrorCorrectionCodeType.QRCode);
        Console.WriteLine("\n\nMode: V " + _version + "\nNiveau Correction erreur: L\n" + "Mode sur 4 bits: " + _mode + "\n" + "Nombre de caracteres sur 9 bits: " + _nbrbin + "\n" + "Données: " + _chainbits.Substring(13, 96 - 13) + "\n" + "Correction d'erreurs: " + _chainbits.Substring(97, _chainbits.Length - 98));
    }

    public void Affichage(int tailleQR)//ecriture graphique de toutes les composantes du QRCode
    {
        MyImage QRCode = new MyImage("QRcode.bmp");
        int module = 21;

        if (_version == 2)
        {
            module = 25;
        }
    }
    
    //Motifs de Recherche:
    public void MotifsDeRecherche(Pixel[,] motifs,int taille)
    {
        
    }
    //Séparateurs
    
    //Motifs de synchro
    
    //Modules centraux
    
    //
}