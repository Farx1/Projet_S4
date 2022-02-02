namespace Projet_S4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("ENTREZ UN FICHIER A COPIER:"+"\n"+"\n"+"( coco , lac , Test )");
            string? testfinal= Console.ReadLine();
            Console.ReadKey();
            
            
            MyImage test = new MyImage($"../../../Images/{testfinal}.bmp");
            byte[] myfile = File.ReadAllBytes($"../../../Images/{testfinal}.bmp");
            /*
            #region Affichage du fichier d'origine en binaire
            
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
            for (int i = 54; i < myfile.Length; i = i + 60)
            {
                for (int j = i; j < i + 60; j++)
                {
                    Console.Write(myfile[j] + " ");
                }

                Console.WriteLine();
            }
            
            Console.WriteLine("\n");
            
            
            #endregion
            */
            
            //File.WriteAllBytes($"../../../Images/{testfinal}2.bmp", myfile);
            //Console.Write(test.toString());





            //MyImage.From_Image_To_File(test, $"../../../Images/{testfinal}.bmp");

            MyImage.NuancesGris(test,$"../../../Images/{testfinal}.bmp");
            Console.Write(test.toString());
        }
    }
}




