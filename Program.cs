namespace Projet_S4
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] myfile = File.ReadAllBytes("Images/coco.bmp");
            Console.WriteLine("\n Header \n");

            for (int i = 0; i < 14; i++)
            {
                Console.Write(myfile[i] + " ");
            }

            Console.WriteLine("\n HEADER INFOS \n");
            for (int i = 14; i < 54; i++)
            {
                Console.WriteLine(myfile[i] + " ");
            }

            Console.WriteLine("\n IMAGE \n");
            for (int i = 0; i < myfile.Length; i++)
            {
                for (int j = 0; j < i + 60; j++)
                {
                    Console.WriteLine(myfile[j] + " ");
                }

                Console.WriteLine();
            }
            File.WriteAllBytes("Images/coco.bmp",myfile);

        }
    }
}




