using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Helpers;
using Lift_Project.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;

namespace Lift_Project.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string displayText;

        public string DisplayText
        {
            get { return displayText; }
            set
            {
                if (displayText != value)
                {
                    displayText = value;
                    OnPropertyChanged(nameof(displayText));
                }
            }
        }

        public static string liftText = "A";
        public string LiftText
        {
            get { return liftText; }
            set
            {
                if (liftText != value)
                {
                    liftText = value;
                    OnPropertyChanged(nameof(displayText));

                }
            }
        }

        public string updateFloorText = "0";
        public string UpdateFloorText
        {
            get { return updateFloorText; }
            set
            {
                if (updateFloorText != value)
                {
                    updateFloorText = value;
                    OnPropertyChanged(nameof(updateFloorText));
                }
            }
        }

        public ObservableCollection<Dictionary<string, Lift>> liftMap;

        public ObservableCollection<Dictionary<string, Lift>> LiftMap
        {
            get { return liftMap; }
            set
            {
                if (liftMap != value)
                {
                    liftMap = value;
                }
            }
        }


        public MainWindowViewModel()
        {
            List<Floor> list1 = new List<Floor>();
            List<Floor> list2 = new List<Floor>();
            List<Floor> list3 = new List<Floor>();
            List<Floor> list4 = new List<Floor>();

            list1.Add(new Floor(0));
            list2.Add(new Floor(11));
            list3.Add(new Floor(11));
            list4.Add(new Floor(0));



            Dictionary<int, List<User>> Dict1 = new Dictionary<int, List<User>>();
            Dictionary<int, List<User>> Dict2 = new Dictionary<int, List<User>>();
            Dictionary<int, List<User>> Dict3 = new Dictionary<int, List<User>>();
            Dictionary<int, List<User>> Dict4 = new Dictionary<int, List<User>>();


            // Initialize the LiftMap with data (replace this with your actual data)
            LiftMap = new ObservableCollection<Dictionary<string, Lift>>
            {
                new Dictionary<string, Lift>
                {
                    { "A", new Lift(1,list1,0,0,false,false,false,true,Dict1) },
                    { "B", new Lift(2,list2,12,0,false,false,false,true,Dict2)  },
                    { "C", new Lift(3,list3,8,0,false,false,false,true,Dict3)  },
                    { "D", new Lift(4,list4,0,0,false,false,false,true,Dict4)  },
                }
            };
        }



        private RelayCommand? floorCommand;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand FloorCommand { get { return new RelayCommand<string>(OnFloorBtnClicked); } }

        private void OnFloorBtnClicked(string clickedBtn)
        {
            string ClickedButton = clickedBtn;
            DisplayText = ClickedButton;
            UpdateFloorText = ClickedButton ;

            AssigningLift(ClickedButton);
        }

        public void AssigningLift(string clickedButton)
        {
            User user = new User();
            user.destinatedFloor = Convert.ToInt32(clickedButton);
            user.currentFloor = 2;
             

            foreach (Lift lifts in LiftMap[0].Values)
            {
                

                if ((user.IsUsermovingUp() && lifts.IsLiftGoingtUp(user)) && lifts.CurrentFloor <= user.currentFloor || lifts.IsLiftContainSelectedFloor(user.destinatedFloor))
                {
                    lifts.Floors.Add(new Floor(user.destinatedFloor));
                    lifts.IsMovingUp = true;
                    lifts.AddUserToLift(user);

                    //lifts.LiftTransition(user);

                    var x = user.currentFloor;
                    while (lifts.CurrentFloor < x)
                    {
                        lifts.LiftFloorTransaction(user);
                        lifts.CurrentFloor++;
                        if (lifts.CurrentFloor == 11)
                        {
                            lifts.IsOnTop = true;
                        }
                    }                    
                }

                if ((user.IsUsermovingDown() && lifts.IsLiftGoingDown(user)) && lifts.CurrentFloor >= user.currentFloor || lifts.IsLiftContainSelectedFloor(user.destinatedFloor))
                {
                    lifts.Floors.Add(new Floor(user.destinatedFloor));
                    lifts.AddUserToLift(user);
                    lifts.IsMovingUp = false;
                    lifts.LiftTransition(user);
                    UpdateFloorText = lifts.UpdatingFloors().ToString();

                }
            }
        }

    } 

       
       



}
