using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift_Project.Model
{
    public class User
    {
        public User()
        {
        }

        public User(int currentFloor, int destinatedFloor)
        {
            this.currentFloor = currentFloor;
            this.destinatedFloor = destinatedFloor;
        }

        public int currentFloor { get; set; }
        public int destinatedFloor { get; set; }

        public bool IsUsermovingUp()
        {
            bool flag = false;
            if (destinatedFloor - currentFloor > 0)
            {
                flag = true;
            }
            else
            {

            }
            return flag;
        }

        public bool IsUsermovingDown()
        {
            bool flag = false;
            if (destinatedFloor - currentFloor < 0)
            {
                flag = true;
            }
            else
            {

            }
            return flag;
        }
    }
}


