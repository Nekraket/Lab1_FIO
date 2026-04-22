class Program
{
    static void Main()
    {
        Console.Write("Сторона A: ");
        string a = Console.ReadLine();
        Console.Write("Сторона B: ");
        string b = Console.ReadLine();
        Console.Write("Сторона C: ");
        string c = Console.ReadLine();

        int[,] coords = new int[3, 2];
        string type = CalculateTriangle(a, b, c, coords);

        Console.WriteLine("\nТип: " + type);
        Console.WriteLine("Координаты: (" + coords[0, 0] + "," + coords[0, 1] + "), (" + coords[1, 0] + "," + coords[1, 1] + "), (" + coords[2, 0] + "," + coords[2, 1] + ")");
    }

    static string CalculateTriangle(string strA, string strB, string strC, int[,] coords)
    {
        try
        {
            float a, b, c;

            bool aOk = float.TryParse(strA, out a);
            bool bOk = float.TryParse(strB, out b);
            bool cOk = float.TryParse(strC, out c);

            if (!aOk || !bOk || !cOk || a <= 0 || b <= 0 || c <= 0)
            {
                coords[0, 0] = -2; coords[0, 1] = -2;
                coords[1, 0] = -2; coords[1, 1] = -2;
                coords[2, 0] = -2; coords[2, 1] = -2;
                Log("НЕУСПЕХ", strA + "," + strB + "," + strC, "Нечисловые данные", coords);
                return "";
            }

            if (a + b <= c || a + c <= b || b + c <= a)
            {
                coords[0, 0] = -1; coords[0, 1] = -1;
                coords[1, 0] = -1; coords[1, 1] = -1;
                coords[2, 0] = -1; coords[2, 1] = -1;
                Log("НЕУСПЕХ", strA + "," + strB + "," + strC, "не треугольник", coords);
                return "не треугольник";
            }

            string type;
            if (Math.Abs(a - b) < 0.001f && Math.Abs(b - c) < 0.001f)
                type = "равносторонний";
            else if (Math.Abs(a - b) < 0.001f || Math.Abs(a - c) < 0.001f || Math.Abs(b - c) < 0.001f)
                type = "равнобедренный";
            else
                type = "разносторонний";

            float xA = 0;
            float yA = 0;
            float xB = c;
            float yB = 0;
            float xC = (b * b + c * c - a * a) / (2 * c);
            float yC = (float)Math.Sqrt(b * b - xC * xC);

            float minX = Math.Min(0, Math.Min(xB, xC));
            float minY = Math.Min(0, Math.Min(yB, yC));
            float maxX = Math.Max(0, Math.Max(xB, xC));
            float maxY = Math.Max(0, Math.Max(yB, yC));

            float scaleX = 90 / (maxX - minX);
            float scaleY = 90 / (maxY - minY);
            float scale = scaleX;
            if (scaleY < scaleX) scale = scaleY;

            coords[0, 0] = (int)Math.Round(5 + (xA - minX) * scale);
            coords[0, 1] = (int)Math.Round(5 + (yA - minY) * scale);
            coords[1, 0] = (int)Math.Round(5 + (xB - minX) * scale);
            coords[1, 1] = (int)Math.Round(5 + (yB - minY) * scale);
            coords[2, 0] = (int)Math.Round(5 + (xC - minX) * scale);
            coords[2, 1] = (int)Math.Round(5 + (yC - minY) * scale);

            Log("УСПЕХ", strA + "," + strB + "," + strC, type, coords);

            return type;
        }
        catch (Exception ex)
        {
            coords[0, 0] = -2; coords[0, 1] = -2;
            coords[1, 0] = -2; coords[1, 1] = -2;
            coords[2, 0] = -2; coords[2, 1] = -2;
            Log("НЕУСПЕХ", strA + "," + strB + "," + strC, "Ошибка: " + ex.ToString(), coords);
            return "";
        }
    }

    static void Log(string status, string input, string result, int[,] coords)
    {
        string coordsText = "(" + coords[0, 0] + "," + coords[0, 1] + ") " +
                            "(" + coords[1, 0] + "," + coords[1, 1] + ") " +
                            "(" + coords[2, 0] + "," + coords[2, 1] + ")";

        string log = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + status +
                     " | Вход: " + input +
                     " | Результат: " + result + " " + coordsText;

        Console.WriteLine(log);
        File.AppendAllText("log.txt", log + "\r\n");
    }
}