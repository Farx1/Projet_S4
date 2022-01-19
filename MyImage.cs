namespace Projet_S4
{
    public class MyImage
    {

        private string _type;
        private int _height;
        private int _weight;
        private int _size;
        private int _numberRgb;
        private int _offset;
        private Pixel[] _imageData;

        
        public MyImage(string type, int height, int weight, int size, int numberRgb, int offset, Pixel[] imageData)
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

        public Pixel[] ImageData
        {
            get => _imageData;
            set => _imageData = value ?? throw new ArgumentNullException(nameof(value));
        }
    }   
}

