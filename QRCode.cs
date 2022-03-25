// ReSharper disable All

namespace Projet_S4;
public class QRCode : MyImage
{
    
    #region Constructeur et Attributs
    
    private int _version;
    private string _modecorrection ; 
    private int _contours;
    private char[] _alphanum =
    {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
        'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', ' ', '$', '%', '*', '+', '-', '.', '/',
        ':'
    };
    private string _chainbits ;
    private int _mask;
    private int _taillemodule;
    private int _nivcorrection;

    public int Version
    {
        get => _version;
        set => _version = value;
    }

    public string Mode
    {
        get => _modecorrection;
        set => _modecorrection = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Contours
    {
        get => _contours;
        set => _contours = value;
    }
    

    public char[] Alphanum
    {
        get => _alphanum;
        set => _alphanum = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Chainbits
    {
        get => _chainbits;
        set => _chainbits = value ;
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

    public QRCode(int version,int contours, string chainbits, int mask, int taillemodule, int nivcorrection,Pixel[,] imageData, string paires,string modecorrection,int height,int width,string typeImage,int numberRgb,int offset)
    {
        _version = version;
        _contours = contours;
        _chainbits = chainbits;
        _mask = mask;
        _taillemodule = taillemodule;
        _nivcorrection = nivcorrection;
        ImageData = imageData;
        _modecorrection = modecorrection;
        Height = height;
        Width = width;
        TypeImage = typeImage;
        NumberRgb = numberRgb;
        Offset = offset;
        



    }
    #endregion
    
    
    #region Constructeur et écriture du QRCode
    //Peut être séparé plus tard
    public QRCode(int taillemodule, int version, int contours, int nivcorrection, int mask)
    {
        int bordsQR = (8 * 2 + (4 * version + 1)) * taillemodule + 2 * contours;
        Height = bordsQR;
        Width = bordsQR;
        ImageData = new Pixel[Height,Width];
        MyImage QRCode = new MyImage(Height,Width,ImageData);
        
        _version = version;
        _contours = contours;
        SizeFile = Height * Width * 3 + Offset;
        _taillemodule = taillemodule;
        Offset = 54;
        Ecriture = 1;
        NumberRgb = 24;
        _mask = mask;
        _nivcorrection = nivcorrection;
        ModulesDeRecherches(0 + _contours, 0 + _contours);
        ModulesDeRecherches(0 + Height - (7 * _taillemodule) - _contours,0+_contours);
        ModulesDeRecherches(0 +  _contours, 0 + Width - (7 * _taillemodule) - _contours);
        Separateurs(0 + _contours, 0 + _contours);
        Separateurs(0 + Height - 8 * _taillemodule - _contours,0+_contours);
        Separateurs(0 + _contours, 0 + Width - 8 * _taillemodule - _contours);
        EcritureMotifsAlignement();
        MotifsDeSynchro();
        DarkModule();
        EcritureInfoVersionQRCode();
        EcritureInfoFormat();
        
        
        
        QRCode.FillImageWithGrey();//pour voir les modules non remplis
        
        
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
                else if(i > ligne + 6 * _taillemodule - 1||j > colonne + 6 * _taillemodule - 1)
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
                else if(i >= ligne + 2 * _taillemodule && j >= colonne + 2 * _taillemodule &&
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
                ImageData[i, j] ??= new Pixel(255,255, 255);
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
                    ImageData[i, j] ??= new Pixel(0,0 ,0 );
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
            for (int i = 6 * _taillemodule + _contours; i < 7* _taillemodule + _contours; i++)
            {
                if ((j - _contours) / _taillemodule % 2 == 0)
                {
                    ImageData[i, j] = new Pixel(0, 0, 0);
                }
                else
                {
                    ImageData[i, j] = new Pixel(255,255,255);
                }
                
            }
        }
        
        
        
    }
    #endregion

    
    #region Dark Module
public void DarkModule()
    {
        for (int j = 8*_taillemodule + _contours; j < _taillemodule * 9 + _contours; j++)
        {
            for (int i = (4 * _version + 9)*_taillemodule +_contours; i < _taillemodule + (4 * _version + 9)*_taillemodule +_contours; i++)
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

        var coordonees = Coordonees(@"../../../Coordonées.txt",_version);
        
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
                    string [] ligne = lines.Split(" ");
                    coordonee = new int [ligne.Length - 1];
                    for (int j = 0; j < ligne.Length-1; j++)
                    {
                        coordonee[j] = (Convert.ToInt32(ligne[j + 1]))*_taillemodule + _contours;
                    }
                }

                i++;
            }
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(" Le fichier spécifié " + nomfichier + " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
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
            
        return donees.Where(x => x.Length ==2).ToArray();
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
            while((lines = flux.ReadLine()) != null)
            {
                if (i == _version - 7)
                {
                    string [] ligne = lines.Split(" ");
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
            Console.WriteLine(" Le fichier spécifié " + "InfosVersionQRCode.txt" + " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
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
    
    
    #region Format QRCode et Ecriture des Info du Format (a finir)
    

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
            while((lines = flux.ReadLine()) != null)
            {
                if (i == _mask + k)
                {
                    string [] ligne = lines.Split(" ");
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
            Console.WriteLine(" Le fichier spécifié " + "InfosFormatQRCode.txt" + " est introuvable, veuillez réessayer.\nMessage d'erreur : \n" + e.Message);
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
        
        int i = Height - _contours -_taillemodule;
        int j = 0 + _contours;
        foreach(int n in infoformat)
        {
            if (ImageData[i, colonne] != null) i -= _taillemodule;
            if (ImageData[ligne, j] != null) j += _taillemodule;

            for (int l = 0; l < _taillemodule; l++)
            {
                for (int c = 0; c < _taillemodule; c++)
                {
                    ImageData[ligne+l, j + c] = n == 0 ? new Pixel(255, 255, 255) : new Pixel(0, 0, 0);
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
                    ImageData[ligne+l,ImageData.GetLength(0) - 8 * _taillemodule - _contours+c] = ImageData[ligne +l,ImageData.GetLength(0) - 7 * _taillemodule - _contours+c];
                }
            }
            
            i -= _taillemodule;
            j += _taillemodule;
        }

        
        

    }
    
    
    #endregion


    #endregion

}
