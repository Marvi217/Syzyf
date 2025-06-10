using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data;
using WpfApp1.Model;

namespace WpfApp1.ViewModels
{
    public class MeetingsViewModel : INotifyPropertyChanged
    {
        private readonly SyzyfContext _context;

        private ObservableCollection<Meeting> _employeeMeetings;
        public ObservableCollection<Meeting> EmployeeMeetings
        {
            get => _employeeMeetings;
            set
            {
                _employeeMeetings = value;
                OnPropertyChanged(nameof(EmployeeMeetings));
            }
        }

        public MeetingsViewModel(SyzyfContext context)
        {
            _context = context;
            EmployeeMeetings = new ObservableCollection<Meeting>();
        }

        public async Task LoadMeetingsForEmployee(long employeeId)
        {
            var meetings = await _context.Meetings
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Employee)
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Candidate)
                .Include(m => m.Participants)
                    .ThenInclude(p => p.Client)
                .Where(m => m.Participants.Any(p => p.EmployeeId == employeeId))
                .OrderBy(m => m.StartTime)
                .ToListAsync();

            EmployeeMeetings = new ObservableCollection<Meeting>(meetings);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
