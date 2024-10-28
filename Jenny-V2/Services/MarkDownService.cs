using System.Drawing.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Jenny_V2.Services
{
    public class MarkDownService
    {
        public Paragraph ConvertMarkDownIntoParaGraph(string text)
        {
            Paragraph paragraph = new Paragraph();
            bool inCodeBlock = false; // Track if we're in a code block

            string[] lineSeperatedText = text.Split('\n');
            foreach (string line in lineSeperatedText)
            {
                // Check for start or end of a code block
                if (line.StartsWith("```"))
                {
                    inCodeBlock = !inCodeBlock; // Toggle code block mode
                    continue; // Skip adding ``` lines to paragraph
                }

                if (inCodeBlock)
                {
                    // Add code block line without styling
                    paragraph.Inlines.Add(CreateCodeBlock(line));
                }
                else
                {
                    // Add regular Markdown styled line
                    paragraph.Inlines.Add(CreateRunFromString(line));
                }
            }

            return paragraph;
        }

        private Run CreateRunFromString(string line)
        {
            Run run = new Run(line);

            // Headings (up to 6 levels)
            if (line.StartsWith("######"))
            {
                run.Text = line.Substring(6).Trim();
                run.FontSize = 14;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.StartsWith("#####"))
            {
                run.Text = line.Substring(5).Trim();
                run.FontSize = 16;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.StartsWith("####"))
            {
                run.Text = line.Substring(4).Trim();
                run.FontSize = 18;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.StartsWith("###"))
            {
                run.Text = line.Substring(3).Trim();
                run.FontSize = 20;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.StartsWith("##"))
            {
                run.Text = line.Substring(2).Trim();
                run.FontSize = 22;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.StartsWith("#"))
            {
                run.Text = line.Substring(1).Trim();
                run.FontSize = 24;
                run.FontWeight = FontWeights.Bold;
            }

            // Bold
            if (line.Contains("**"))
            {
                line = line.Replace("**", "");
                run.Text = line;
                run.FontWeight = FontWeights.Bold;
            }
            else if (line.Contains("*") && !line.Contains("**"))
            {
                line = line.Replace("*", "");
                run.Text = line;
                run.FontWeight = FontWeights.Bold;
            }

            // Italics
            if (line.Contains("_"))
            {
                line = line.Replace("_", "");
                run.Text = line;
                run.FontStyle = FontStyles.Italic;
            }

            // Strikethrough
            if (line.Contains("~~"))
            {
                line = line.Replace("~~", "");
                run.Text = line;
                run.TextDecorations = TextDecorations.Strikethrough;
            }

            // Add a line break after each line
            run.Text += "\n";
            return run;
        }

        // Creates a code block with monospaced font and no additional styling
        private Run CreateCodeBlock(string line)
        {
            Run run = new Run(line.Trim());
            run.FontFamily = new FontFamily("Consolas"); // Monospaced font
            run.FontSize = 14;
            run.Foreground = Brushes.DarkSlateGray;

            run.Text += "\n";
            return run;
        }
    }
}
