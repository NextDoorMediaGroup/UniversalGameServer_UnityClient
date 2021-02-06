namespace ndmg.UniversalGameServer
{
    public class Utility
    {
        public static string[] UnpackMessage(string data)
        {
            string[] d = {"", ""};
            string[] parts = data.Split(' ');
            if (parts.Length > 1)
            {
                d[0] = parts[0];

                d[1] = data.Replace(parts[0] + ' ', "");
            }
            else
            {
                d[0] = data;
            }

            return d;
        }
    }
}