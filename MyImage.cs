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
        private byte[] perroquet = File.ReadAllBytes("Images/coco.bmp");

        public void Propriété()
        {
            if (perroquet[0] == 66)
            {

            }
        }
    }   
}

