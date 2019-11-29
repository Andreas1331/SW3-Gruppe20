﻿using Caliburn.Micro;
using SW3Projekt.Models;
using SW3Projekt.Models.Repository;
using SW3Projekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SW3Projekt
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container.Instance(_container);

            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            GetType().Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => _container.RegisterPerRequest(
                    viewModelType, viewModelType.ToString(), viewModelType));

            _container
                .PerRequest<IRepository<Employee>, EntityFrameworkRepository<Employee>>()
                .PerRequest<IRepository<Workplace>, EntityFrameworkRepository<Workplace>>()
                .PerRequest<IRepository<Route>, EntityFrameworkRepository<Route>>()
                .PerRequest<IRepository<CollectiveAgreement>, EntityFrameworkRepository<CollectiveAgreement>>()
                .PerRequest<IRepository<TimesheetEntry>, EntityFrameworkRepository<TimesheetEntry>>();

            base.Configure();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            Console.WriteLine("Obj: " + instance);
            _container.BuildUp(instance);
        }
    }
}
