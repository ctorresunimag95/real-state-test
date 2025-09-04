using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var sql = builder.AddSqlServer("sql")
    .WithHostPort(55425)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
var db = sql.AddDatabase("realStateTest");

var api = builder.AddProject<RealState_Test_Api>("api")
    .WithReference(db, "database")
    .WaitFor(db);

builder.Build().Run();