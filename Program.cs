﻿namespace Projet_S4
{
    class Program
    {
        static void Main(string[] args)
        {
          
            MyImage test = new MyImage("../../../Images/lac.bmp");
            /*
          byte[] myfile = File.ReadAllBytes("C:\\Users\\jules\\RiderProjects\\Projet S4\\Images\\Test.bmp");
          Console.WriteLine("\n Header \n");

          for (int i = 0; i < 14; i++)
          
          {
              Console.Write(myfile[i] + " ");
          }

          Console.WriteLine("\n HEADER INFOS \n");
          for (int i = 14; i < 54; i++)
          {
              Console.Write(myfile[i] + " ");
          }

          Console.WriteLine("\n IMAGE \n");
          for (int i = 54; i < myfile.Length; i=i+60)
          {
              for (int j = i; j < i + 60; j++)
              {
                  Console.Write(myfile[j] + " ");
              }

              Console.WriteLine();
          }
          Console.WriteLine("\n");
          
          //File.WriteAllBytes("C:\\Users\\jules\\RiderProjects\\Projet S4\\Images\\Test3.bmp",myfile);
          //Console.Write(test.toString());
          
          



          //MyImage.From_Image_To_File(test,"C:\\Users\\jules\\RiderProjects\\Projet_S4\\Images\\Test.bmp");
          */

            //MyImage mirr = test.Rotate90(180);
            
            //HEHE
            /*
            for (int i = 0; i <=360; i++)
            {
                MyImage test = new MyImage("../../../Images/coco.bmp");
                var lol = test.Rotate(i);//marche pour 47 -75 mais pas pour d'autres valeurs
                lol.From_Image_To_File($@"../../../Images/Test{i}.bmp");
                
            }
            */

            //MyImage mirr = test;
            //mirr.NuancesGris();
            //var lol = test;
            //MyImage mirr= test.Rotate(5);
            //var lol = test.Negatif();//marche pour 47 -75 mais pas pour d'autres valeurs
            //mirr.DetectionSobel(Matrice.Sobel1,Matrice.Sobel2);//Flou(facteur= 0.11111)--Contour--Renforcement--Repoussage
            //mirr.From_Image_To_File(@"../../../Images/Test5.bmp");
            MyImage mand = test;
            //mand.DrawMandelbrotA();
            mand.Convolution(Matrice.Contraste,Matrice.Null,3);
            mand.From_Image_To_File(@"../../../Images/Test5.bmp");


 


        }
    }
}
 




