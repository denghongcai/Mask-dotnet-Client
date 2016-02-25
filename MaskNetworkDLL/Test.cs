namespace MaskGame.Queue
{
    public class Test
    {
        public int c;

        public void AddValue(int a, int b)
        {
            c = a + b;
        }

        public static int GenerateRandom(int min, int max)
        {
            System.Random rand = new System.Random();
            return rand.Next(min, max);
        }
    }
}
