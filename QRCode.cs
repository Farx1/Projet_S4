// ReSharper disable All

using ReedSolomon;

namespace Projet_S4;

public class QRCode : MyImage
{

    #region Constructeur et Attributs

    private int _version;
    private int[] _modeindicator;
    private int _contours;
    private static Dictionary<char,int> _alphanum = new();
    private int _taillemessage;
    private List<int> _chainbits;
    private int _mask;
    private int _taillemodule;
    private int[] _bitwords;
    private int _nivcorrection;
    private int _datacode;
    private int _errordata;
    private string _message;
   

    public int Version
    {
        get => _version;
        set => _version = value;
    }

    public int[] ModeIndicator
    {
        get => _modeindicator;
        set => _modeindicator = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Contours
    {
        get => _contours;
        set => _contours = value;
    }


    
    public Dictionary<char,int> Alphanum
    {
        get => _alphanum;
        set => _alphanum = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<int> Chainbits
    {
        get => _chainbits;
        set => _chainbits = value;
    }

    public int Mask
    {
        get => _mask;
        set => _mask = value;
    }

    public int Taillemodule
    {
        get => _taillemodule;
        set => _taillemodule = value;
    }

    public int Nivcorrection
    {
        get => _nivcorrection;
        set => _nivcorrection = value;
    }

    public int Taillemessage
    {
        get => _taillemessage;
        set => _taillemessage = value;
    }
    
    public int Datacode
    {
        get => _datacode;
        set => _datacode = value;
    }
    
    public int ErrorData
    {
        get => _errordata;
        set => _errordata= value;
    }
    
    public int[] Bitwords
    {
        get => _bitwords;
        set => _bitwords = value;
    }
    
    public string Message
    {
        get => _message;
        set => _message = value;
    }
    
        
    
    public QRCode(int version, int contours, List<int> chainbits, int mask, int taillemodule, int nivcorrection,
        Pixel[,] imageData, string paires, int[] modeindicator, int height, int width, string typeImage, int numberRgb,
        int offset)
    {
        _version = version;
        _contours = contours;
        _chainbits = chainbits;
        _mask = mask;
        _taillemodule = taillemodule;
        _nivcorrection = nivcorrection;
        ImageData = imageData;
        _modeindicator = modeindicator;
        Height = height;
        Width = width;
        TypeImage = typeImage;
        NumberRgb = numberRgb;
        Offset = offset;



    }

    #endregion


    #region Constructeur et écriture du QRCode

    //Peut être séparé plus tard
    public QRCode(int version,int taillemodule, int contours,int nivcorrection,int masque,string message)
    {
        _message = message;
        //MeilleurVersionEtNiveauDeCorrection();
        int bordsQR = (8 * 2 + (4 * version + 1)) * taillemodule + 2 * contours;
        Height = bordsQR;
        Width = bordsQR;
        ImageData = new Pixel[Height, Width];
        MyImage QRCode = new MyImage(Height, Width, ImageData);
        _contours = contours;
        SizeFile = Height * Width * 3 + Offset;
        _taillemodule = taillemodule;
        Offset = 54;
        Ecriture = 1;
        NumberRgb = 24;
        _version =version;
        _mask = masque;
        _nivcorrection = nivcorrection;

        ModulesDeRecherches(0 + _contours, 0 +_contours);
        ModulesDeRecherches(0 + Height - (7 * _taillemodule) -_contours, 0+_contours);
        ModulesDeRecherches(0 +_contours, 0 + Width - (7 * _taillemodule)-_contours);
        Separateurs(0+_contours, 0+_contours);
        Separateurs(0 + Height - 8 * _taillemodule-_contours, 0+_contours);
        Separateurs(0+_contours, 0 + Width - 8 * _taillemodule-_contours);
        EcritureMotifsAlignement();
        MotifsDeSynchro();
        DarkModule();
        EcritureInfoVersionQRCode();
        EcritureInfoFormat();
        Dico();
        DataCodeAndErrorDataWords();
        MessageData(_message);
        ErrorCorrectionQRCode();
        //_bitwords = new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        MessageQRCode(_bitwords);
        
        QRCode.FillImageWithWhite(); //pour voir les modules non remplis


        this.From_Image_To_File($"../../../Images/QRCode_V{_version}_N{_nivcorrection}_M{_mask}.bmp");
    }
    #endregion


    #region Modules de Recherches

    public void ModulesDeRecherches(int ligne, int colonne)
    {
        for (int i = ligne; i < _taillemodule * 7 + ligne; i++)
        {
            for (int j = colonne; j < _taillemodule * 7 + colonne; j++)
            {
                if (i < _taillemodule + ligne || j < _taillemodule + colonne)
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
                else if (i > ligne + 6 * _taillemodule - 1 || j > colonne + 6 * _taillemodule - 1)
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
                else if (i >= ligne + 2 * _taillemodule && j >= colonne + 2 * _taillemodule &&
                         i <= ligne + 6 * _taillemodule - 2 * _taillemodule + (_taillemodule - 1) &&
                         j <= colonne + 6 * _taillemodule - 2 * _taillemodule + (_taillemodule - 1))
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
                else
                {
                    ImageData[i, j] ??= new Pixel(255, 255, 255);
                }
            }
        }
    }

    #endregion


    #region Séparateurs

    public void Separateurs(int ligne, int colonne)
    {
        for (int i = ligne; i < 8 * _taillemodule + ligne; i++)
        {
            for (int j = colonne; j < 8 * _taillemodule + colonne; j++)
            {
                ImageData[i, j] ??= new Pixel(255, 255, 255);
            }
        }
    }

    #endregion


    #region Motifs de Synchronisation

    public void MotifsDeSynchro()
    {
        for (int i = 7 * _taillemodule + _contours; i <= Height - 7 * _taillemodule - _contours; i++)
        {
            for (int j = 6 * _taillemodule + _contours; j < 7 * _taillemodule + _contours; j++)
            {
                if ((i - _contours) / _taillemodule % 2 == 0)
                {
                    ImageData[i, j] ??= new Pixel(0, 0, 0);
                }
                else
                {
                    ImageData[i, j] ??= new Pixel(255, 255, 255);
                }
                //On utilise le else pour vérifie l'écriture
                /*
                else
                {
                    ImageData[i, j] = new Pixel(0,0,255);
                }
                */
            }
        }

        for (int j = 7 * _taillemodule + _contours; j <= Height - 7 * _taillemodule - _contours; j++)
        {
            for (int i = 6 * _taillemodule + _contours; i < 7 * _taillemodule + _contours; i++)
            {
                if ((j - _contours) / _taillemodule % 2 == 0)
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
                else
                {
                    ImageData[i, j] = new Pixel(255, 255, 255);
                }

            }
        }



    }

    #endregion


    #region Dark Module

    public void DarkModule()
    {
        for (int j = 8 * _taillemodule + _contours; j < _taillemodule * 9 + _contours; j++)
        {
            for (int i = (4 * _version + 9) * _taillemodule + _contours;
                 i < _taillemodule + (4 * _version + 9) * _taillemodule + _contours;
                 i++)
            {
                ImageData[i, j] = new Pixel(0, 0, 0);
            }
        }
    }

    #endregion


    #region Motifs d'Alignements

    public void EcritureMotifsAlignement()
    {

        //voir https://askcodez.com/generer-toutes-les-combinaisons-pour-une-liste-de-chaines-de-caracteres.html
        //Il faut qu'on réussisse à faire une combinaison de toutes les coordonées des Motifs d'Alignements
        //le problème est de savoir ensuite quels motifs il faut mettre et lequels il faut enlever(superposition)
        if (_version < 2) return;

        var coordonees = Coordonees(@"../../../Coordonées.txt", _version);

        foreach (var intArray in coordonees)
        {
            MotifsAlignement(intArray[0], intArray[0]);
            MotifsAlignement(intArray[0], intArray[1]);
            MotifsAlignement(intArray[1], intArray[0]);
            MotifsAlignement(intArray[1], intArray[1]);

        }


    }

    public void MotifsAlignement(int ligne, int colonne)
    {
        if (ImageData[ligne, colonne] != null) return;
        for (int i = ligne - 2 * _taillemodule; i <= ligne + 2 * _taillemodule + (_taillemodule - 1); i++)
        {
            for (int j = colonne - 2 * _taillemodule; j <= colonne + 2 * _taillemodule + (_taillemodule - 1); j++)
            {

                int newligne = ((2 * _taillemodule - ligne) + i) / _taillemodule;
                int newcolonne = ((2 * _taillemodule - colonne) + j) / _taillemodule;

                if (newligne == 0 || newcolonne == 0 || newligne == 4 || newcolonne == 4)
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
                else if (newligne != 0 && newcolonne != 0 && newligne != 4 && newcolonne != 4)
                {
                    if (newligne == 2 && newcolonne == 2)
                    {
                        ImageData[i, j] = new Pixel(0, 0, 0);
                    }
                    else
                    {
                        ImageData[i, j] = new Pixel(255, 255, 255);
                    }
                }



            }
        }

    }


    public int[][] Coordonees(string nomfichier, int version) // lecture du fichier pour determiner les coordonnées
    {
        StreamReader? flux = null;
        string? lines;
        int i = 0;
        int[] coordonee = new int[] { };
        try
        {
            flux = new StreamReader(nomfichier);
            while ((lines = flux.ReadLine()) != null)
            {

                if (i == _version - 2)
                {
                    string[] ligne = lines.Split(" ");
                    coordonee = new int [ligne.Length - 1];
                    for (int j = 0; j < ligne.Length - 1; j++)
                    {
                        coordonee[j] = (Convert.ToInt32(ligne[j + 1])) * _taillemodule + _contours;
                    }
                }

                i++;
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(" Le fichier spécifié " + nomfichier +
                              " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1.Message);
        }
        finally
        {
            if (flux != null) flux.Close();
        }

        var donees = Combinaisons(coordonee);

        return donees.Where(x => x.Length == 2).ToArray();
    }

    #endregion


    #region Version du QRCode et Ecriture Version

    public int[] InfoVersionQRCode()
    {

        StreamReader? flux = null;
        int i = 0;
        string? lines;
        int[] final = new int[] { };
        if (_version < 7)
        {
            return Array.Empty<int>();
        }

        try
        {
            flux = new StreamReader(@"../../../InfosVersionQRCode.txt");
            while ((lines = flux.ReadLine()) != null)
            {
                if (i == _version - 7)
                {
                    string[] ligne = lines.Split(" ");
                    final = new int [ligne[1].Length];
                    for (int j = 0; j < ligne[1].Length; j++)
                    {
                        final[j] = (int) ligne[1][j] - 48;
                        //Console.WriteLine(final[j]);debug
                    }
                }

                i++;
            }


        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(" Le fichier spécifié " + "InfosVersionQRCode.txt" +
                              " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1.Message);
        }
        finally
        {
            if (flux != null) flux.Close();
        }

        return final;
    }

    public void EcritureInfoVersionQRCode()
    {
        if (_version < 7)
        {
            return;
        }
        else
        {
            int[] couleur = InfoVersionQRCode();
            for (int i = 0; i < couleur.Length; i++)
            {
                Pixel pixel = couleur[i] == 1 ? new Pixel(0, 0, 0) : new Pixel(255, 255, 255);
                int ligne = (2 - i % 3) * _taillemodule + ImageData.GetLength(0) - 11 * _taillemodule - _contours;
                int colonne = (5 - i / 3) * _taillemodule + _contours;
                for (int a = ligne; a < _taillemodule + ligne; a++)
                {
                    for (int b = colonne; b < _taillemodule + colonne; b++)
                    {
                        ImageData[a, b] = pixel;
                        ImageData[b, a] = pixel;
                    }
                }
            }
        }
    }

    #endregion


    #region Format QRCode et Ecriture des Info du Format


    #region Récupération des informations du format du QRCode sous un int[]

    public int[] InfoFormatQRcode()
    {

        #region Méthode pas bonne

        /*
        var bin = Convert.ToString(_mask,2);
        int[] tab = new int[bin.Length];
        for (int i = 0; i < bin.Length; i++)
        {
            tab[i] = bin[i] -48;
        }
        var correction = Convert.ToString(_nivcorrection,2);
        int[] tabcor = new int[correction.Length];
        for (int i = 0; i < correction.Length; i++)
        {
            tabcor[i] = correction[i] -48;
        }

        var finalbin = tab;
        var finalcor = tabcor;
        
        if (finalbin.Length < 3)
        {
            finalbin = MyImage.UnShift(finalbin, 2);
        }
        
        if (finalcor.Length < 2)
        {
            finalcor = MyImage.UnShift(finalcor, 2);
        }
        
        var final = finalcor.Concat(finalbin).ToArray();
        
        
        
        int[] inform = MyImage.TrimAndPad(tabcor,14);
        var poly = new[] {1, 0, 1, 0, 0, 1, 1, 0, 1, 1, 1};
        var polynome = MyImage.TrimAndPad(poly, 14);
        var div = MyImage.XOR(inform, polynome);
        div = MyImage.Trim(div);
        while (div.Length > 10)
        {
            polynome = MyImage.TrimAndPad(poly, div.Length);
            div = MyImage.XOR(div, polynome);
            div = MyImage.Trim(div);
        }

        if (div.Length < 10)
        {
            div = MyImage.Pad(div, 10);
        }
        
        var masque = new[] {1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0};
        
        
        return MyImage.XOR(masque,final.Concat(div).ToArray());
        */

        #endregion

        #region méthode Fonctionelle

        StreamReader? flux = null;
        int i = 0;
        int k = 0;
        if (_nivcorrection == 2) k = 8;
        if (_nivcorrection == 3) k = 16;
        if (_nivcorrection == 4) k = 24;

        string? lines;
        int[] final = new int[] { };
        try
        {
            flux = new StreamReader(@"../../../InfosFormatQRCode.txt");
            while ((lines = flux.ReadLine()) != null)
            {
                if (i == _mask + k)
                {
                    string[] ligne = lines.Split(" ");
                    final = new int [ligne[2].Length];
                    for (int j = 0; j < ligne[2].Length; j++)
                    {
                        final[j] = (int) ligne[2][j] - 48;
                        //Console.Write(final[j]);
                    }
                }

                i++;
            }


        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(" Le fichier spécifié " + "InfosFormatQRCode.txt" +
                              " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1.Message);
        }
        finally
        {
            if (flux != null) flux.Close();
        }

        return final;

        #endregion

    }

    #endregion


    #region Ecriture du Format sur le QRCode

    public void EcritureInfoFormat()
    {
        int[] infoformat = InfoFormatQRcode();
        int colonne = 8 * _taillemodule + _contours;
        int ligne = 8 * _taillemodule + _contours;

        int i = Height - _contours - _taillemodule ;
        int j = 0 + _contours;
        foreach (int n in infoformat)
        {
            if (ImageData[i, colonne] != null) i -= _taillemodule;
            if (ImageData[ligne, j] != null) j += _taillemodule;

            for (int l = 0; l < _taillemodule; l++)
            {
                for (int c = 0; c < _taillemodule; c++)
                {
                    ImageData[ligne + l, j + c] = n == 0 ? new Pixel(255, 255, 255) : new Pixel(0, 0, 0);
                    ImageData[i + l, colonne + c] = n == 0 ? new Pixel(255, 255, 255) : new Pixel(0, 0, 0);
                }
            }

            if (i - _taillemodule == (4 * _version + 9) * _taillemodule + _contours)
            {
                i = 8 * _taillemodule + _contours;
                continue;
            }

            if (j == 8 * _taillemodule + _contours)
            {
                j = ImageData.GetLength(0) - 7 * _taillemodule - _contours;
                continue;
            }

            for (int l = 0; l < _taillemodule; l++)
            {
                for (int c = 0; c < _taillemodule; c++)
                {
                    ImageData[ligne + l, ImageData.GetLength(0) - 8 * _taillemodule - _contours + c] = 
                        ImageData[ligne + l, ImageData.GetLength(0) - 7 * _taillemodule - _contours + c];
                }
            }

            i -= _taillemodule;
            j += _taillemodule;
        }




    }


    #endregion


    #endregion
    

    #region Identification de la meilleur version et du meilleur masque pour le QRCode

    public void MeilleurVersionEtNiveauDeCorrection()
    {
        int taillemessage = _message.Length;
        var taille = CatchFile($"../../../CharacterCapacitiesQRCodeOrdered.txt").ToArray();
        int i = 0;
        //find the best version for the message length in Char CharacterCapacitiesQRCodeOrdered.txt
        foreach (var n in taille)
        {
            var readligne1 = taille[i].Split(",");
            var readligne =taille[i+1].Split(",");
            var readligne2 = taille[i+2].Split(",");
            
            var taillebas = Convert.ToInt32(readligne1[0]);
            var taillehaut = Convert.ToInt32(readligne2[0]);
            if (taillebas < taillemessage  && taillemessage < taillehaut)
            {
                _version = Convert.ToInt32(readligne[1]);
                _nivcorrection = Convert.ToInt32(readligne[2]);
                break;
            }
            else if (taillemessage == taillebas)
            {
                _version = Convert.ToInt32(readligne1[1]);
                _nivcorrection = Convert.ToInt32(readligne1[2]);
                break;
            }
            else if (taillemessage == taillehaut)
            {
                _version = Convert.ToInt32(readligne2[1]);
                _nivcorrection = Convert.ToInt32(readligne2[2]);
                break;
            }

            i++;


        }
        
        

    }

    #endregion
    
    
    #region Récupération de la taille de la chaine de cractère dans CharacterCapacitiesQRCode

    public void DataCodeAndErrorDataWords()
    {
        var ligne = CatchFile($"../../../CharacterCapacitiesQRCode.txt").ToArray();
        //vérifier si ce n'est pas la valeur entiere correspondant plutôt
        var i = _nivcorrection
            switch
            {
                1 => 0,
                2 => 3,
                3 => 2,
                4 => 1,
                _ => 0,
            };

        var readligne = ligne[(_version - 1) * 4 + i];
        var finalinfo =readligne.Split(";");
        var final = new int[finalinfo.Length -2];
        for (int n =1; n < finalinfo.Length -1; n++)
        {
            final[n-1] = Convert.ToInt32(finalinfo[n]);
        }
        _datacode = final[0];
        _errordata = final[1]* final[2] + final[1]*final[4];

    }
    
    #endregion
    
    
    #region Calcul des Datas et Initialisation du Dictionary
    public int TotalData()
    {
        return _datacode + _errordata;
    }

    public List<int> DataByteEncoding()
    {
        return BitToByte(_chainbits.ToArray()).ToList();
    }
    public static void Dico()
    {
        
        var Dico = CatchFile("../../../AlphanumQRCode.txt");
        foreach (var n in Dico)
        {
            var args = n.Split('=');
            _alphanum.Add(Convert.ToChar(args[0]), Convert.ToInt32(args[1]));
        }
    }
    
    #endregion
    
    
    #region Masques EncodageQRCode

    public bool MasqueQRCode(int j,int i)
    {
        bool boole= _mask 
            switch
            {
                0 => (j + i) % 2 == 0,
                1 => (j) % 2 == 0,
                2 => (i) % 3 == 0,
                3 => (j + i) % 3 == 0,
                4 => ((j / 2) + (i / 3)) % 2 == 0,
                5 => ((j * i) % 2) + ((j * i) % 3) == 0,
                6 => (((j * i) % 2) + ((j * i) % 3)) % 2 == 0,
                7 => (((j + i) % 2) + ((j * i) % 3)) % 2 == 0,
                _ => (j) % 2 == 0,
            };
        return boole;
        
    }
    
    #endregion
    
    
    #region Encodage du message
    public void MessageData(string message)
    {
        message = message.ToUpper();
        _modeindicator = new int[] {0, 0, 1, 0};
        List<int> final = new List<int>(_modeindicator);
        var messagelength =message.Length;
        var bin = Convert.ToString(messagelength, 2);
        var term = new int[bin.Length];
        for (int i = 0; i < bin.Length; i++)
        {
            term[i] = bin[i] - 48;
        }

        var modif = UnShift(term, _version < 10 ? 9 : _version < 27 ? 11 : 13);
        final.AddRange(modif);

        for (int k = 0; k <= message.Length - 1; k +=2)
        {
            if (k % 2 == 0 && k != message.Length - 1)
            {
                _alphanum.TryGetValue(message[k], out var value1);
                _alphanum.TryGetValue(message[k + 1], out var value2);
                var value = value1 * 45 + value2;
                var bin1 = Convert.ToString(value, 2);
                var term1 = new int[bin1.Length];
                for (int i = 0; i < bin1.Length; i++)
                {
                    term1[i] = bin1[i] - 48;
                }
                
                var modif1 = UnShift(term1, 11);
                final.AddRange(modif1);
                
            }
            if(k==message.Length-1 && message.Length%2 ==1)
            {
                _alphanum.TryGetValue(message[k], out var index);
                var bin2 = Convert.ToString(index, 2);
                var term2 = new int[bin2.Length];
                for (int i = 0; i < bin2.Length; i++)
                {
                    term2[i] = bin2[i] - 48;
                }
            
                var modif2 = UnShift(term2, 6);
                final.AddRange(modif2);  
            }




        }

        var n = 1;
        
        while(final.Count<_datacode*8 && n<=4)
        {
            final.Add(0);
            n++;
        }
        
        final = final.Count % 8 != 0 ? Pad(final.ToArray(), final.Count + (8 - final.Count % 8)).ToList() : final;
        if (final.Count < _datacode * 8)
        {
            var add1 = Convert.ToString(236 ,2);
            var add2 = Convert.ToString(17 ,2);
            var term3 = new int[add1.Length];
            for (int i = 0; i < add1.Length; i++)
            {
                term3[i] = add1[i] - 48;
            }
            var modif3 = UnShift(term3, 8);
            
            var term4 = new int[add2.Length];
            for (int i = 0; i < add2.Length; i++)
            {
                term4[i] = add2[i] - 48;
            }
            var modif4 = UnShift(term4, 8);

            var val2 = (_datacode*8 -final.Count);
            
            for(int j = 0; j<val2/8; j++)//a revoir
            {
                final.AddRange(j%2 ==0 ? modif3 : modif4);
            }


        }
        _chainbits = final;
        
        

    }

    #endregion
    
    
    #region ErrorCorrection
    public void ErrorCorrectionQRCode()
    {
        var ggf = new GenericGF(285, 256, 0);
        var reed = new ReedSolomonEncoder(ggf);
        var data = _chainbits.ToArray();

        var errordata = _errordata;
        
        var array1 = Enumerable.Repeat(0, errordata);
        var bytes = DataByteEncoding().Concat(array1).ToArray();
        reed.Encode(bytes, errordata);
        _bitwords = ByteToBit(bytes);
        /*
        for (int i = 0; i < data.Length; i++)
        {
            Console.Write(data[i]);
        }
        Console.Write("\n");
        for (int i = 0; i < _bitwords.Length; i++)
        {
            Console.Write(_bitwords[i]);
        }
        Console.WriteLine("\n");
        Console.WriteLine("\n");
        */
    }
    
    #endregion
    
    
    #region Ecriture du message
    public void MessageQRCode(int[] tab)
    {
        
        for (int i = 0; i < _bitwords.Length; i++) 
        {
            Console.Write(_bitwords[i]);
        }
        
        Console.WriteLine("");
        
        var bas = true;
        var spec = true;
        var compteur = 0;
        

        for (var i = Width - _taillemodule - _contours; i >_contours; i-=2*_taillemodule)
        {
            if (i <= (7 * _taillemodule) && spec==true)
            {
                i= i-_taillemodule ;
                spec=false;
            }
            if (compteur >= tab.Length) break;
            if (bas)
            {
                if (compteur >= tab.Length) break;
                for (var j = Height - _taillemodule - _contours; j > _contours; j-=_taillemodule)
                {
                    
                    if (compteur >= tab.Length) break;
                    if (ImageData[j, i] == null)
                    {
                        
                        if (tab[compteur] == 0)//blanc
                        {
                            
                            var testbool = MasqueQRCode(j-_contours,i-_contours);
                            
                            if (testbool== true)
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c] = new Pixel(0, 0, 0);//switch en noir
                                    }
                                }

                                Console.Write("1");
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c] = new Pixel(255, 255, 255);//reste blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            

                        }   
                        else if (tab[compteur] == 1)//noir
                        {
                            
                            var testbool = MasqueQRCode(j-_contours,i-_contours);
                            
                            
                            if (testbool == true)
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c] = new Pixel(255, 255, 255);//witch en blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c] = new Pixel(0, 0, 0);//reste noir 
                                    }
                                } 
                                Console.Write("1");
                            }
                            

                        }
                        
                        compteur++;
                    }


                    if (ImageData[j, i - _taillemodule] == null)
                    {
                        if (tab[compteur] == 0)//blanc
                        { 
                            var testbool = MasqueQRCode(j-_contours,i-_taillemodule-_contours);
                            
                            
                            if (testbool == true)
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(0, 0, 0);//switch en noir
                                    }
                                }
                                Console.Write("1");
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(255, 255, 255);//reste blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            

                        }
                        else if (tab[compteur] == 1)//noir
                        { 
                            var testbool = MasqueQRCode(j-_contours,i-_taillemodule-_contours);
                            
                            if (testbool == true)
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(255, 255, 255);//switch en blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(0, 0, 0);//reste noir
                                    }
                                } 
                                Console.Write("1");
                            }
                            
                        }

                        compteur++;
                    }
                    


                }
                //Console.Write(" ");

            }
            
            if (compteur >= tab.Length) break;

            
            else
            {
                
                if (compteur >= tab.Length) break;
                for (var j = _contours; j <Height - _contours; j+=_taillemodule)
                {
                    
                    if (compteur >= tab.Length) break;

                    if (ImageData[j,i] == null)
                    {
                        if (tab[compteur] == 0)
                        {
                            
                            var testbool = MasqueQRCode(j-_contours,i-_contours);
                            
                            
                            if (testbool == true)//blanc
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c] = new Pixel(0, 0, 0);//switch en noir
                                    }
                                }
                                Console.Write("1");
                                
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c] = new Pixel(255, 255, 255);//reste blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            

                        }   
                        else if (tab[compteur] == 1)//noir
                        {
                            
                            var testbool = MasqueQRCode(j-_contours,i-_contours);
                            
                            
                            if (testbool == true)
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c] = new Pixel(255, 255, 255);//switch en blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l,i+c] = new Pixel(0, 0, 0);//reste noir

                                    }
                                }
                                Console.Write("1"); 
                            }
                                                 
                        }
                        compteur++;
                    }
                    
                    if (compteur >= tab.Length) break;

                    if (ImageData[j,i-_taillemodule] == null)
                    {
                        if (tab[compteur] == 0)//blanc
                        {
                            var testbool = MasqueQRCode(j-_contours,i-_taillemodule-_contours);
                            
                            if (testbool == true)
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(0, 0, 0);//switch en noir
                                    }
                                }
                                Console.Write("1");
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(255, 255, 255);//reste blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            
                        }
                        else if (tab[compteur] == 1)//noir
                        {
                            
                            var testbool = MasqueQRCode(j-_contours,i-_taillemodule-_contours);
                            
                            
                            if (testbool == true)
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(255, 255, 255);//switch en blanc
                                    }
                                }
                                Console.Write("0");
                            }
                            else
                            {
                                for (int l = 0; l < _taillemodule; l++)
                                {
                                    for (int c = 0; c < _taillemodule; c++)
                                    {
                                        ImageData[j+l, i+c-_taillemodule] = new Pixel(0, 0, 0);//reste noir
                                    }
                                }
                                Console.Write("1");
                            }
                            
                        }
                        
                        compteur++;
                    }


                }
                //Console.Write(" ");


            }
            bas = !bas;

        }
        Console.Write("\n");
    }
    
    #endregion
    
    
    #region Autres méthodes
    public static IEnumerable<T[]> Combinaisons<T>(IEnumerable<T> source)
    {
        if (null == source)
            throw new ArgumentNullException(nameof(source));

        var data = source.ToArray();

        return Enumerable
            .Range(0, 1 << (data.Length))
            .Select(index => data
                .Where((v, i) => (index & (1 << i)) != 0)
                .ToArray());
    }
    
    public static int[] TrimAndPad(int[] array, int desiredLength)
      {
          while (array[0] == 0) array = array.Skip(1).ToArray();
          var zerosArray = Enumerable.Repeat(0, desiredLength - array.Length);
          return array.Concat(zerosArray).ToArray();
      }
        
      public static int[] Pad(int[] array, int desiredLength)
      {
          var zerosArray = Enumerable.Repeat(0, desiredLength - array.Length);
          return array.Concat(zerosArray).ToArray();
      }
        
      public static int[] UnShift(int[] array, int desiredLength)
      {
          var zerosArray = Enumerable.Repeat(0, desiredLength - array.Length);
          return zerosArray.Concat(array).ToArray();
      }
        
      public static int[] Trim(int[] array)
      {
          while (array[0] == 0) array = array.Skip(1).ToArray();
          return array;
      }
      
      public static int[] XOR(int[] x, int[] y)
      {
          var result = new int[x.Length];
          for (int i = 0; i < result.Length; i++)
          {
              result[i] = x[i] == 1 && y[i] == 1 ? 0 : x[i] != y[i] ? 1 : 0; 
          }
          return result;
      }
      
      public static IEnumerable<string> CatchFile(string path)
      {
          var lignes = new Stack<string>();
          try
          {
              using var sr = new StreamReader(path);
              string line;
              while ((line = sr.ReadLine()) != null)
              {
                  lignes.Push(line);
              }
          }
          catch (Exception e)
          {
              Console.WriteLine("Le fichier n'as pas pu être lu, veuillez réessayer");
              Console.WriteLine(e.Message);
              throw new IOException();
                
          }

          return lignes.ToArray().Reverse();
      }
      
      public static int[] BitToByte(int[] data)
      {
          var final = new int[data.Length / 8];
          var bytes = 0;
          for (int i = 0; i < data.Length; i++)
          {

              bytes += (int) (Math.Pow(2, Math.Abs(i % 8 - 7)) * data[i]);
              if (i != 0 && (i + 1) % 8 == 0)
              {
                  final[i / 8] = bytes;
                  bytes = 0;
              }
          }
          return final;
      }
      
      public static int[] ByteToBit(int[] data)
      {
          var result = new int[data.Length * 8];
          for (int i = 0; i < result.Length; i++)
          {
              var division =  (int) (data[i / 8] / Math.Pow(2, Math.Abs(i % 8 - 7)));
              result[i] = division;
              data[i / 8] -= (int) (Math.Pow(2, Math.Abs(i % 8 - 7)) * division);
          }
          return result;
      }
    
    #endregion
    
    
    
    
}


    


   