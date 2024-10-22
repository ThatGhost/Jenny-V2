using System.Diagnostics;

namespace Jenny_V2.Services
{
    public class BrowserService
    {
        public void OpenUrlInBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void SearchInBrowser(string searchTerm)
        {
            OpenUrlInBrowser($"https://www.google.com/search?q={searchTerm}");
        }
    }
}
