using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Data;
using WpfApp1.Model;

namespace WpfApp1.Services
{
    public class MeetingService
    {
        private readonly SyzyfContext _context;

        public MeetingService(SyzyfContext context)
        {
            _context = context;
        }

        public async Task<ObservableCollection<Meeting>> GetMeetingsForEmployeeAsync(long employeeId)
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

            return new ObservableCollection<Meeting>(meetings);
        }
    }
}

