using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Data;
using WpfApp1.Model;
using WpfApp1.Models;


namespace WpfApp1.Views
{
    public partial class EditProjectPage : Page
    {
        private Frame _mainFrame;
        private User _user;
        private readonly SyzyfContext _context;
        private Project Project;
        public EditProjectPage(Frame mainFrame, User user, SyzyfContext syzyf, Project project)
        {
            _mainFrame = mainFrame;
            _user = user;
            _context = syzyf;
            Project = project;
            InitializeComponent();
        }
    }
}
