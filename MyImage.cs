namespace Projet_S4
{
    public class MyImagefichier
    {

        private string _type;
        private int _height;
        private int _weight;
        private int _size;
        private int _numberRgb;
        private int _offset;
        private Pixel[,] _imageData;

        
        public MyImagefichier(string type, int height, int weight, int size, int numberRgb, int offset, Pixel[,] imageData)
        {
            this._type = type;
            this._height = height;
            this._weight = weight;
            this._size = size;
            _numberRgb = numberRgb;
            this._offset = offset;
            _imageData = imageData;
        }
        
        
        public string Type
        {
            get => _type;
            set => _type = value;
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

        public int Size
        {
            get => _size;
            set => _size = value;
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
        
        public MyImagefichier(string pov)
        {
            byte[] myfile = File.ReadAllBytes(pov);

            string type = "";
            if (myfile[0] == 66 && myfile[1] == 77)
            {
                type = "BMP";
            }
            byte[] bMp = new byte[] {myfile[0], myfile[1]};
        
            byte[] tabLargeur = new byte[] {myfile[18], myfile[19], myfile[20], myfile[21]};
            int largeur = Convertir_Endian_To_Int(tabLargeur);
        
            byte[] tabHauteur = new byte[] {myfile[22], myfile[23], myfile[24], myfile[25]};
            int hauteur = Convertir_Endian_To_Int(tabHauteur);
        
            byte[] tabTaille = new byte[] {myfile[2], myfile[3], myfile[4], myfile[5]};
            int taille = Convertir_Endian_To_Int(tabTaille);
        
            byte[] tabBits = new byte[] {myfile[28], myfile[29]};
            int bitsParCouleur = Convertir_Endian_To_Int(tabBits);
        
            byte[] tabOffset = new byte[] {myfile[10], myfile[11], myfile[12], myfile[13]};
            int offset = Convertir_Endian_To_Int(tabOffset);
        
            Pixel[] data = new Pixel[] { };
            for (int i = offset; i < myfile.Length; i+=3)
            {
                if (pov != null) data[i - offset] = fromByteToPixel(myfile[i], myfile[i + 1], myfile[i + 2]);

            }

            var image = new MyImagefichier(type, hauteur, largeur, taille, bitsParCouleur, offset,data);
            
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
    }   
}

