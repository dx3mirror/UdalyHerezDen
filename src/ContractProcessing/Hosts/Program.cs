using Utilities.DbContextSettings;
using Utilities.MassTransitSettings;
using Utilities.MassTransitSettings.Configurations;
using Utilities.WolverineSettings;
using Warehouse.ContractProcessing.Applications.AppServices;
using Warehouse.ContractProcessing.Applications.AppServices.Abstract;
using Warehouse.ContractProcessing.Applications.AppServices.Sagas;
using Warehouse.ContractProcessing.Applications.Handlers.Commands.CreateUnloadingContractCommand;
using Warehouse.ContractProcessing.Infrastructures.Common.Configurators;
using Warehouse.ContractProcessing.Infrastructures.Common.Contexts;
using Warehouse.ContractProcessing.Infrastructures.Sagas;

var builder = WebApplication.CreateBuilder(args);

var rabbitCfg = builder.Configuration
    .GetSection("RabbitMq")
    .Get<RabbitMqConfiguration>();

builder.Services.AddDataAccess<ContractProcessingDbContext, ContractProcessingDbContextConfigurator>();
builder.Services.AddScoped<IUnloadingContractService, UnloadingContractService>();
var connectionString = builder.Configuration.GetConnectionString("ContractProcessingDb");
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("Connection string 'ContractProcessingDb' is not configured.");
if (rabbitCfg == null)
    throw new InvalidOperationException("RabbitMQ configuration is not initialized.");
builder.Services.AddMassTransitSaga<
    UnloadingContractStateMachine,
    UnloadingContractState,
    ContractProcessingDbContext>(
        connectionString,
        rabbitCfg);

builder.AddWolverineWithDefaults<CreateUnloadingContractCommandProcess>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
