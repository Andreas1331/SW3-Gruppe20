﻿using Caliburn.Micro;
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
        public int cornerRadius { get; set; } = 0;

        // Collection used to determine which workplaces are currently being shown on the datagrid
        private BindableCollection<Workplace> _workplaceCollection;
        public BindableCollection<Workplace> WorkplaceCollection {
            get { return _workplaceCollection; }
            set {
                _workplaceCollection = value;
                NotifyOfPropertyChange(() => WorkplaceCollection);
            }
        }

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

        public double MaxPayout
        {
            get
            {
                return NewWorkplace.MaxPayout;
            }
            set
            {
                NewWorkplace.MaxPayout = value;
                NotifyOfPropertyChange(() => MaxPayout);
            }
        }

        public Workplace SelectedWorkplace { get; set; }

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
            if (NewWorkplace.MaxPayout <= 0)
            {
                new Notification(Notification.NotificationType.Error, "Max beløb kan ikke være lig 0");
                return;
            }

            using (var ctx = new DatabaseDir.Database())
            {
                CanAddNewWorkplace = false;

                bool success = await Task<bool>.Run(() =>
                {
                    try
                    {
                        ctx.Workplaces.Add(NewWorkplace);
                        ctx.SaveChanges();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        // Should we log exceptions?
                        return false;
                    }
                });

                if (success)
                {
                    new Notification(Notification.NotificationType.Added, $"{NewWorkplace.Name} er blevet tilføjet til databasen.");
                    NewWorkplace = new Workplace();

                    AllWorkplaces = await GetWorkplacesAsync();
                    WorkplaceCollection = new BindableCollection<Workplace>(AllWorkplaces);
                }

                CanAddNewWorkplace = true;
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
                List<Workplace> workplaces = await Task.Run(() => ctx.Workplaces.Where(x => x.Archived == false).ToList());
                return workplaces;
            }
        }

        public void BtnDeleteSelectedWorkplace()
        {
            //If not found
            if (WorkplaceCollection.Where(x => x == SelectedWorkplace).Count() == 0)
                return;

            //Find the entryrow
            Workplace workplace = WorkplaceCollection.FirstOrDefault(x => x == SelectedWorkplace);

            //Delete from datagrid
            WorkplaceCollection.Remove(workplace);
            NotifyOfPropertyChange(() => WorkplaceCollection);

            // Archives the selected workplace
            using (var ctx = new DatabaseDir.Database())
            {
                ctx.Workplaces.First(x => x.Id == workplace.Id).Archived = true;
                ctx.SaveChanges();
            }
        }
    }
}
