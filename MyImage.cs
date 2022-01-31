namespace Projet_S4
{
    public class MyImage
    {

        
        #region Attributs
        private string _typeImage = null!;
        private int _height;
        private int _weight;
        private int _sizeFile;
        private int _numberRgb;
        private int _offset;
        private Pixel[,] _imageData;
        #endregion
        

        #region Les 2 constructeurs
        /// <summary>
        /// Constructeur naturel avec paramètres saisis manuellement
        /// </summary>
        /// <param name="typeImage"> type de l'image </param>
        /// <param name="height"> hauteur de l'image </param>
        /// <param name="weight"> largeur de l'image </param>
        /// <param name="size"> taille du fichier </param>
        /// <param name="numberRgb"> nombre de bits par couleur </param>
        /// <param name="offset"> taille du header + headerinfo </param>
        /// <param name="imageData"> matrice RGB de l'image elle-mêle </param>

        #region Premier constructeur
        public MyImage(string typeImage, int height, int weight, int size, int numberRgb, int offset, Pixel[,] imageData)
                {
                    this._typeImage = typeImage;
                    this._height = height;
                    this._weight = weight;
                    this._sizeFile = size;
                    _numberRgb = numberRgb;
                    this._offset = offset;
                    _imageData = imageData;
                }
        

        #endregion
        
        
        /// <summary>
        /// Constructeur prenant en entrée un fichier
        /// </summary>
        /// <param name="filename"> Un fichier.bmp quelconque que l'on ajoutera pour être lu </param>

        #region Deuxième constructeur
        public MyImage(string filename)
        {
            byte[] myfile = File.ReadAllBytes(filename);
            
            if (myfile[0] == 66 && myfile[1] == 77)
            {
                this._typeImage = "BMP";
            }
            
            byte[] tabLargeur = new byte[] {myfile[18], myfile[19], myfile[20], myfile[21]};
            this._weight = Convertir_Endian_To_Int(tabLargeur);
        
            byte[] tabHauteur = new byte[] {myfile[22], myfile[23], myfile[24], myfile[25]};
            this._height = Convertir_Endian_To_Int(tabHauteur);
        
            byte[] tabTaille = new byte[] {myfile[2], myfile[3], myfile[4], myfile[5]};
            this._sizeFile= Convertir_Endian_To_Int(tabTaille);
        
            byte[] tabBits = new byte[] {myfile[28], myfile[29]};
            this._numberRgb = Convertir_Endian_To_Int(tabBits);
        
            byte[] tabOffset = new byte[] {myfile[10], myfile[11], myfile[12], myfile[13]};
            this._offset = Convertir_Endian_To_Int(tabOffset);

            this._imageData = new Pixel[_height, _weight];
            int k = _offset;
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _weight; j++)
                {
                    this._imageData[i, j] = new Pixel(myfile[k],myfile[k + 1],myfile[k + 2]); //creation d'un pixel pour chaque set de 3 bytes du fichier
                    k += 3;
                }
                /*
                int reste = _weight % 4;
                if (reste == 1)
                {
                    k = k + 3;
                } 
                if (reste == 2)
                {
                    k = k + 2;
                }

                if (reste == 3)
                {
                    k = k + 1;
                }*/
               
            }
            
        }
        

        #endregion
        

        #endregion


        #region Propriétés
        public string TypeImage
        {
            get => _typeImage;
            set => _typeImage = value;
        }

        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public int Weight
        {
            get => _weight;
            set => _weight = value;
        }

        public int SizeFile
        {
            get => _sizeFile;
            set => _sizeFile = value;
        }

        public int NumberRgb
        {
            get => _numberRgb;
            set => _numberRgb = value;
        }

        public int Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public Pixel[,] ImageData
        {
            get => _imageData;
            set => _imageData = value ?? throw new ArgumentNullException(nameof(value));
        }
        

        #endregion
        
        
        #region Méthodes de conversion

        #region Endian --> Entier
        public static int Convertir_Endian_To_Int(byte[] tab)
        {
            int result = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                result = (int) (result + tab[i] * Math.Pow(256, i));
            }

            return result;
        }
        

        #endregion

        #region Entier --> Endian 
        public static byte[] Convertir_Int_To_Endian(int val,int size)
        {
            byte[] newone = new byte[size];
            for (int i = size - 1 ; i >= 0 ; i--)
            {
                newone[i] = (byte) (val / Math.Pow(256, i));
                val -= newone[i] * ((int) Math.Pow(256, i));
            }

            return newone;
        }
        

        #endregion

        #endregion
        
        
        #region Méthode pour afficher les caractéristiques d'une image

        public string toString()
        {
            string s = "Type de l'image : " + TypeImage + "\n" + "Hauteur de l'image : " + Height + "\n" +
                       "Largeur de l'image : " + Weight + "\n" + "Taille du fichier : " + SizeFile+ "\n" + 
                       "Nombre de bits par couleur : " +NumberRgb+ "\n" + "Taille du bandeau : " + Offset+"\n"+"\n";
            if (ImageData != null)
            {
                s += "Données de l'image : " + "\n";
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Weight; j++)
                    {
                        s += ImageData[i, j].toString();
                        
                    }
                    s += "\n";
                }  
            }
                 
            return s;
        }

        #endregion
        

        #region Méthode qui tranforme une image en fichier binaire 
        public static void From_Image_To_File(MyImage im, string path)
        {
            // creation de 3 lists, une pour chaque catégorie, où on y ajoute les données une à une 
            #region Header
            List<byte> header = new List<byte>();//header
            if (im.TypeImage == "BMP")
            {
                header.Add(Convert.ToByte(66));
                header.Add(Convert.ToByte(77));
            }
            
            header.AddRange(Convertir_Int_To_Endian(im.SizeFile,4));
            
            for (int i = 0; i < 4; i++)
            {
                header.Add(Convert.ToByte(0));
            }
            header.Add(Convert.ToByte(54));
            for (int i = 0; i < 3; i++)
            {
                header.Add(Convert.ToByte(0));
            }
            #endregion

            #region HeaderInfo
            List<byte> headerInfo = new List<byte>();//HeaderInfos

            headerInfo.Add(40);

            for (int i = 1; i < 4; i++)
            {
                headerInfo.Add( 0);
            }
            
            headerInfo.AddRange(Convertir_Int_To_Endian(im.Weight,4));
            
            headerInfo.AddRange(Convertir_Int_To_Endian(im.Height,4));

            headerInfo.Add(1);
            headerInfo.Add(0);
            
            headerInfo.AddRange(Convertir_Int_To_Endian(im.NumberRgb,2));
            

            for (int i = 0; i < 4; i++)
            {
                headerInfo.Add(0);
            }
            

            headerInfo.Add(176);
            headerInfo.Add(4);
            
            for (int i = 0; i < 18; i++)
            {
                headerInfo.Add(0);
            }
            #endregion

            #region Image
            List <byte> image = new List<byte>();//Image
            for (int i = 0; i < im.Height; i++)
            {
                for (int j = 0; j < im.Weight; j++)
                {
                    image.Add(im.ImageData[i,j].Red);
                    image.Add(im.ImageData[i,j].Green); 
                    image.Add(im.ImageData[i,j].Blue);
                }
            }
            #endregion
            
            #region Affichage du fichier binaire
            
            string s = "Header"+"\n"+"\n";
            for (int i = 0; i < header.Count; i++)
            {
                s = s + header[i]+" ";
            }
            
            s += "\n" + "HEADER INFOS"+"\n"+"\n";
            
            for (int i = 0; i < headerInfo.Count; i++)
            {
                s += headerInfo[i] + " ";
            }

            s += "\n" + "IMAGE"+"\n"+"\n";


            int taille_ligne = im.Weight * 3; // car 1 Pixel = 3 bytes
            for (int j = 0; j < image.Count; j++)
            {
                s += image[j] + " ";
                if (j!=0 && (j+1)% (taille_ligne) == 0) //retour à la ligne lorsque j est un multiple d'une taille de ligne
                {
                    s += "\n";
                }
            }
            Console.Write(s);

            #endregion
        }
        #endregion
        
        
    }   
}

