using Caliburn.Micro;
using SW3Projekt.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW3Projekt.ViewModels
{
    public class WorkplacesViewModel : Screen
    {
        // Collection used to determine which workplaces are currently being shown on the datagrid
        private BindableCollection<Workplace> _workplaceCollection;
        public BindableCollection<Workplace> WorkplaceCollection {
            get { return _workplaceCollection; }
            set {
                _workplaceCollection = value;
                NotifyOfPropertyChange(() => WorkplaceCollection);
            }
        }
        // Text displayed above the add btn etc. "Saved!"
        private string _progressStateTxt = "";
        public string ProgressStateTxt {
            get {
                return _progressStateTxt;
            }
            set {
                _progressStateTxt = value;
                NotifyOfPropertyChange(() => ProgressStateTxt);
            }
        }

        private enum ProgressStates { PleaseWait, Added, ErrorUserExists };

        // This checks if BtnAddNewWorkplace can be clicked
        public bool CanBtnAddNewWorkplace { get { return CanAddNewWorkplace; } }

        private bool _canAddNewWorkplace = true;
        public bool CanAddNewWorkplace {
            get {
                return _canAddNewWorkplace;
            }
            set {
                _canAddNewWorkplace = value;
                NotifyOfPropertyChange(() => CanAddNewWorkplace);
                NotifyOfPropertyChange(() => CanBtnAddNewWorkplace);
            }
        }

        private string _searchWorkplaceText = "";
        public string SearchWorkplaceText {
            get {
                return _searchWorkplaceText;
            }
            set {
                _searchWorkplaceText = value;
                NotifyOfPropertyChange(() => SearchWorkplaceText);
                Task.Run(async () => await SearchForWorkplaceAsync(SearchWorkplaceText));
            }
        }

        // List to constantly keep track of all the workplaces
        private List<Workplace> AllWorkplaces { get; set; }

        private Workplace _newWorkplace;
        public Workplace NewWorkplace {
            get {
                return _newWorkplace;
            }
            set {
                _newWorkplace = value;
                NotifyOfPropertyChange<Workplace>(() => NewWorkplace);
            }
        }

        public WorkplacesViewModel()
        {
            NewWorkplace = new Workplace();
            Task.Run(async () =>
            {
                AllWorkplaces = await GetWorkplacesAsync();
                WorkplaceCollection = new BindableCollection<Workplace>(AllWorkplaces);
            });
        }

        public async Task BtnAddNewWorkplace()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                ChangeProgressTxt(ProgressStates.PleaseWait);
                CanAddNewWorkplace = false;

                bool success = await Task<bool>.Run(() =>
                {
                    try
                    {
                        ctx.Workplaces.Add(NewWorkplace);
                        ctx.SaveChanges();
                        return true;
                    }
                    catch (DbUpdateException ex)
                    {
                        // Should we log exceptions?
                        ChangeProgressTxt(ProgressStates.ErrorUserExists);
                        return false;
                    }
                });

                if (success)
                {
                    NewWorkplace = new Workplace();
                    ChangeProgressTxt(ProgressStates.Added);

                    AllWorkplaces = await GetWorkplacesAsync();
                    WorkplaceCollection = new BindableCollection<Workplace>(AllWorkplaces);
                }

                CanAddNewWorkplace = true;
            }
        }
        private void ChangeProgressTxt(ProgressStates state)
        {
            switch (state)
            {
                case ProgressStates.PleaseWait:
                    ProgressStateTxt = "Vent venligst...";
                    break;
                case ProgressStates.Added:
                    ProgressStateTxt = "Gemt!";
                    break;
                case ProgressStates.ErrorUserExists:
                    ProgressStateTxt = "FEJL: Det tildelte løn nr. eksisterer allerede i databasen!";
                    break;
                default:
                    ProgressStateTxt = "";
                    break;
            }
        }
        public async Task SearchForWorkplaceAsync(string criteria)
        {
            // Searched by name
            // Check if the user cleared the field, and then just display everyone once more
            if (String.IsNullOrEmpty(criteria))
            {
                WorkplaceCollection = new BindableCollection<Workplace>(AllWorkplaces);
            }
            // Otherwise attempt to find employees based on the search value
            else
            {
                WorkplaceCollection = await Task.Run(() => new BindableCollection<Workplace>(AllWorkplaces.Where(x => x.Name.ToLower().Contains(criteria.ToLower())).ToList()));
            }
        }

        private async Task<List<Workplace>> GetWorkplacesAsync()
        {
            using (var ctx = new DatabaseDir.Database())
            {
                List<Workplace> workplaces = await Task.Run(() => ctx.Workplaces.ToList());

                return workplaces;

            }
        }
    }
}
