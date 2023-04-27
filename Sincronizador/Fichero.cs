namespace Sincronizador
{
    public class Fichero
    {
        public static void CheckDirectorio()
        {
            try
            {
                // Verificamos si existe
                if (Directory.Exists("C:\\MileSync") == false)
                {
                    // Creamos
                    Directory.CreateDirectory("C:\\MileSync");
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        public static string GetExt(string dir)
        {
            // Set
            string ext;
            try
            {
                // Get name
                FileInfo file = new FileInfo(dir);
                ext = file.Name;

                // Get extension
                ext = ext.Contains("-") ? ext.Substring(0, ext.IndexOf("-")) : "xxx";

                // Libre de pecados
                return ext;
            }
            catch (Exception ex)
            {
                ext = "xxx";
                return ext;
            }
        }
    }
}
