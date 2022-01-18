using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Media;
using System.ComponentModel;
using System.Diagnostics;

namespace Projet
{
    public class MyImage
    {

        private int type;
        private int height;
        private int weight;
        private int size;
        private int numberRGB;
        private int offset;
        private Pixel[] _imageData;

        
        public MyImage(int type, int height, int weight, int size, int numberRgb, int offset, Pixel[] imageData)
        {
            this.type = type;
            this.height = height;
            this.weight = weight;
            this.size = size;
            numberRGB = numberRgb;
            this.offset = offset;
            _imageData = imageData;
        }
        
        
        public int Type
        {
            get => type;
            set => type = value;
        }

        public int Height
        {
            get => height;
            set => height = value;
        }

        public int Weight
        {
            get => weight;
            set => weight = value;
        }

        public int Size
        {
            get => size;
            set => size = value;
        }

        public int NumberRgb
        {
            get => numberRGB;
            set => numberRGB = value;
        }

        public int Offset
        {
            get => offset;
            set => offset = value;
        }

        public Pixel[] ImageData
        {
            get => _imageData;
            set => _imageData = value ?? throw new ArgumentNullException(nameof(value));
        }

        public MyImage(string myfile)
        {
            
        }

        public void From_Image_To_File(string file)
        {
            
        }

        public int Convertir_Endian_To_Int(byte[] tab )
        {
            
        }

        public byte[] Convertir_Int_To_Endian(int val )
        {
            
        }
    }   
}

