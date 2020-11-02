using ActReport.Core.Contracts;
using ActReport.Core.Entities;
using ActReport.Persistence;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ActReport.ViewModel {
    public class AddActivityViewModel : BaseViewModel {
        private Activity _activity;
        private DateTime _date;
        private DateTime _startTime;
        private DateTime _endTime;
        private string _activityText;
        private bool _insert;

        public DateTime Date {
            get => _date;
            set {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public DateTime StartTime {
            get => _startTime;
            set {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        public DateTime EndTime {
            get => _endTime;
            set {
                _endTime = value;
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public string ActivityText {
            get => _activityText;
            set {
                _activityText = value;
                OnPropertyChanged(nameof(ActivityText));
            }
        }

        public AddActivityViewModel(IController controller, Employee employee, Activity activity) : base(controller)
        {
            _insert = activity == null;
            if (activity == null)
            {
                _activity = new Activity()
                {
                    Employee_Id = employee.Id
                };
                _date = DateTime.Now;
                _startTime = DateTime.Now;
                _endTime = DateTime.Now;
                _activityText = null;
            }
            else
            {
                _activity = activity;
                _date = activity.Date;
                _startTime = activity.StartTime;
                _endTime = activity.EndTime;
                _activityText = activity.ActivityText;
            }
        }

        private ICommand _cmdSave;
        public ICommand CmdSave {
            get {
                if (_insert)
                {
                    _cmdSave = new RelayCommand(
                      execute: _ =>
                      {
                          using IUnitOfWork uow = new UnitOfWork();
                          _activity.Date = Date;
                          _activity.StartTime = StartTime;
                          _activity.EndTime = EndTime;
                          _activity.ActivityText = ActivityText;
                          uow.ActivityRepository.Insert(_activity);
                          uow.Save();
                          _controller.CloseWindow(this);
                      },
                      canExecute: _ => _activity != null);
                }
                else
                {
                    _cmdSave = new RelayCommand(
                     execute: _ =>
                     {
                         using IUnitOfWork uow = new UnitOfWork();
                         _activity.Date = Date;
                         _activity.StartTime = StartTime;
                         _activity.EndTime = EndTime;
                         _activity.ActivityText = ActivityText;
                         uow.ActivityRepository.Update(_activity);
                         uow.Save();
                         _controller.CloseWindow(this);


                     },
                     canExecute: _ => _activity != null);
                }

                return _cmdSave;
            }
        }

        private ICommand _cmdCancel;
        public ICommand CmdCancel {
            get {
                if(_cmdCancel == null)
                {
                    _cmdCancel = new RelayCommand(
                        execute: _ =>
                        {
                            _controller.CloseWindow(this);
                        },
                        canExecute: _ => true);
                }

                return _cmdCancel;
            }
        }
    }
}
