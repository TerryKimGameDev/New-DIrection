using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//GitHub now
//https://github.com/TerryKimGameDev/New-DIrection.git
namespace New_DIrection
{
    public partial class Form1 : Form
    {
        //Member Fields______________________________________________________________________________________________________________
        #region Member Fields
        // The universe array
        Cell[,] universe = new Cell[20, 20];

        // The scratchpad array
        Cell[,] nextUniverse;

        // Drawing colors
        Color gridColor = Color.Black;
        Color cellColor = Color.Gray;
        Color Grid10Color = Color.Black;

        // The Timer class
        Timer timer = new Timer();

        // Generation count //replaced functionality with a property
        int generations = 0;

        // number of living cells in the current universe //this should not be used use property
        int _Living = 0;

        //Timer Interval //property prefered
        int _interval = 100;

        // Seed value //property prefered
        private int _Seed = 20;

        //RunTo generation
        int stopat = -1;
        #endregion Member Fields

        //Properties_________________________________________________________________________________________________________________
        #region Properties
        //All properties are used to update the value on anything when they are changed
        public int Living
        {
            get { return _Living; }
            set
            {
                if (value != _Living)
                {
                    _Living = value;
                    LivingCount.Text = "Alive = " + Living.ToString();
                }
            }

        }
        public int Seed
        {
            get { return _Seed; }
            set
            {
                if (value != _Seed)
                {
                    _Seed = value;
                    SeedStatus.Text = "Seed = " + Seed.ToString();
                }
            }
        }

        public int Generations
        {
            get { return generations; }
            set
            {
                if (value != generations)
                {
                    generations = value;
                    toolStripStatusLabelGenerations.Text = "Generations = " + Generations.ToString();
                }
            }
        }

        public int Interval
        {
            get { return _interval; }
            set
            {
                if (value != _interval)
                {
                    _interval = value;
                    timer.Interval = Interval;
                    IntervalStatus.Text = "Interval = " + Interval.ToString();
                }
            }
        }
        #endregion Properties

        public Form1()
        {
            InitializeComponent();

            //Initialize arrays
            ArrayInitialization();

            // Setup the timer
            timer.Interval = Interval; // milliseconds
            timer.Tick += Timer_Tick;
        }

        // Calculate the next generation of cells
        private void NextGeneration()
        {

            // Increment generation count
            Generations++;

            Swap();
            graphicsPanel1.Invalidate();
        }

        // The event called by the timer every Interval milliseconds.
        private void Timer_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            if (generations == stopat)
            {
                timer.Stop();
                Play.Enabled = true;
                startToolStripMenuItem.Enabled = true;
                Next.Enabled = true;
                nextToolStripMenuItem.Enabled = true;
                Pause.Enabled = false;
                pauseToolStripMenuItem.Enabled = false;
            }

        }
        //Paint Eventers_________________________________________________________________________________________________________________________
        #region Painters
        //Painter
        private void graphicsPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Calculate the width and height of each cell in pixels
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0) - 0.01f;
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1) - 0.01f;

            // A Pen for drawing the grid lines (color, width)
            Pen gridPen = new Pen(gridColor, 1);

            // A Brush for filling living cells interiors (color)
            Brush cellBrush = new SolidBrush(cellColor);
            Living = 0;
            // A rectangle to represent each cell in pixels
            RectangleF cellRect = Rectangle.Empty;
            // Iterate through the universe in the y, top to bottom
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                // Iterate through the universe in the x, left to right
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;
                    cellRect.Width = cellWidth;
                    cellRect.Height = cellHeight;

                    // Fill the cell with a brush if alive
                    if (universe[x, y].GetCell() == true)
                    {
                        e.Graphics.FillRectangle(cellBrush, cellRect);
                        Living++;
                    }
                    if (neighborCountToolStripMenuItem.Checked == true)
                    {
                        NeighborDisplay(e, cellRect, x, y);
                    }
                    // Outline the cell with a pen and clear gridline
                    if (gridToolStripMenuItem.Checked == true)
                    {
                        e.Graphics.DrawRectangle(gridPen, cellRect.X, cellRect.Y, cellRect.Width, cellRect.Height);
                        Grid10by10(e);
                    }
                }
            }

            //show HUD //Place this after the for loops as order is important to not make the counter buffer repeatedly... duh
            if (hudToolStripMenuItem.Checked == true)
            {
                HUDelements(e);
            }
            if (timer.Enabled == false)
            {
                stopat = -1;
            }
            // Cleaning up pens and brushes
            gridPen.Dispose();
            cellBrush.Dispose();
        }
        //painter click
        private void graphicsPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            // If the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the width and height of each cell in pixels
                // Calculate the width and height of each cell in pixels
                // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
                float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0) - 0.01f;
                // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
                float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1) - 0.01f;

                // Calculate the cell that was clicked in
                // CELL X = MOUSE X / CELL WIDTH
                int x = (int)(e.X / cellWidth);
                // CELL Y = MOUSE Y / CELL HEIGHT
                int y = (int)(e.Y / cellHeight);

                // Toggle the cell's state
                universe[x, y].CellToggle();

                // Tell Windows you need to repaint
                graphicsPanel1.Invalidate();
            }
        }

        //display neighbor count
        private void NeighborDisplay(PaintEventArgs e, RectangleF rect, int X, int Y)
        {
            Brush tBrush = Brushes.Red;
            //get position from rect
            int count = 0;
            if (toroidalToolStripMenuItem.Checked == true)
            {
                count = CountNeighborsToroidal(X, Y);
            }
            else count = CountNeighborsFinite(X, Y);


            if (universe[X, Y].GetCell() == true && count == 2 || count == 3)
            {
                tBrush = Brushes.Green;
            }

            if (universe[X, Y].GetCell() == false && count == 3)
            {
                tBrush = Brushes.Green;
            }
            //string format settings
            Font font = new Font("Arial", rect.Height * 0.7f, FontStyle.Bold);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            //draws neighbor count to display
            if (count > 0)
            {
                e.Graphics.DrawString((count).ToString(), font, tBrush, rect, stringFormat);
            }
        }

        //The Hud display
        private void HUDelements(PaintEventArgs e)
        {
            //hud format string
            Font font = new Font("Arial", 12f, FontStyle.Bold);

            //should a string display as toroidal or finite
            string s = (toroidalToolStripMenuItem.Checked == true) ? "Toroidal" : "Finite";
            //the string to display
            string Hudtext = $"Generations: {Generations}\nCell Count: {Living}\nBoundary Type:{s}\nUniverse Size:(Width={universe.GetLength(0)}, Height={universe.GetLength(1)})";
            //a rectangle the size of the panel to adjust shape automatically
            RectangleF rect = new RectangleF(0, 0, graphicsPanel1.ClientSize.Width, graphicsPanel1.ClientSize.Height);

            //align text to bottom left
            StringFormat stringFormat = new StringFormat();
            stringFormat.LineAlignment = StringAlignment.Far;

            //draw the hud
            e.Graphics.DrawString(Hudtext, font, Brushes.Salmon, rect, stringFormat);
        }
        //the 10 by 10 grid display
        private void Grid10by10(PaintEventArgs e)
        {
            // CELL WIDTH = WINDOW WIDTH / NUMBER OF CELLS IN X
            float cellWidth = (float)graphicsPanel1.ClientSize.Width / (float)universe.GetLength(0) - 0.01f;
            // CELL HEIGHT = WINDOW HEIGHT / NUMBER OF CELLS IN Y
            float cellHeight = (float)graphicsPanel1.ClientSize.Height / (float)universe.GetLength(1) - 0.01f;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    // A rectangle to represent each cell in pixels
                    RectangleF cellRect = Rectangle.Empty;
                    cellRect.X = x * cellWidth;
                    cellRect.Y = y * cellHeight;

                    if (x % 10 == 0 && y % 10 == 0)
                    {
                        //draw the 10 by 10 grid based on modulo of 10
                        e.Graphics.DrawRectangle(new Pen(Grid10Color, 2), cellRect.X, cellRect.Y, cellWidth * 10, cellHeight * 10);
                    }
                }
            }
        }

        #endregion Painters
        //Personal Functions ____________________________________________________________________________________________________________________
        #region Personal Functions
        //for initializing everything in the given arrays
        private void ArrayInitialization()
        {
            //set scratchpad
            SetScratchpadSize();
            //initialize arrays
            for (int x = 0; x < universe.GetLength(0); x++)
            {
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    universe[x, y] = new Cell();
                    nextUniverse[x, y] = new Cell();
                }
            }
        }
        //Set the size of the scratchpad to universe size
        private void SetScratchpadSize()
        {
            nextUniverse = new Cell[universe.GetLength(0), universe.GetLength(1)];
        }
        //finite universe
        private int CountNeighborsFinite(int x, int y)
        {

            int count = 0;

            int xLen = universe.GetLength(0);

            int yLen = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)
            {
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    int xCheck = x + xOffset;
                    int yCheck = y + yOffset;
                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then continue
                    if (xCheck < 0)
                    {
                        continue;
                    }
                    // if yCheck is less than 0 then continue
                    if (yCheck < 0)
                    {
                        continue;
                    }
                    // if xCheck is greater than or equal too xLen then continue
                    if (xCheck >= xLen)
                    {
                        continue;
                    }
                    // if yCheck is greater than or equal too yLen then continue
                    if (yCheck >= yLen)
                    {
                        continue;
                    }
                    //test(e, xCheck, yCheck, count);
                    if (universe[xCheck, yCheck].GetCell() == true) count++;
                }
            }
            return count;
        }

        //toroidal universe
        private int CountNeighborsToroidal(int x, int y)

        {

            int count = 0;

            int xLen = universe.GetLength(0);

            int yLen = universe.GetLength(1);

            for (int yOffset = -1; yOffset <= 1; yOffset++)

            {

                for (int xOffset = -1; xOffset <= 1; xOffset++)

                {

                    int xCheck = x + xOffset;

                    int yCheck = y + yOffset;

                    // if xOffset and yOffset are both equal to 0 then continue
                    if (xOffset == 0 && yOffset == 0)
                    {
                        continue;
                    }
                    // if xCheck is less than 0 then set to xLen - 1
                    if (xCheck < 0)
                    {
                        xCheck = xLen - 1;
                    }
                    // if yCheck is less than 0 then set to yLen - 1
                    if (yCheck < 0)
                    {
                        yCheck = yLen - 1;
                    }

                    // if xCheck is greater than or equal too xLen then set to 0
                    if (xCheck >= xLen)
                    {
                        xCheck = 0;
                    }

                    // if yCheck is greater than or equal too yLen then set to 0
                    if (yCheck >= yLen)
                    {
                        yCheck = 0;
                    }
                    if (universe[xCheck, yCheck].GetCell() == true) count++;
                }
            }
            return count;

        }

        //Rules for next generations to follow
        private void CellRules(int count, int x, int y)
        {
            if (count < 2)
            {
                //NextGen[x, y] = false; //scratchpad death
                nextUniverse[x, y].SetCell(false);
            }
            if (count > 3)
            {
                //NextGen[x, y] = false;
                nextUniverse[x, y].SetCell(false);
            }
            if (universe[x, y].GetCell() == true && count == 2 || count == 3)
            {
                //NextGen[x, y] = true;
                nextUniverse[x, y].SetCell(true);
            }
            else nextUniverse[x, y].SetCell(false);

            if (universe[x, y].GetCell() == false && count == 3)
            {
                //NextGen[x, y] = true;
                nextUniverse[x, y].SetCell(true);
            }
        }

        //swap the universe with the scratchpad based on rules
        private void Swap()
        {
            //nested for loops to loop throught the size of universe
            int count = 0;
            for (int y = 0; y < universe.GetLength(1); y++)
            {
                for (int x = 0; x < universe.GetLength(0); x++)
                {
                    if (toroidalToolStripMenuItem.Checked == true)
                    {
                        count = CountNeighborsToroidal(x, y);
                    }
                    else
                        count = CountNeighborsFinite(x, y);
                    CellRules(count, x, y);
                }
            }
            // Swap them...
            Cell[,] temp = universe;
            universe = nextUniverse;
            nextUniverse = temp;
        }
        //Randomize the unverse based on a given seed And is to be used by many other functions
        private void RandomizeUniverse()
        {
            //seed the random
            Random Rint = new Random(Seed);

            //fill values for live/ dead cells
            for (int i = 0; i < universe.GetLength(0); i++)
            {
                for (int j = 0; j < universe.GetLength(1); j++)
                {
                    int num = Rint.Next(0, 3);
                    if (num == 0)
                    {
                        universe[i, j].SetCell(true);
                    }
                    else universe[i, j].SetCell(false);
                }
            }
            graphicsPanel1.Invalidate();
        }
        //color dialog control for changing whatever needs to have a color change
        private Color CdialogControl(Color clr)
        {
            ColorDialog Cdlg = new ColorDialog();
            Cdlg.Color = clr;
            if (DialogResult.OK == Cdlg.ShowDialog())
            {
                clr = Cdlg.Color;
            }
            return clr;
        }

        #endregion Personal Functions

        //Events_________________________________________________________________________________________________________________________________
        #region Events
        //Invalidation on click for multiple functions like the grid10 or hud tools
        private void Invalidator(object sender, EventArgs e)
        {
            graphicsPanel1.Invalidate();
        }
        //Useful Buttons______________________________________________________________________________________________________________
        #region Useful Buttons
        //next button
        private void NextButton(object sender, EventArgs e)
        {
            NextGeneration();
        }
        //runto
        private void RunTo(object sender, EventArgs e)
        {
            timer.Stop();
            //initialize runto dialogue
            RunGeneration RunThrough = new RunGeneration();
            RunThrough.generations = generations;

            //open run through
            if (DialogResult.OK == RunThrough.ShowDialog())
            {
                PlayButton(sender, e);
                //activate and deactivate associated buttons
                Play.Enabled = false;
                startToolStripMenuItem.Enabled = false;
                Next.Enabled = false;
                nextToolStripMenuItem.Enabled = false;
                Pause.Enabled = true;
                pauseToolStripMenuItem.Enabled = true;

                //store stopat
                stopat = RunThrough.StopAt;
                //start timer
                timer.Start();
            }
        }
        //play button
        private void PlayButton(object sender, EventArgs e)
        {
            timer.Start();
            Pause.Enabled = true;
            pauseToolStripMenuItem.Enabled = true;
            Next.Enabled = false;
            nextToolStripMenuItem.Enabled = false;
            Play.Enabled = false;
            startToolStripMenuItem.Enabled = false;
        }

        //stop button
        private void StopButton(object sender, EventArgs e)
        {
            timer.Stop();
            Pause.Enabled = false;
            pauseToolStripMenuItem.Enabled = false;
            Next.Enabled = true;
            nextToolStripMenuItem.Enabled = true;
            Play.Enabled = true;
            startToolStripMenuItem.Enabled = true;
        }
        //News Universe
        private void GridReset(object sender, EventArgs e)
        {
            timer.Stop();
            Generations = 0;
            Play.Enabled = true;
            startToolStripMenuItem.Enabled = true;
            Next.Enabled = true;
            nextToolStripMenuItem.Enabled = true;
            Pause.Enabled = false;
            pauseToolStripMenuItem.Enabled = false;
            ArrayInitialization();

            graphicsPanel1.Invalidate();
        }
        //Exit Button
        private void Exit(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion Useful Buttons
        //settings_____________________________________________________________________________________________________________
        #region Settings
        //color Settings________________________________________________________________________________________________
        #region Color Settings
        //backcolor
        private void BackColorChange(object sender, EventArgs e)
        {
            graphicsPanel1.BackColor = CdialogControl(graphicsPanel1.BackColor);
            graphicsPanel1.Invalidate();
        }
        //cellcolor
        private void CellColorChange(object sender, EventArgs e)
        {
            cellColor = CdialogControl(cellColor);
            graphicsPanel1.Invalidate();
        }
        //GridColor
        private void GridColorChange(object sender, EventArgs e)
        {
            gridColor = CdialogControl(gridColor);
            graphicsPanel1.Invalidate();
        }
        //Grid10Color
        private void Grid10ColorChange(object sender, EventArgs e)
        {
            Grid10Color = CdialogControl(Grid10Color);
            graphicsPanel1.Invalidate();
        }
        #endregion Color Settings 
        //Options________________________________________________________________________________________________
        #region Options
        //options
        private void Options(object sender, EventArgs e)
        {
            OptionsDialogue OpDialogue = new OptionsDialogue();
            //set values for the dialogue to display
            OpDialogue.interval = timer.Interval;
            OpDialogue.width = universe.GetLength(0);
            OpDialogue.height = universe.GetLength(1);
            if (DialogResult.OK == OpDialogue.ShowDialog())
            {
                //set the timer
                Interval = OpDialogue.interval;
                //if check so to not new universe should not be changed within specified parameters
                if (universe.GetLength(0) != OpDialogue.width || universe.GetLength(1) != OpDialogue.height)
                {
                    //set universe and scratchpad size
                    universe = new Cell[OpDialogue.width, OpDialogue.height];
                    GridReset(sender, e);
                }
            }
        }
        #endregion Options
        #endregion
        //Randomize__________________________________________________________________________________________
        #region Randomize
        //time based seed
        private void TimeRandom(object sender, EventArgs e)
        {
            //reset grid
            GridReset(sender, e);
            //rand based on time
            Random time = new Random(DateTime.Now.Millisecond);

            //get seed value
            Seed = time.Next(int.MinValue, int.MaxValue);

            RandomizeUniverse();
        }
        //Current Seed randomize
        private void CurrentSeed(object sender, EventArgs e)
        {
            //clear the universe
            GridReset(sender, e);

            //seed random
            Random Rint = new Random(Seed);

            //fill values for live/ dead cells
            for (int i = 0; i < universe.GetLength(0); i++)
            {
                for (int j = 0; j < universe.GetLength(1); j++)
                {
                    int num = Rint.Next(0, 3);
                    if (num == 0)
                    {
                        universe[i, j].SetCell(true);
                    }
                    else universe[i, j].SetCell(false);
                }
            }
            graphicsPanel1.Invalidate();
        }

        //Randomizer Dialogue
        private void RanDialogue(object sender, EventArgs e)
        {
            RandomizerDialogue randomDialogue = new RandomizerDialogue();
            //sets the seed in random dialog to Seed here
            randomDialogue.seed = Seed;
            if (DialogResult.OK == randomDialogue.ShowDialog())
            {
                //clear grid
                GridReset(sender, e);
                //get the seed value
                Seed = randomDialogue.seed;
                //seed random
                Random Rint = new Random(Seed);

                //fill universe based on seed
                for (int i = 0; i < universe.GetLength(0); i++)
                {
                    for (int j = 0; j < universe.GetLength(1); j++)
                    {
                        int num = Rint.Next(0, 3);
                        if (num == 0)
                        {
                            universe[i, j].SetCell(true);
                        }
                        else universe[i, j].SetCell(false);
                    }
                }
            }
        }


        #endregion Randomize
        //View________________________________________________________________________________________
        #region  View
        //torodial toolstrip
        private void toroidalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //toggle finite
            finiteToolStripMenuItem.Checked = !finiteToolStripMenuItem.Checked;
            graphicsPanel1.Invalidate();
        }
        //finite toolstip
        private void finiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //toggle toroidal
            toroidalToolStripMenuItem.Checked = !toroidalToolStripMenuItem.Checked;
            graphicsPanel1.Invalidate();
        }
        #endregion View
        //Save&LoadFileEvents___________________________________________________________________
        #region Save&load
        //Save File
        private void SaveFile(object sender, EventArgs e)
        {
            //save dialogue and setup
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2; dlg.DefaultExt = "cells";

            //show the save dialogue
            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamWriter writer = new StreamWriter(dlg.FileName);

                // Write any comments you want to include first.
                // Prefix all comment strings with an exclamation point.
                // Use WriteLine to write the strings to the file. 
                // It appends a CRLF for you.
                writer.WriteLine("!" + DateTime.Now + "\n");

                // Iterate through the universe one row at a time.
                for (int y = 0; y < universe.GetLength(1); y++)
                {
                    // Create a string to represent the current row.
                    String currentRow = string.Empty;

                    // Iterate through the current row one cell at a time.
                    for (int x = 0; x < universe.GetLength(0); x++)
                    {
                        // If the universe[x,y] is alive then append 'O' (capital O)
                        // to the row string.
                        if (universe[x, y].GetCell() == true)
                        {
                            currentRow += 'O';
                        }
                        // Else if the universe[x,y] is dead then append '.' (period)
                        // to the row string.
                        else
                        {
                            currentRow += '.';
                        }
                    }
                    // Once the current row has been read through and the 
                    // string constructed then write it to the file using WriteLine.
                    writer.WriteLine(currentRow);
                }
                // After all rows and columns have been written then close the file.
                writer.Close();
            }
        }
        //open file
        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*|Cells|*.cells";
            dlg.FilterIndex = 2;

            if (DialogResult.OK == dlg.ShowDialog())
            {
                StreamReader reader = new StreamReader(dlg.FileName);

                // Create a couple variables to calculate the width and height
                // of the data in the file.
                int maxWidth = 0;
                int maxHeight = 0;

                // Iterate through the file once to get its size.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();

                    // If the row begins with '!' then it is a comment
                    // and should be ignored.
                    // If the row is not a comment then it is a row of cells.
                    // Increment the maxHeight variable for each row read.
                    if (!row.StartsWith("!") && !string.IsNullOrEmpty(row))
                    {
                        maxHeight++;
                    }
                    // Get the length of the current row string
                    // and adjust the maxWidth variable if necessary.
                    maxWidth = row.Length;
                }

                // Resize the current universe and scratchPad
                // to the width and height of the file calculated above.
                universe = new Cell[maxWidth, maxHeight];
                ArrayInitialization();


                // Reset the file pointer back to the beginning of the file.
                reader.BaseStream.Seek(0, SeekOrigin.Begin);


                //yposition of universe
                int y = 0;

                // Iterate through the file again, this time reading in the cells.
                while (!reader.EndOfStream)
                {
                    // Read one row at a time.
                    string row = reader.ReadLine();


                    // If the row begins with '!' then
                    // it is a comment and should be ignored.
                    // If the row is not a comment then 
                    // it is a row of cells and needs to be iterated through.
                    if (!row.StartsWith("!") && !string.IsNullOrEmpty(row))
                    {
                        for (int xPos = 0; xPos < row.Length; xPos++)
                        {
                            // If row[xPos] is a 'O' (capital O) then
                            // set the corresponding cell in the universe to alive.
                            if (row[xPos] == 'O')
                            {
                                universe[xPos, y].SetCell(true);
                            }
                            // If row[xPos] is a '.' (period) then
                            // set the corresponding cell in the universe to dead.
                            else
                            {
                                universe[xPos, y].SetCell(false);
                            }
                        }
                        y++;
                    }
                }
                graphicsPanel1.Invalidate();
                // Close the file.
                reader.Close();
            }
        }
        #endregion
        //UserSettingsEvents____________________________________________________
        #region User Settings
        //on Formload load user settings
        private void FormLoad(object sender, EventArgs e)
        {
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            Grid10Color = Properties.Settings.Default.Grid10Color;
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            Interval = Properties.Settings.Default.Interval;
            Seed = Properties.Settings.Default.Seed;
            universe = new Cell[Properties.Settings.Default.UniW, Properties.Settings.Default.UniH];
            GridReset(sender, e);

        }
        //on FormClose user settings are saved
        private void FormClose(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.CellColor = cellColor;
            Properties.Settings.Default.Grid10Color = Grid10Color;
            Properties.Settings.Default.GridColor = gridColor;
            Properties.Settings.Default.BackColor = graphicsPanel1.BackColor;
            Properties.Settings.Default.Interval = Interval;
            Properties.Settings.Default.Seed = Seed;
            Properties.Settings.Default.UniH = universe.GetLength(1);
            Properties.Settings.Default.UniW = universe.GetLength(0);
            Properties.Settings.Default.Save();
        }
        //Reset to defaults on toolstrip click
        private void Reset(object sender, EventArgs e)
        {

            Properties.Settings.Default.Reset();
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            Grid10Color = Properties.Settings.Default.Grid10Color;
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            Interval = Properties.Settings.Default.Interval;
            Seed = Properties.Settings.Default.Seed;
            universe = new Cell[Properties.Settings.Default.UniW, Properties.Settings.Default.UniH];
            GridReset(sender, e);
        }
        //reload to previous settings
        private void Reload(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reload();
            cellColor = Properties.Settings.Default.CellColor;
            gridColor = Properties.Settings.Default.GridColor;
            Grid10Color = Properties.Settings.Default.Grid10Color;
            graphicsPanel1.BackColor = Properties.Settings.Default.BackColor;
            Interval = Properties.Settings.Default.Interval;
            Seed = Properties.Settings.Default.Seed;
            universe = new Cell[Properties.Settings.Default.UniW, Properties.Settings.Default.UniH];
            GridReset(sender, e);
        }
        #endregion User Settings

        #endregion Events

    }
}
