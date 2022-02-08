using System.Diagnostics.CodeAnalysis;

namespace Projet_S4
{
    public class MyImage
    {


        #region Attributs

        private string _typeImage;
        private int _height;
        private int _width;
        private int _sizeFile;
        private int _numberRgb;
        private int _offset;
        private Pixel[,] _imageData;

        #endregion


        #region Les 4 constructeurs

        /// <summary>
        /// Constructeur naturel avec paramètres saisis manuellement
        /// </summary>
        /// <param name="typeImage"> type de l'image </param>
        /// <param name="height"> hauteur de l'image </param>
        /// <param name="width"> largeur de l'image </param>
        /// <param name="size"> taille du fichier </param>
        /// <param name="numberRgb"> nombre de bits par couleur </param>
        /// <param name="offset"> taille du header + headerinfo </param>
        /// <param name="imageData"> matrice RGB de l'image elle-même </param>

        #region Premier constructeur

        public MyImage(string typeImage, int height, int width, int size, int numberRgb, int offset,
            Pixel[,] imageData)
        {
            this._typeImage = typeImage;
            this._height = height;
            this._width = width;
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

        public MyImage(string path)
        {
            byte[] myfile = File.ReadAllBytes(path);

            if (myfile[0] == 66 && myfile[1] == 77)
            {
                this._typeImage = "BMP";
            }

            byte[] tabLargeur = new byte[] {myfile[18], myfile[19], myfile[20], myfile[21]};
            this._width = Convertir_Endian_To_Int(tabLargeur);

            byte[] tabHauteur = new byte[] {myfile[22], myfile[23], myfile[24], myfile[25]};
            this._height = Convertir_Endian_To_Int(tabHauteur);

            byte[] tabTaille = new byte[] {myfile[2], myfile[3], myfile[4], myfile[5]};
            this._sizeFile = Convertir_Endian_To_Int(tabTaille);

            byte[] tabBits = new byte[] {myfile[28], myfile[29]};
            this._numberRgb = Convertir_Endian_To_Int(tabBits);

            byte[] tabOffset = new byte[] {myfile[10], myfile[11], myfile[12], myfile[13]};
            this._offset = Convertir_Endian_To_Int(tabOffset);

            this._imageData = new Pixel[_height, _width];
            int k = _offset;
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    this._imageData[i, j] =
                        new Pixel(myfile[k], myfile[k + 1],
                            myfile[k + 2]); //creation d'un pixel pour chaque set de 3 bytes du fichier
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
                }
                */
            }

        }



        #endregion

        #region Constructeur null

        #endregion

        #region Constructeur clone

        public MyImage(MyImage image)
        {
            this._typeImage = image._typeImage;
            this._height = image._height;
            this._width = image._width;
            this._sizeFile = image._sizeFile;
            _numberRgb = image._numberRgb;
            this._offset = image._offset;
            _imageData = image._imageData;
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
            get => _width;
            set => _width = value;
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

        public static byte[] Convertir_Int_To_Endian(int val, int size)
        {
            byte[] newone = new byte[size];
            for (int i = size - 1; i >= 0; i--)
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
                       "Largeur de l'image : " + Weight + "\n" + "Taille du fichier : " + SizeFile + "\n" +
                       "Nombre de bits par couleur : " + NumberRgb + "\n" + "Taille du bandeau : " + Offset + "\n" +
                       "\n";
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

        public void From_Image_To_File(string path)
        {
            // creation de 3 lists, une pour chaque catégorie, où on y ajoute les données une à une 

            #region Header

            List<byte> header = new List<byte>(); //header
            MyImage im = this;
            if (im.TypeImage == "BMP")
            {
                header.Add(Convert.ToByte(66));
                header.Add(Convert.ToByte(77));
            }


            header.AddRange(Convertir_Int_To_Endian(im.SizeFile, 4));

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

            List<byte> headerInfo = new List<byte>(); //HeaderInfos

            headerInfo.Add(40);

            for (int i = 1; i < 4; i++)
            {
                headerInfo.Add(0);
            }

            headerInfo.AddRange(Convertir_Int_To_Endian(im.Weight, 4));

            headerInfo.AddRange(Convertir_Int_To_Endian(im.Height, 4));

            headerInfo.Add(1);
            headerInfo.Add(0);

            headerInfo.AddRange(Convertir_Int_To_Endian(im.NumberRgb, 2));


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

            List<byte> image = new List<byte>(); //Image
            for (int i = 0; i < im.Height; i++)
            {
                for (int j = 0; j < im.Weight; j++)
                {
                    image.Add(im.ImageData[i, j].Red);
                    image.Add(im.ImageData[i, j].Green);
                    image.Add(im.ImageData[i, j].Blue);
                }
            }

            #endregion

            var output = header.Concat(headerInfo).Concat(image); //fusionne les 3 listes
            File.WriteAllBytes(path, output.ToArray());
        }

        #endregion


        #region Méthode Couleur --> Noir&Blanc

        public MyImage
            NuancesGris() //Revérifier si le Offset a une incidence sur la construction de la nouvelle image (normalement non)
        {
            MyImage neb = new MyImage(this);


            neb._imageData = new Pixel[this._height, this._width];

            int k = _offset;
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    byte moyenne = Convert.ToByte((this.ImageData[i, j].Red + this.ImageData[i, j].Blue +
                                                   this.ImageData[i, j].Green) / 3);
                    neb._imageData[i, j] =
                        new Pixel(moyenne, moyenne, moyenne);
                    k += 3;
                }
            }

            return neb;
        }


        #endregion


        #region Méthode pour agrandir et retrecir

        //UPDATE: il faut qu'on complète la fonction, pourvoir agrandir de 1,3 est possible si on fait agrandir:x13 et rétécir:x10 par exemple
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: Projet_S4.Pixel")]
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: Projet_S4.Pixel[,]")]
        public MyImage
            AgrandirRetrecir(double facteur) //Voir dans le dossier directement, l'affichage ne se fait pas sur Riders
        {
            double count = 0;

            if (facteur % 1 != 0)
            {
                count = 0;
                while (facteur % 1 != 0)
                {
                    facteur = facteur * 10;
                    count++;
                }

                count = Math.Pow(10,Convert.ToDouble(count));
            }
            int fact = Convert.ToInt32(facteur);
            
            Console.WriteLine("Souhaitez vous agrandir('a') ou retrecir ('r') l'image?");
            string reponse = Console.ReadLine();
            
            MyImage nvlImage = new MyImage(this);
            if ((reponse == "a" || reponse == "r") && facteur > 0)
            {
                if (reponse == "a")
                {
                    nvlImage._height *= fact;
                    nvlImage._width *= fact;
                    nvlImage._imageData = new Pixel[this._imageData.GetLength(0) * fact, this._imageData.GetLength(1) * fact];
                    for (int i = 0; i < nvlImage._imageData.GetLength(0); i++)
                    {
                        for (int j = 0; j < nvlImage._imageData.GetLength(1); j++)
                        {
                            nvlImage._imageData[i, j] = new Pixel(this._imageData[i / fact, j / fact]);
                        }
                    }

                    
                    nvlImage._height /= Convert.ToInt32(count);
                    nvlImage._width /= Convert.ToInt32(count);;
                    nvlImage._imageData = new Pixel[(int) (this._imageData.GetLength(0) / count), (int) (this._imageData.GetLength(1) / count)];
                    for (int i = 0; i < nvlImage._imageData.GetLength(0); i++)
                    {
                        for (int j = 0; j < nvlImage._imageData.GetLength(1); j++)
                        {
                            nvlImage._imageData[i, j] = new Pixel(this._imageData[(int) (i * count), (int) (j * count)]);
                        }
                    }
                }

                if (reponse == "r")
                {
                    nvlImage._height /= fact;
                    nvlImage._width /= fact;
                    nvlImage._imageData = new Pixel[this._imageData.GetLength(0) / fact, this._imageData.GetLength(1) / fact];
                    for (int i = 0; i < nvlImage._imageData.GetLength(0); i++)
                    {
                        for (int j = 0; j < nvlImage._imageData.GetLength(1); j++)
                        {
                            nvlImage._imageData[i, j] = new Pixel(this._imageData[i * fact, j * fact]);
                        }
                    }
                    
                    nvlImage._height *= Convert.ToInt32(count);
                    nvlImage._width *= Convert.ToInt32(count);
                    nvlImage._imageData = new Pixel[(int) (this._imageData.GetLength(0) * count), (int) (this._imageData.GetLength(1) * count)];
                    for (int i = 0; i < nvlImage._imageData.GetLength(0); i++)
                    {
                        for (int j = 0; j < nvlImage._imageData.GetLength(1); j++)
                        {
                            nvlImage._imageData[i, j] = new Pixel(this._imageData[(int) (i / count), (int) (j / count)]);
                        }
                    }
                }
            }
            else
            {
                Console.Write("Error: Invalid");
            }

            return nvlImage;
        }





        #endregion

        
        #region Méthodes mirroir(Horizontal/Vertical)
        public MyImage MirroirHorizontal()
        {
            MyImage mir = new MyImage(this);
            mir._imageData = new Pixel[this._height, this._width];
            for (int i = 0; i < mir._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < mir._imageData.GetLength(1); j++)
                {
                    mir._imageData[i, j] = this._imageData[i,this._imageData.GetLength(1)-1- j];
                }
            }
            return mir;
        }
        
        
        public MyImage MirroirVertical()
        {
            MyImage mir = new MyImage(this);
            mir._imageData = new Pixel[this._height, this._width];
            for (int i = 0; i < mir._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < mir._imageData.GetLength(1); j++)
                {
                    mir._imageData[i, j] = this._imageData[this._imageData.GetLength(0)-1- i,j];
                }
            }
            return mir;
        }
       #endregion
       
    }
}

