using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;

namespace Sincronizador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            // Init
            InitializeComponent();

            // Deshabilitamos el error multiTask
            CheckForIllegalCrossThreadCalls = false;

            // Cultura chupistica
            var culture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;

            // Minimizamos
            WindowState = FormWindowState.Minimized;

            // Ocultamos de la barra de tarea
            ShowInTaskbar = false;

            // Listamos los procesos
            List<Process> processes = Process.GetProcesses().ToList();

            // Si hay mas de uno abierto, cerramos
            int count = processes.Count(x => x.ProcessName.ToLower() == "sincronizador");
            if (count > 1)
            {
                // Cerramos
                try { Application.Exit(); } catch (Exception e) { Console.WriteLine(e); }
                try { Close(); } catch (Exception e) { Console.WriteLine(e); }
            }

            _ = Descargador();
        }

        public async Task Descargador()
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    // Listamos los txt
                    List<string> listFiles = Directory.GetFiles("C:\\MileSync\\", "*.txt").ToList();

                    // Recorremos
                    foreach (var row in listFiles)
                    {
                        try
                        {
                            // Verificamos
                            Fichero.CheckDirectorio();

                            // Abrimos el archivo
                            string url = await File.ReadAllTextAsync(row);

                            // Mandamos a descargar
                            using (HttpClient httpClient = new HttpClient())
                            {
                                using (Task<Stream> streamAsync = httpClient.GetStreamAsync(url))
                                {
                                    using (FileStream fileStream = new FileStream($"C:\\MileSync\\{Guid.NewGuid()}.{Fichero.GetExt(row)}", FileMode.OpenOrCreate))
                                    {
                                        await streamAsync.Result.CopyToAsync(fileStream);
                                    }
                                }
                            }

                            // Borramos el txt
                            try { File.Delete(row); } catch (Exception e) { Console.WriteLine(e); }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    }

                    // Esperamos 1 seg
                    await Task.Delay(1000);
                }
            });
        }
    }
}