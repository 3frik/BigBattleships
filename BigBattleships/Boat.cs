using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBattleships
{
    internal class Boat
    {
        internal int Length;
        internal int Xpos = 0;
        internal int Ypos = 0;
        internal bool isHorizontal;
    
        public Boat(int length, int xpos=0, int ypos=0, bool IsHorizontal=true)
        {
            Length = length;
            Xpos = xpos; Ypos = ypos;
            isHorizontal = IsHorizontal;

        }

        internal void Redeploy(int XCoor,int YCoor, int orientation)
        {
            Xpos = XCoor;
            Ypos = YCoor;
            if (orientation > 0)
            {
                isHorizontal = false;
            }
            else
            {
                isHorizontal = true;
            }
        }
    };
}
