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
        private bool alive;
        private int row;
        private int column;
        private Label cellAppearance;
        private bool changeFlag;

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

        public void changeCellToAlive() {
            this.alive = true;
            this.cellAppearance.Background = new SolidColorBrush(Colors.Black);
        }

        public void changeCellToDead() {
            this.alive = false;
            this.cellAppearance.Background = new SolidColorBrush(Colors.White);
        }
    }
}
