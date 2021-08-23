using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ASEDemo
{
    public partial class Form1 : Form
    {
        public static readonly Bitmap OutputBitmap = new Bitmap(444, 388);   //readonly (const) ensure the size never changes during runtime (determines size of image being drawn)
        readonly CanvasCommands Canvas;
        readonly VarCommand varCommand;
        readonly LoopCommand loopCommand;
        readonly IfElseCommands ifElseCommands;
        readonly MethodCommand methodCommand;

        public Form1()
        {
            InitializeComponent();
            Canvas = new CanvasCommands(Graphics.FromImage(OutputBitmap), varCommand);  //passes bitmap thru the graphics area (on the OutputArea below)
            varCommand = new VarCommand();
            loopCommand = new LoopCommand();
            ifElseCommands = new IfElseCommands(varCommand);
            methodCommand = new MethodCommand();
        }

        /// <summary>
        /// Bitmap is stored.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutputArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImageUnscaled(OutputBitmap, 0, 0);
        }

        /// <summary>
        /// Message shown when exception occurs.
        /// </summary>
        public void InvalidInput()     //displays mssg when invalid command/parameter inputted
        {
            var image = new Bitmap(this.OutputArea.Width, this.OutputArea.Height);
            var font = new Font("TimesNewRoman", 25, FontStyle.Bold, GraphicsUnit.Pixel);
            var graphics = Graphics.FromImage(image);
            graphics.DrawString("Invalid command/parameter entered", font, Brushes.Black, new Point(0, 0));
            this.OutputArea.Image = image;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Executes commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CommandLineInterface_KeyDown(object sender, KeyEventArgs e)
        {
            string[] commandLines = CommandLineInterface.Lines;     // Get each line from the CommandLineInterface and separate them.
            string[] allLines = ProgramArea.Lines;         // Get each line from the ProgramArea too.
            var parser = new CommandParser(this.varCommand);       // Uses the CommandParser() to parse input.


            if (e.KeyCode == Keys.Enter || commandLines.Contains("run"))
            {
                varCommand.Clear();
                methodCommand.Reset();      // everytime loop refreshes - prevents leftovers

                List<String> allCommands = allLines.ToList<String>();
                allCommands.AddRange(commandLines);

                // Do something for each line.
                for (int i = 0; i < allCommands.Count; i++)
                {
                    try
                    {
                        String line = allCommands[i];

                        // If it's the "draw to" line...
                        if (line.Trim().ToLower().Contains("draw to"))
                        {
                            // Get the parts past the command.
                            var tokens = parser.TokenizeCommand(line, "draw to");
                            // If they are both numbers...
                            if (int.TryParse(tokens[0].Trim(), out int x) &&
                                int.TryParse(tokens[1].Trim(), out int y))
                            {
                                // Draw a line to the indicated position.
                                Canvas.DrawTo(x, y);
                            }
                        }
                        else if (line.Trim().ToLower().Contains("rect"))
                        {
                            // Get the parts past the command.
                            var tokens = parser.TokenizeCommand(line, "rect");
                            // If they are both numbers...
                            if (int.TryParse(tokens[0].Trim(), out int t) &&
                                int.TryParse(tokens[1].Trim(), out int v))
                            {
                                // Draw to the indicated position.
                                Canvas.DrawRectangle(t, v);
                            }
                        }
                        else if (line.Trim().ToLower().Contains("circle"))
                        {
                            // Get the parts past the command.
                            var tokens = parser.TokenizeCommand(line, "circle");
                            // If they are both numbers...
                            if (float.TryParse(tokens[0].Trim(), out float R))
                            {
                                // Draw to the indicated position.
                                Canvas.DrawCircle(R);
                            }
                        }
                        else if (line.Trim().ToLower().Contains("triangle"))
                        {
                            // Get the parts past the command.
                            var tokens = parser.TokenizeCommand(line, "triangle");

                            // If they are both numbers...
                            if (int.TryParse(tokens[0].Trim(), out int T) &&
                                int.TryParse(tokens[1].Trim(), out int T2))
                            {
                                // Draw to the indicated position.
                                Canvas.DrawTriangle(T, T2);
                            }
                        }
                        else if (line.Trim().ToLower().Contains("move to"))
                        {
                            // Get the parts past the command.
                            var tokens = parser.TokenizeCommand(line, "move to");
                            // If they are both numbers...
                            if (int.TryParse(tokens[0].Trim(), out int move) &&
                                int.TryParse(tokens[1].Trim(), out int move2))
                            {
                                // Draw to the indicated position.
                                Canvas.MovePen(move, move2);
                            }
                        }
                        else if (line.Trim().ToLower().Contains("pen colour"))
                        {
                            var tokens = parser.TokenizeCommand(line, "pen colour");
                            // to ensure its a string...
                            if (tokens.Length > 0)
                            {
                                var colour = tokens[0].Trim();
                                Canvas.PenColour(colour);
                            }
                        }
                        else if (line.Trim().ToLower().Contains("fill pen"))
                        {
                            var tokens = parser.TokenizeCommand(line, "fill pen");
                            // to ensure its a string...
                            if (tokens.Length > 0)
                            {
                                var mode = tokens[0].Trim();
                                Canvas.FillPen(mode);
                            }
                        }
                        else if (line.Trim().ToLower().Equals("clear"))
                        {
                            Canvas.Clear();
                        }

                        else if (line.Trim().ToLower().Equals("reset"))
                        {
                            Canvas.Reset();
                        }
                        else if (line.Equals("run") || line.Equals(""))     // "" is when return is pressed
                        {
                            Console.WriteLine("no error to display...");    // Both are correct inputs
                        }
                        else if (line.Trim().ToLower().Contains("loop for "))
                        {
                            loopCommand.SetStartLineNo(i);
                            var tokens = parser.TokenizeCommand(line.Trim().ToLower(), "loop for");

                            loopCommand.SetLoopCount(varCommand.Parse(tokens[0]));

                            i = loopCommand.EndLoop(allCommands, i);

                        }
                        else if (line.Trim().ToLower().Contains("end for"))
                        {
                            i = loopCommand.getStartLineNo() - 1;
                        }
                        else if (line.Contains(" = ") || line.Contains(" + ") || line.Contains(" - "))
                        {
                            varCommand.Parse(line);
                        }
                        else if (line.Trim().ToLower().Contains("if "))
                        {
                            var split_tokens = line.Trim().ToLower().Substring("if".Length);
                            String[] tokens = split_tokens.Trim().ToLower().Split('=');

                            if (varCommand.Parse(tokens[0]) != varCommand.Parse(tokens[1]))
                            {
                                i = ifElseCommands.IfCommand(allCommands);      // i Is the loop we are in and executes them all within if
                            }
                            else
                            {
                                ifElseCommands.Reset();
                            }
                        }
                        else if (line.Trim().ToLower().Contains("endif"))
                        {
                            // SKIP
                        }
                        else if (line.Trim().ToLower().Contains("method "))
                        {
                            i = methodCommand.RegisterMethod(allCommands, i);
                        }
                        else if (line.Trim().ToLower().Contains("endmethod"))
                        {
                            i = methodCommand.ReturnMethod(allCommands);
                        }
                        else if (line.Trim().ToLower().Contains("(") &&
                            line.Trim().ToLower().Contains(")"))            // () specifies method
                        {
                            i = methodCommand.CallMethod(allCommands, line.Trim().ToLower(), i);
                        }
                        else
                        {
                            InvalidInput();
                        }
                    }
                    catch (Exception)    // When wrong input entered 
                    {
                        InvalidInput();
                    }
                }
                CommandLineInterface.Text = ""; // Clears the CLI
                Refresh();  // Ensures the user input is generated stryt after
            }
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProgramArea.AppendText(ProgramArea.Text);
            System.IO.File.WriteAllText(@"C:\Users\Mhasa\Desktop\ASEDemo.txt", ProgramArea.Text.Replace("\n", Environment.NewLine));
        }                                                       //saves as ASEDemo

        /// <summary>
        /// Opens files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog opentext = new OpenFileDialog();
            if (opentext.ShowDialog() == DialogResult.OK)
            {
                ProgramArea.Text = File.ReadAllText(opentext.FileName);
            }
        }

        private void ProgramArea_TextChanged(object sender, EventArgs e)
        {
        }

        private void MenuBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}