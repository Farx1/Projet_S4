namespace Projet_S4
{
    public class MyImage
    {

        private string _typeImage = null!;
        private int _height;
        private int _weight;
        private int _sizeFile;
        private int _numberRgb;
        private int _offset;
        private Pixel[,] _imageData;

        
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
                    this._imageData[i, j] = new Pixel(myfile[k],myfile[k + 1],myfile[k + 2]);
                    k += 3;
                }

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
               
            }
            
        }
        
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

        public  string toString()
        {
            string s = TypeImage+" "+Height+" "+Weight+" "+SizeFile+" "+NumberRgb+" "+Offset+"\n";
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Weight; j++)
                {
                    s = s + ImageData[i, j].toString();
                    
                }

                s += "\n";
            }   
            return s;
        }
  
        public void From_Image_To_File(MyImage Image, string path)
        {
            List<byte> header = new List<byte>();
            if (Image._typeImage == "BMP")
            {
                header.Add(66);
                header.Add(77);
            }
            
            header.AddRange(Convertir_Int_To_Endian(Image.SizeFile));
            
            
            for (int i = 0; i < 4; i++)
            {
                header.Add(0);
            }
            
            header.AddRange(Convertir_Int_To_Endian(Image.Offset));
            
            
            List<byte> headerInfo = new List<byte>();

            headerInfo.Add(40);

            for (int i = 1; i < 4; i++)
            {
                headerInfo.Add( 0);
            }
            
            headerInfo.AddRange(Convertir_Int_To_Endian(Image.Weight));
            
            headerInfo.AddRange(Convertir_Int_To_Endian(Image.Height));

            headerInfo.Add(1);
            headerInfo.Add(0);
            
            headerInfo.AddRange(Convertir_Int_To_Endian(Image.NumberRgb));
            

            for (int i = 0; i < 4; i++)
            {
                headerInfo.Add(0);
            }

            headerInfo.Add(176);
            headerInfo.Add(4);
            
            for (int i = 0; i < 17; i++)
            {
                headerInfo.Add(0);
            } 
            
            
            
            
        }
        
    }   
}

