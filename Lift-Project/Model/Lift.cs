using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lift_Project.Model
{
    public class Lift
    {
       

        //public Lift(List<Floor> floorNum)
        //{
        //    FloorNum = floorNum;
        //}

        public int LiftNumber { get; set; }
        public List<Floor> Floors { get; set; }
        public int CurrentFloor { get; set; }
        public int DestinatedFloor { get; set; }
        public bool IsMovingUp { get;set; }
        public bool IsMovingDown { get; set; }

        public bool IsOnTop { get; set; }
        public bool IsOnGrund { get; set; }
        public Dictionary<int, List<User>> userFlorMap;

        public Lift(int liftNumber, List<Floor> floors, int currentFloor, int destinatedFloor, bool isMovingUp, bool isMovingDown, bool isOnTop, bool isOnGrund, Dictionary<int, List<User>> userFlorMap)
        {
            LiftNumber = liftNumber;
            Floors = floors;
            CurrentFloor = currentFloor;
            DestinatedFloor = destinatedFloor;
            IsMovingUp = isMovingUp;
            IsMovingDown = isMovingDown;
            IsOnTop = isOnTop;
            IsOnGrund = isOnGrund;
            this.userFlorMap = userFlorMap;
        }

        public bool IsLiftContainSelectedFloor(int y)
        {
           bool flag =  Floors.Where(x => x.floorNum == y).Equals(Convert.ToInt32(y));
            return flag;
        }
        public bool IsLiftGoingtUp(User user)
        {
            bool flag = false;

            if ((Floors.Count == 1 && user.destinatedFloor > Floors[0].floorNum))
            {
                flag= true;
            }

            if(Floors.Count >= 2 && Floors[0].floorNum < Floors[1].floorNum)
            {
                flag = true;
            }
            
           
            return flag;
        }

        public bool IsLiftGoingDown(User user)
        {
            bool flag = false;
            if (Floors.Count == 1 && user.destinatedFloor < Floors[0].floorNum)
            {
                flag = true;
            }
            else
            {
                if (Floors.LastOrDefault()?.floorNum > user.currentFloor && Floors.LastOrDefault()?.floorNum>user.currentFloor)
                {
                    flag = true;
                }
            }
            return flag;
        }

        public void AddUserToLift(User user)
        {
            if (userFlorMap.ContainsKey(user.destinatedFloor))
            {
                userFlorMap[user.destinatedFloor].Add(user);
            }
            else
            {
                List<User> list = new List<User>();
                list.Add(user);
                userFlorMap.Add(user.destinatedFloor, list);
            }
        }

        public void LiftTransition(Lift lifts, User user)
        {

            if (lifts.IsMovingUp == true)
            {
                var x = user.currentFloor;
                while (lifts.CurrentFloor < x)
                {
                    LiftFloorTransaction(user);
                    lifts.CurrentFloor++;
                    UpdatingFloors();

                    if (lifts.CurrentFloor == 11)
                    {
                        lifts.IsOnTop = true;
                    }

                }
                x = user.destinatedFloor;
                while (lifts.CurrentFloor < x)
                {
                    LiftFloorTransaction(user);
                    lifts.CurrentFloor++;
                    UpdatingFloors();

                    if (lifts.CurrentFloor == 11)
                    {
                        lifts.IsOnTop = true;
                    }
                }
            }
            else
            {
                var x = user.currentFloor;
                while (lifts.CurrentFloor > x)
                {
                    LiftFloorTransaction(user);
                    lifts.CurrentFloor--;
                    UpdatingFloors();

                    if (lifts.CurrentFloor == 0)
                    {
                        lifts.IsOnGrund = true;
                    }
                    DoEmptyLift();

                }

                x = user.destinatedFloor;
                while (lifts.CurrentFloor > x)
                {
                    LiftFloorTransaction(user);
                    lifts.CurrentFloor--;
                    UpdatingFloors();

                    if (lifts.CurrentFloor == 0)
                    {
                        lifts.IsOnGrund = true;
                    }
                    DoEmptyLift();
                }
            }

        }

        public async void LiftFloorTransaction(User user)
        {
            if(user.currentFloor > CurrentFloor)
            {
                await Task.Delay(3000);
            }

            //if (user.currentFloor < CurrentFloor)
            //{
            //    await Task.Delay((CurrentFloor - user.currentFloor)*1000);
            //}

            if(user.currentFloor == CurrentFloor) {
                int x = user.destinatedFloor - CurrentFloor;
                if(x == 0)
                {
                    await Task.Delay(1000);

                }
                else
                {
                    await Task.Delay((x)*1000);
                }

            }
        }

        public void DoEmptyLift()
        {
            if (IsOnGrund)
            {
                Floor floor = new Floor(11);
                Floors.Clear();
                userFlorMap.Clear();
                CurrentFloor = floor.floorNum;
            }
            if (IsOnTop)
            {
                Floor floor = new Floor(0);
                Floors.Clear();
                Floors.Add(floor);
                CurrentFloor = floor.floorNum;
            }
        }

        public int UpdatingFloors()
        {
            return CurrentFloor;
        }
        public void LiftTransition(User user)
        {

            if (IsMovingUp)
            {
                var x = user.currentFloor;
                while (CurrentFloor < x)
                {
                    LiftFloorTransaction(user);
                    CurrentFloor++;
                    if (CurrentFloor == 11)
                    {
                        IsOnTop = true;
                    }

                }
                x = user.destinatedFloor;
                while (CurrentFloor < x)
                {
                    LiftFloorTransaction(user);
                    CurrentFloor++;
                    if (CurrentFloor == 11)
                    {
                        IsOnTop = true;
                    }
                }
            }
            else
            {
                var x = user.currentFloor;
                while (CurrentFloor > x)
                {
                    LiftFloorTransaction(user);
                    CurrentFloor--;
                    if (CurrentFloor == 0)
                    {
                        IsOnGrund = true;
                    }
                    DoEmptyLift();

                }

                x = user.destinatedFloor;
                while (CurrentFloor > x)
                {
                    LiftFloorTransaction(user);
                    CurrentFloor--;
                    if (CurrentFloor == 0)
                    {
                        IsOnGrund = true;
                    }
                    DoEmptyLift();

                }
            }

        }
    }
}
