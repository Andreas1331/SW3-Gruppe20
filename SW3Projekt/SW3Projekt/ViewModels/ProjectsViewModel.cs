using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Controls;

namespace SW3Projekt.ViewModels
{
    class ProjectsViewModel : Screen
    {
        //PROPERTIES
        public List<Project> AllProjects { get; set; } = new List<Project>();

        public BindableCollection<Project> ShownProjectsCollection { get; set; } = new BindableCollection<Project>();

        private string _searchProject;
        public string SearchProject { get { return _searchProject; } set { _searchProject = value; FiltherProjects(); } }

        private int _selectedWeek;
        public int SelectedWeek { get { return _selectedWeek; } set { _selectedWeek = value; Task.Run(async () => { await GetProjectsAsync(); }); } }

        private int _selectedYear;
        public int SelectedYear { get { return _selectedYear; } set { _selectedYear = value; Task.Run(async () => { await GetProjectsAsync(); }); } }

        //CONTRUCTOR
        public ProjectsViewModel()
        {
            Task.Run(async () => { await GetProjectsAsync(); });
        }

        //METHODS
        public void BtnFilter()
        {
            Task.Run(async () => { await GetProjectsAsync(); });
        }

        private async Task GetProjectsAsync()
        {
            List<TimesheetEntry> timesheetEntries;
            ShownProjectsCollection.Clear();
            AllProjects.Clear();

            //Get all entries with a project ID
            using (var ctx = new DatabaseDir.Database())
                //Get entries
                timesheetEntries = ctx.TimesheetEntries.Include(ts => ts.vismaEntries.Select(ve => ve.LinkedRate)).ToList();

            //Convert to projects
            foreach (TimesheetEntry timesheetEntry in timesheetEntries)
            {
                //Filter empty project IDs
                if (string.IsNullOrWhiteSpace(timesheetEntry.ProjectID))
                    continue;

                //Filter timesheets with no "Arbejde" types
                if (timesheetEntry.vismaEntries.Where(x => x.LinkedRate.Type == "Arbejde").ToList().Count() == 0)
                    continue;

                //Filter if a period is specified
                if (SelectedWeek > 0 && SelectedYear > 0)
                {
                    DateTime from = DateHelper.WeekNumToDateTime(SelectedWeek, SelectedYear, 0);
                    DateTime to = DateHelper.WeekNumToDateTime(SelectedWeek, SelectedYear, 6);

                    if (timesheetEntry.Date < from || timesheetEntry.Date > to)
                        continue; //Then skip
                }

                //Initalize
                double normalHours = 0;
                double overtimeHours = 0;

                //Sum up hours from the entry
                foreach (VismaEntry vismaEntry in timesheetEntry.vismaEntries)
                {
                    //Filter non "Arbejde" (work) types
                    if (vismaEntry.LinkedRate.Type != "Arbejde")
                        continue;

                    //Sum overtime hours
                    if (vismaEntry.LinkedRate.Name == "Normal")
                    {
                        if (vismaEntry.Value > 0)
                            normalHours += vismaEntry.Value;
                    }
                    else
                        overtimeHours += vismaEntry.Value;
                }

                //Add time to project
                if (!AllProjects.Where(p => p.ProjectID == timesheetEntry.ProjectID).ToList().Any())
                    //If project is not already listed
                    AllProjects.Add(new Project(timesheetEntry.ProjectID, normalHours, overtimeHours));
                else
                {   
                    //Else add hours to existing
                    Project SelectedProject = AllProjects.Where(p => p.ProjectID == timesheetEntry.ProjectID).FirstOrDefault();
                    SelectedProject.TotalNormalHours += normalHours;
                    SelectedProject.TotalOverTimeHours += overtimeHours;
                }
                AllProjects.Sort((x, y) => string.Compare(x.ProjectID, y.ProjectID));
                FiltherProjects();
            }
        }

        public void FiltherProjects()
        {
            if (!string.IsNullOrWhiteSpace(SearchProject)) //If something is entered then filter
                ShownProjectsCollection = new BindableCollection<Project>(AllProjects.Where(p => string.IsNullOrWhiteSpace(p.ProjectID) ? false : p.ProjectID.Contains(SearchProject)).ToList());
            else //Else show all elements
                ShownProjectsCollection = new BindableCollection<Project>(AllProjects.ToList());

            NotifyOfPropertyChange(() => ShownProjectsCollection);
        }

        //CLASSES
        public class Project
        {
            public string ProjectID { get; set; }
            public double TotalNormalHours { get; set; }
            public double TotalOverTimeHours { get; set; }

            public Project(string projectID, double totalNormalHours, double totalOverTimeHours)
            {
                ProjectID = projectID;
                TotalNormalHours = totalNormalHours;
                TotalOverTimeHours = totalOverTimeHours;
            }
        }
    }
}
