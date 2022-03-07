namespace Projet_S4
{
    class Program
    {
        static void Main(string[] args)
        {
          
            MyImage test = new MyImage("../../../Images/chemin.bmp");
            MyImage test2 = new MyImage("../../../Images/coco.bmp");
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
            for (int i = 0; i <=360; i++)
            {
                MyImage test = new MyImage("../../../Images/coco.bmp");
                var lol = test.Rotate(i);//marche pour 47 -75 mais pas pour d'autres valeurs
                lol.From_Image_To_File($@"../../../Images/Test{i}.bmp");
                
            }
            */ 

            //var lol = test;
            //MyImage mirr= test.Rotate(5);
            //var lol = test.Negatif();//marche pour 47 -75 mais pas pour d'autres valeurs
            //mirr.DetectionSobel(Matrice.Sobel1,Matrice.Sobel2);//Flou(facteur= 0.11111)--Contour--Renforcement--Repoussage
            //mirr.From_Image_To_File(@"../../../Images/Test5.bmp");
            
            //MyImage mand = test;
            //MyImage coco = test2;
            //mand.DrawMandelbrotA();
            //mand.CacherImage(coco);
            //mand.From_Image_To_File(@"../../../Images/Test5.bmp");
            //MyImage code = new MyImage("../../../Images/Test5.bmp");
            //code.DecoderImage();

            //code.From_Image_To_File("../../../Images/Test6.bmp");
            MyImage gris = new MyImage("../../../Images/lac.bmp");
            //mand.Rotate(180);
            //mand.From_Image_To_File("../../../Images/Test6.bmp");
            gris.NuancesGris();

            //QRCode cat = new QRCode("bonsoir");//ne marche pas pour un input<=4

        }
    }
}
 




