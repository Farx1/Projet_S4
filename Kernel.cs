namespace Projet_S4;

public static class Kernel
{
    public static int[,] Flou
    {
        get
        {
            return new int[,]
            {
                {1,1,1}, {1,1,1}, {1,1,1}
            };
        }
    }
    
    public static int[,] Contour
    {
        get
        {
            return new int[,]
            {
                {0,1,0}, {1,-4,1}, {0,1,0}
            };
        }
    }
    public static int[,] Renforcement
    {
        get
        {
            return new int[,]
            {
                {0,0,0}, {-1,1,0}, {0,0,0}
            };
        }
    }
    public static int[,] Repoussage
    {
        get
        {
            return new int[,]
            {
                {-2,-1,0}, {-1,1,1}, {0,1,2}
            };
        }
    }
    
}