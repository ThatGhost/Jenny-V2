using System.Windows;
using System.Windows.Documents;

namespace Jenny_V2.Services
{
    public class MarkDownService
    {
        public Paragraph ConvertMarkDownIntoParaGraph(string text)
        {
            Paragraph paragraph = new Paragraph();

            string[] lineSeperatedText = text.Split('\n');
            foreach (string line in lineSeperatedText)
            {
                paragraph.Inlines.Add(CreateRunFromString(line));
            }

            return paragraph;
        }

        private Run CreateRunFromString(string line)
        {
            Run run = new Run(line);

            // Headings
            if (line.StartsWith("###"))
            {
                run.Text = line.Substring(3).Trim();
                run.FontSize = 18;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.StartsWith("##"))
            {
                run.Text = line.Substring(2).Trim();
                run.FontSize = 20;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.StartsWith("#"))
            {
                run.Text = line.Substring(1).Trim();
                run.FontSize = 24;
                run.FontWeight = FontWeights.Bold;
            }

            // Bold text
            if (line.Contains("**"))
            {
                run.Text = line.Replace("*", "");
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.Contains("*") && !line.Contains("**"))
            {
                run.Text = line.Replace("*", "");
                run.FontWeight = FontWeights.Bold;
            }

            // Italics
            if (line.Contains("_"))
            {
                run.Text = line.Replace("_", "");
                run.FontStyle = FontStyles.Italic;
            }

            // Strikethrough
            if (line.Contains("~~"))
            {
                run.Text = line.Replace("~~", "");
                run.TextDecorations = TextDecorations.Strikethrough;
            }

            run.Text += "\n";
            return run;
        }
    }
}
