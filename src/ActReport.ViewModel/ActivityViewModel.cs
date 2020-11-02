using ActReport.Core.Contracts;
using ActReport.Core.Entities;
using ActReport.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace ActReport.ViewModel {
    public class ActivityViewModel : BaseViewModel {
        private Employee _employee;
        private ObservableCollection<Activity> _activities;
        private Activity _selectedActivity;

        public ObservableCollection<Activity> Activities {
            get => _activities;
            set {
                _activities = value;
                OnPropertyChanged(nameof(Activities));
            }
        }

        public Activity SelectedActivity {
            get => _selectedActivity;
            set {
                _selectedActivity = value;
                OnPropertyChanged(nameof(SelectedActivity));
            }
        }

        public string FullName => $"{_employee.FirstName} {_employee.LastName}";

        public ActivityViewModel(IController controller, Employee employee) : base(controller)
        {
            _employee = employee;
            LoadActivities();
        }

        private void LoadActivities()
        {
            using IUnitOfWork uow = new UnitOfWork();
            Activities = new ObservableCollection<Activity>(uow.ActivityRepository.Get(
                filter: x => x.Employee_Id == _employee.Id,
                orderBy: coll => coll.OrderBy(activity => activity.Date).ThenBy(activity => activity.StartTime)));

            Activities.CollectionChanged += Activities_CollectionChanged;
        }

        private void Activities_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                using (UnitOfWork uow = new UnitOfWork())
                {
                    foreach(var item in e.OldItems)
                    {
                        uow.ActivityRepository.Delete((item as Activity).Id);
                    }

                    uow.Save();
                }
            }
        }

        private ICommand _cmdAddActivity;
        public ICommand CmdAddActivity {
            get {
                if (_cmdAddActivity == null)
                {
                    _cmdAddActivity = new RelayCommand(
                        execute: _ => _controller.ShowWindow(new AddActivityViewModel(_controller, _employee, null)),
                        canExecute: _ => true
                    );
                }

                return _cmdAddActivity;
            }
        }

        private ICommand _cmdEditActivity;
        public ICommand CmdEditActivity {
            get {
                if (_cmdEditActivity == null)
                {
                    _cmdEditActivity = new RelayCommand(
                        execute: _ => _controller.ShowWindow(new AddActivityViewModel(_controller, _employee, SelectedActivity)),
                        canExecute: _ => SelectedActivity != null && _cmdDeleteActivity != null
                    );
                }

                return _cmdEditActivity;
            }
        }

        private ICommand _cmdDeleteActivity;
        public ICommand CmdDeleteActivity {
            get {
                if (_cmdDeleteActivity == null)
                {
                    _cmdDeleteActivity = new RelayCommand(
                       execute: _ =>
                       {
                           using IUnitOfWork uow = new UnitOfWork();

                           uow.ActivityRepository.Delete(SelectedActivity);
                           uow.Save();
                           LoadActivities();
                       },
                canExecute: _ => SelectedActivity != null && _cmdDeleteActivity != null);
                }

                return _cmdDeleteActivity;
            }
        }
    }
}
