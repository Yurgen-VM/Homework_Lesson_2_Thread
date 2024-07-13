namespace Task_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Server.AcceptMess();
            }
            else
            {
                Client.SendtMess($"Yura");
            }

            Console.ReadLine();

        }
    }
}
