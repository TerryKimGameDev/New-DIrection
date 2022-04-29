using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace New_DIrection
{
    //Cell Class
    public class Cell
    {
        //bool for living state of cell
        private bool IsLive = false;
        //int for how many generations a cell has lived
        public int CellGen { get; private set; } = 0;

        public void CellToggle()
        {
            IsLive = !IsLive;
        }

        public void SetCell(bool value)
        {
            IsLive = value;
        }

        public bool GetCell()
        {
            return IsLive;
        }
    }
}
