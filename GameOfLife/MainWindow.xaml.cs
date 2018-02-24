using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int numberOfRows;
        int numberOfColumns;
        List<Cell> cells = new List<Cell>();
        public MainWindow()
        {
            InitializeComponent();
            DynamicGrid.ShowGridLines = true;

        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            iteration(cells);


        }

        private void CreateDynamicWPFGrid(int numberColumns, int numberRows)
        {
            DynamicGrid.ColumnDefinitions.Clear();
            DynamicGrid.RowDefinitions.Clear();

            // Create Columns
            for (int i = 0; i < numberColumns; i++)
            {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            // Create Rows
            for (int i = 0; i < numberRows; i++)
            {
                DynamicGrid.RowDefinitions.Add(new RowDefinition());
            }



            for (int j = 0; j < numberColumns; j++)
            {
                for (int k = 0; k < numberRows; k++)
                {
                    cells.Add(new Cell(false, k, j));
                }
            }

            foreach (Cell c in cells)
            {
                Grid.SetRow(c.CellAppearance, c.Row);
                Grid.SetColumn(c.CellAppearance, c.Column);
                c.CellAppearance.MouseDown += delegate { cellClicked(c); };
                DynamicGrid.Children.Add(c.CellAppearance);
            }

            numberOfRows--;
            numberOfColumns--;


        }

        private void gridButton_Click(object sender, RoutedEventArgs e)
        {
            numberOfRows = Convert.ToInt16(rowTextBox.Text);
            numberOfColumns = Convert.ToInt16(columnTextBox.Text);

            CreateDynamicWPFGrid(numberOfColumns, numberOfRows);
        }

        private static void cellClicked(Cell mCell)
        {
            if (mCell.Alive)
                mCell.changeCellToDead();
            //dynamic grid children add?
            else
                mCell.changeCellToAlive();

        }

        private Cell findCell(int row, int column)
        {

            foreach (Cell c in cells)
            {
                if (c.Row == row && c.Column == column)
                    return c;
            }
            return null;

        }

        private void iteration(List<Cell> mCells)
        {
            foreach (Cell c in mCells)
            {
                int numberOfNeighbours = 0;
                int cellColumn = c.Column;
                int cellRow = c.Row;

                //column and row 
                int leftColumn = cellColumn - 1;
                int topRow = cellRow - 1;
                int rightColumn = cellColumn + 1;
                int bottomRow = cellRow + 1;


                if(topRow >= 0) //if top row exists
                {
                    //check top row middle
                    if (findCell(topRow, cellColumn).Alive) numberOfNeighbours++;

                    //if left column exists, check top left 
                    if (leftColumn >= 0) if (findCell(topRow, leftColumn).Alive) numberOfNeighbours++;

                    //if right column exists, check top right
                    if (rightColumn <= numberOfColumns) if (findCell(topRow, rightColumn).Alive) numberOfNeighbours++;
                }

                if(bottomRow <= numberOfRows)
                {
                    //check bottom row middle
                    if (findCell(bottomRow, cellColumn).Alive) numberOfNeighbours++;

                    //if left column exists, check bottom left
                    if (leftColumn >= 0) if (findCell(bottomRow, leftColumn).Alive) numberOfNeighbours++;

                    //if right column exists, check bottom right
                    if (rightColumn <= numberOfColumns) if (findCell(bottomRow, rightColumn).Alive) numberOfNeighbours++;
                }

                //if left column exists check left middle
                if (leftColumn >= 0) if (findCell(cellRow, leftColumn).Alive) numberOfNeighbours++;
                //if right column exisst check right middle
                if (rightColumn <= numberOfColumns) if (findCell(cellRow, rightColumn).Alive) numberOfNeighbours++;

              
               //sort boundarys
                if (c.Alive)
                {
                    if (numberOfNeighbours < 2 || numberOfNeighbours > 3)
                        c.ChangeFlag = true;
                } else
                {
                    if (numberOfNeighbours == 3)
                        c.ChangeFlag = true;
                }
              
                }

            foreach(Cell c in cells)
            {
                if (c.ChangeFlag)
                {
                    if (c.Alive)
                        c.changeCellToDead();
                    else
                        c.changeCellToAlive();

                    c.ChangeFlag = false;
                }
            }
            }
        }



    }





