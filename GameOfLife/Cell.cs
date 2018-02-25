using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLife
{
    class Cell
    {
        private bool alive; //set to true if cell alive
        private int row; //cells row position
        private int column; //cells column position
        private Label cellAppearance; //cells label which is displayed on grdi
        private bool changeFlag; //flag set when cells status needs changing

        public bool Alive
        {
            get
            {
                return alive;
            }

            set
            {
                alive = value;
            }
        }

        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
        }

        public int Column
        {
            get
            {
                return column;
            }

            set
            {
                column = value;
            }
        }

        public Label CellAppearance
        {
            get
            {
                return cellAppearance;
            }

            set
            {
                cellAppearance = value;
            }
        }

        public bool ChangeFlag
        {
            get
            {
                return changeFlag;
            }

            set
            {
                changeFlag = value;
            }
        }

        public Cell(bool m_alive, int m_row, int m_column)
        {
            this.alive = m_alive;
            this.row = m_row;
            this.column = m_column;
            this.cellAppearance = new Label();
            if (!m_alive)
                this.cellAppearance.Background = new SolidColorBrush(Colors.White);
            else
                this.cellAppearance.Background = new SolidColorBrush(Colors.Black);
            this.changeFlag = false;
            
        }

        /// <summary>
        /// Method called to set cell to alive
        /// </summary>
        public void changeCellToAlive() {
            this.alive = true; //set cell to alive
            this.cellAppearance.Background = new SolidColorBrush(Colors.Black); //display to UI
        }

        /// <summary>
        /// Method called to set cell to dead
        /// </summary>
        public void changeCellToDead() {
            this.alive = false; //set cell to dead
            this.cellAppearance.Background = new SolidColorBrush(Colors.White); //display to UI
        }
    }
}
