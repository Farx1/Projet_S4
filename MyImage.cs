// ReSharper disable All

namespace Projet_S4
{
    public class MyImage
    {


        #region Attributs

        private string _typeImage="BMP";
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

        #region Deuxième constructeur

        public MyImage(string path)
        {
            byte[] myfile = File.ReadAllBytes(path);
            
            //if (myfile[0] == 66 && myfile[1] == 77)
            //{
                //this._typeImage ---> Initialisé directement dans le constructeur puique BMP ne change pasa
            //}

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
                        new Pixel(myfile[k], myfile[k + 1], myfile[k + 2]); //creation d'un pixel pour chaque set de 3 bytes du fichier
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

        public int Width
        {
            get => _width;
            set => _width = value;
        }

        public int SizeFile
        {
            get => _sizeFile;
            set => _sizeFile=value;
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
            set => _imageData = value;
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
                       "Largeur de l'image : " + Width + "\n" + "Taille du fichier : " + SizeFile + "\n" +
                       "Nombre de bits par couleur : " + NumberRgb + "\n" + "Taille du bandeau : " + Offset + "\n" +
                       "\n";
            if (ImageData != null)
            {
                s += "Données de l'image : " + "\n";
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
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
        //Méthode ok après check Debug
        public void From_Image_To_File( string path)
        {
            // creation de 3 lists, une pour chaque catégorie, où on y ajoute les données une à une 

            #region Header

            List<byte> header = new List<byte>(); //header
            if (this.TypeImage == "BMP")
            {
                header.Add(Convert.ToByte(66));
                header.Add(Convert.ToByte(77));
            }

            
            header.AddRange(Convertir_Int_To_Endian(this.SizeFile, 4));

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

            headerInfo.AddRange(Convertir_Int_To_Endian(this.Width, 4));

            headerInfo.AddRange(Convertir_Int_To_Endian(this.Height, 4));

            headerInfo.Add(1);
            headerInfo.Add(0);

            headerInfo.AddRange(Convertir_Int_To_Endian(this.NumberRgb, 2));


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
            for (int i = 0; i < this.Height; i++)
            {
                for (int j = 0; j < this.Width; j++)
                {
                    image.Add(this.ImageData[i, j].Red);
                    image.Add(this.ImageData[i, j].Green);
                    image.Add(this.ImageData[i, j].Blue);
                }

                for (int l = 0; l < this._imageData.GetLength(1)%4; l++)
                {
                    image.Add(Convert.ToByte(0));
                }
            }
             var output = header.Concat(headerInfo).Concat(image); //fusionne les 3 listes
             File.WriteAllBytes(path, output.ToArray());
             
            #endregion
            
           
        }

        #endregion


        #region Méthode Couleur --> Noir&Blanc/Inversion
        public void NuancesGris()//Revérifier si le Offset a une incidence sur la construction de la nouvelle image (normalement non)
        {
            Pixel [,] neb = new Pixel[this._height, this._width];
            
            int k = _offset;
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    byte moyenne = Convert.ToByte((this.ImageData[i,j].Red+ this.ImageData[i,j].Blue + this.ImageData[i,j].Green)/3) ;
                    neb[i, j] = new Pixel(moyenne, moyenne, moyenne);
                    k += 3;
                }
            }

            _imageData = neb;
        }
        
        public void Negatif()
        {
            Pixel [,] neg = new Pixel[this._height, this._width];

            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    neg[i, j] = new Pixel(Convert.ToByte(255-this._imageData[i,j].Red) , Convert.ToByte(255-this._imageData[i,j].Green), Convert.ToByte(255-this._imageData[i,j].Blue));

                }
            }

            _imageData = neg;
        }
        
        #endregion

        
        #region Méthode pour agrandir et retrecir
        //UPDATE: il faut qu'on complète la fonction, pourvoir agrandir de 1,3 est possible si on fait agrandir:x13 et rétécir:x10 par exemple
        public void Agrandir(double facteur)//Voir dans le dossier directement, l'affichage ne se fait pas sur Riders
        {

            

            _height = (int) (_height * facteur);
            _width = (int) (_width * facteur);
            
            Pixel[,] grand = new Pixel[_height,_width];
            
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    grand[i, j] = new Pixel (_imageData[(int)(i / facteur),(int) (j / facteur)]);
                }
            }
            _imageData = grand;

        }

        public void Retrecir(double facteur) //Voir dans le dossier directement, l'affichage ne se fait pas sur Riders
        {

            _height = (int) (_height / facteur);
            _width = (int) (_width / facteur);
            Pixel[,] petit = new Pixel[_height, _width];
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    try
                    {
                        petit[i, j] =
                            new Pixel(this._imageData[(int) (i * facteur), (int) (j * facteur)]); // problème est là

                    }
                    catch
                    {
                        petit[i, j] = new Pixel(this._imageData[(int) (i * facteur), (int) (j * facteur)]);

                    }
                }
            }

            _imageData = petit;
        }

        #endregion

        
        #region Méthodes mirroir(Horizontal/Vertical)
        public void MirroirHorizontal()
        {
            Pixel[,] mirh = new Pixel [_height, _width];
            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    mirh[i, j] = this._imageData[i,this._imageData.GetLength(1)-1- j];
                }
            }

            _imageData = mirh;
        }
        public void MirroirVertical()
        {
            Pixel[,] mirv = new Pixel [_height, _width];
            for (int i = 0; i < this._imageData.GetLength(0); i++)
            {
                for (int j = 0; j < this._imageData.GetLength(1); j++)
                {
                    mirv[i, j] = this._imageData[this._imageData.GetLength(0)-1- i,j];
                }
            }

            _imageData = mirv;
        }
        #endregion
        
       
        #region Méthodes pour la rotation d'une Image
       public void Rotate90(int degre)
       {
           //MyImage rot90 = new MyImage(this);
           //MyImage resul = new MyImage(this);
           
           while (degre < 0) degre += 360;
           while (degre >= 360) degre -= 360;

           if (degre % 180 != 0)
           {
               Pixel [,] rot = new Pixel[this._imageData.GetLength(1), this._imageData.GetLength(0)];
               _height = this._imageData.GetLength(1); 
               _width = this._imageData.GetLength(0);
               int k = degre / 90;
               if (k < 2)
               {
                   for (int i = 0; i < this._imageData.GetLength(0); i++)
                   {
                       for (int j = 0; j < this._imageData.GetLength(1); j++)
                       {
                           rot[j, this._imageData.GetLength(0) - 1 - i] = this._imageData[i, j];
                       }
                   }

                   _imageData = rot;
               }
               else
               {
                   this.Rotate90(90);
                   this.MirroirVertical();
                   this.MirroirHorizontal();
               }
           }
           else
           {
               Pixel[,] rot = new Pixel[this._imageData.GetLength(0), this._imageData.GetLength(1)];

               _height = this._imageData.GetLength(0);
               _width = this._imageData.GetLength(1);
               int k = degre / 180;
              
               if(k==1)
               {
                   for (int i = 0; i < this._imageData.GetLength(0); i++)
                   {
                       for (int j = 0; j < this._imageData.GetLength(1); j++)
                       {
                           rot[i, j] = this._imageData[this._imageData.GetLength(0) - 1 - i, this._imageData.GetLength(1) - 1 - j];
                       }
                   }

                   _imageData = rot;
               }
           }

       }
       
       
       public MyImage Rotate(int deg)
        {
            MyImage rot = new MyImage(this);

            while (deg < 0) deg += 360;
            while (deg >= 360) deg -= 360;

            int rotation = deg % 90;

            // On effectue d'abord des rotations de 90° avec une autre méthode plus simple
            this.Rotate90(deg-rotation);

            // On termine la rotation dans le cas où l'angle n'est pas un multiple de 90°
            if (rotation > 0)
            {
                // Angle de rotation en radians
                double rad = (double)rotation * (Math.PI / 180.0);

                // Calcul des donnés de la nouvelle taille de l'image
                
                rot._height = (int) (Math.Abs(Math.Sin(rad) * (double)this._imageData.GetLength(1)) + Math.Abs(Math.Cos(rad) * (double)this._imageData.GetLength(0)));
                rot._width = (int) (Math.Abs(Math.Cos(rad) * (double)this._imageData.GetLength(1)) + Math.Abs(Math.Sin(rad) * (double)this._imageData.GetLength(0)));
                rot._imageData = new Pixel[rot._height, rot._width];

                // Pour chaque pixel de la NOUVELLE image
                for (int i = 0; i < rot._height; i++)
                {
                    for (int j = 0; j < rot._width; j++)
                    {

                        // Calcul des coordonnées cartésiennes du point en question
                        double X = j;
                        double Y = (double) (rot._height - i) - (double)(Math.Sin(rad) * _imageData.GetLength(1));

                        // Mise en coordonnées polaires + Ajout de l'angle de rotation "rad"
                        double r = Math.Sqrt(X * X + Y * Y);
                        double ang = Math.Atan2(Y, X) + rad;

                        // Calcul des nouvelles coordonnées avec l'angle modifié
                        double x = r * Math.Cos(ang);
                        double y = r * Math.Sin(ang);

                        int I = (int)(this._imageData.GetLength(0) - y);
                        int J = (int)x;

                        if (I >= 0 && J >= 0 && I < this._imageData.GetLength(0) && J < this._imageData.GetLength(1))
                        {
                            //Console.WriteLine($"({I}, {J}) ==> ({i}, {j})");
                            rot._imageData[i, j] = this._imageData[I, J];
                        }
                    }
                }
            }

            rot.FillImageWithWhite();
            return rot;
        }

       public void FillImageWithWhite()
       {
           for (int i = 0; i < this._imageData.GetLength(0); i++)
           {
               for (int j = 0; j < this._imageData.GetLength(1); j++)
               {
                   _imageData[i, j] ??= new Pixel(255, 255, 255);
               }
           }
       }
       
       
        #endregion
        
       
        #region Matrice de Convolution  
       public void Convolution(int[,] kernel, double factor=1.000)
       {
           Pixel[,] pix = new Pixel[_height, _width];
           
           var midPoint = kernel.GetLength(0) / 2;

           for (int i = 0; i < _height; i++)
           {
               for (int j = 0; j < _width; j++)
               {
                   pix[i, j] = new Pixel(0, 0, 0);//creer trois somme de int pour chaque pixel
                   int cr = 0;
                   int cg = 0;
                   int cb = 0;
                   
                   for (int k = 0; k < kernel.GetLength(0); k++)
                   {
                       for (int l = 0 ; l < kernel.GetLength(0); l++)
                       {
                           var line = (i + (k - midPoint) + _imageData.GetLength(0)) % _imageData.GetLength(0);
                           var col = (j + (l - midPoint) + _imageData.GetLength(1)) % _imageData.GetLength(1);
                           
                           cr +=(byte) ((int)_imageData[line, col].Red * kernel[k, l]);//changer le premier en somme
                           cg +=(byte) ((int)_imageData[line, col].Green * kernel[k, l]);
                           cb +=(byte) ((int)_imageData[line, col].Blue * kernel[k, l]);


                       }
                   }

                   cr = (int) Math.Abs((double) factor * cr);//passer en int
                   cg = (int) Math.Abs((double) factor * cg);
                   cb = (int) Math.Abs((double) factor * cb);
                   
                   cr = (byte) (cr > 255 ? (byte) 255: cr);//passer en byte
                   cg = (byte) (cg > 255 ? (byte) 255: cg);
                   cb = (byte) (cb > 255 ? (byte) 255: cb);

                   pix[i, j] = new Pixel(Convert.ToByte(cr), Convert.ToByte(cg), Convert.ToByte(cb));
               }
           }

           this._imageData = pix;

       }

       public void DetectionSobel(int[,] sobel1, int[,] sobel2 ,double factor=1.000)
       {
           Pixel[,] pix = new Pixel[_height, _width];
           
           var midPoint = sobel1.GetLength(0) / 2;
           if (sobel1.GetLength(0) != sobel2.GetLength(0))
           {
               throw new ArgumentException("Vérifier la taille des matrices");
           }

           for (int i = 0; i < _height; i++)
           {
               for (int j = 0; j < _width; j++)
               {
                   pix[i, j] = new Pixel(0, 0, 0);//creer trois somme de int pour chaque pixel
                   int cr1 = 0;
                   int cg1 = 0;
                   int cb1 = 0;

                   int cr2 = 0;
                   int cg2 = 0;
                   int cb2 = 0;
                   
                   for (int k = 0; k < sobel1.GetLength(0); k++)
                   {
                       for (int l = 0 ; l < sobel1.GetLength(0); l++)
                       {
                           var line = (i + (k - midPoint) + _imageData.GetLength(0)) % _imageData.GetLength(0);
                           var col = (j + (l - midPoint) + _imageData.GetLength(1)) % _imageData.GetLength(1);
                           
                           cr1 += ((int)_imageData[line, col].Red * sobel1[k, l]);//changer le premier en somme
                           cg1 +=((int)_imageData[line, col].Green * sobel1[k, l]);
                           cb1 +=((int)_imageData[line, col].Blue * sobel1[k, l]);
                           
                           cr2 +=((int)_imageData[line, col].Red * sobel2[k, l]);//changer le premier en somme
                           cg2 += ((int)_imageData[line, col].Green * sobel2[k, l]);
                           cb2 += ((int)_imageData[line, col].Blue * sobel2[k, l]);


                       }
                   }

                   cr1 = (int) Math.Abs((double) factor * cr1);
                   cg1 = (int) Math.Abs((double) factor * cg1);
                   cb1 = (int) Math.Abs((double) factor * cb1);
                   
                   cr2 = (int) Math.Abs((double) factor * cr2);
                   cg2 = (int) Math.Abs((double) factor * cg2);
                   cb2 = (int) Math.Abs((double) factor * cb2);

                   var redValue = Math.Sqrt(cr1 * cr1 + cr2 * cr2);
                   var greenValue = Math.Sqrt(cg1 * cg1 + cg2 * cg2);
                   var blueValue = Math.Sqrt(cb1 * cb1 + cb2 * cb2);


                   pix[i, j] = new Pixel(Convert.ToByte(redValue > 255 ? 255 : redValue), Convert.ToByte(greenValue > 255 ? 255 : greenValue), Convert.ToByte(blueValue > 255 ? 255 : blueValue));
               }
           }

           this._imageData = pix;

       }//Effectue la détection de contour plus efficacement
       
       #endregion

       
       
       
    }
}

