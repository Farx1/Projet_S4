namespace Projet_S4
{
    class Program
    {
        static void Main(string[] args)
        
        {
          
            MyImage test = new MyImage("../../../Images/tunnel.bmp");
            MyImage test2 = new MyImage("../../../Images/coco.bmp");
            MyImage test3 = new MyImage("../../../Images/lac.bmp");
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
            
            /*
            for (int i = 90; i <=94; i++)
            {
                MyImage test1 = new MyImage("../../../Images/lac.bmp");
                var lol = test1.Rotate(i);//marche pour 47 -75 mais pas pour d'autres valeurs
                lol.From_Image_To_File($@"../../../Images/Test{i}.bmp");
                
            }
            */
            

            //var lol = test;
            //MyImage mirr= test.Rotate(5);
            //var lol = test.Negatif();//marche pour 47 -75 mais pas pour d'autres valeurs
            //mirr.DetectionSobel(Matrice.Sobel1,Matrice.Sobel2);//Flou(facteur= 0.11111)--Contour--Renforcement--Repoussage
            //mirr.From_Image_To_File(@"../../../Images/Test5.bmp");
            
            //MyImage mand = test;
            //MyImage rot =mand.Rotate(67);
            //rot.From_Image_To_File(@"../../../Images/Test4.bmp");
            //MyImage coco = test2;
            //mand.DrawMandelbrotB();
            //mand.CacherImage(test2); 
            //mand.From_Image_To_File(@"../../../Images/Test5.bmp");
            //MyImage code = new MyImage(@"../../../Images/Test5.bmp");
            //code.DecoderImage();

            //code.From_Image_To_File("../../../Images/Test6.bmp");
            MyImage gris = new MyImage("../../../Images/coco.bmp");
            gris.DrawHistogram();
            gris.From_Image_To_File("../../../Images/Test5.bmp");
            //MyImage zut =mand.Rotate(67);
            //zut.From_Image_To_File("../../../Images/Test6.bmp");
            //gris.From_Image_To_File("../../../Test5.bmp");

            // test3.ImageData = new Pixel[500, 500];
            // test3.Height = 500;
            // test3.Width = 500;
            // test3.FillImageWithWhite();
            // test3.From_Image_To_File("../../../Images/Test5.bmp");
            //gris.DrawHistogram();
            //gris.From_Image_To_File(@"../../../Images/Test5.bmp");
            
            /*
            for (int i = 1; i <= 40; i++)
            {
                QRCode qrcode = new QRCode(10,i,0,"0010");//rajouter le mask

            }
            */
            
            //QRCode test4 = new QRCode(1,7,0,"0010");
            


            //Finir QRCode, Finir corriger méthodes MyImage , Voir si on fait un affichage



        }
    }
}
 




