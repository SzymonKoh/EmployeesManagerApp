using EmployeesManagerApp;
using EmployeesManagerApp.Data.Repositories;
using EmployeesManagerApp.Components;
using Microsoft.Extensions.DependencyInjection;
using EmployeesManagerApp.Data.Entities;

var services = new ServiceCollection();
services.AddSingleton<IApp, App>();
services.AddSingleton<IEmployeesManager<Employee>, EmployeesManager<Employee>>();
services.AddSingleton<IEmployeeProvider, EmployeeProvider>();

var serviceProvider = services.BuildServiceProvider();
var app = serviceProvider.GetService<IApp>()!;
app.Run();