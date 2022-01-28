namespace Projet_S4
{
    class Program
    {
        static void Main(string[] args)
        {
            MyImage test = new MyImage("../../../Images/Test.bmp");
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
        }
    }
}




