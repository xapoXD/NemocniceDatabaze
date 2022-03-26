using PptNemocnice.Shared;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => "Hello");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

List<VybaveniModel> seznam = VybaveniModel.GetTestList();

app.MapGet("/vybaveni", () =>
{
    return seznam;
});



app.MapGet("/vybaveni/jensrevizi", () =>
{
    return seznam.Where(x=>!x.NeedsRevision);
});





app.MapPost("/vybaveni", (VybaveniModel prichoziModel) =>
{
    // NEW
    Guid Id = Guid.NewGuid();
    var model = prichoziModel;
    model.Id = Id;  

    seznam.Insert(0, model);
});


app.MapDelete("/vybaveni/{Id}",(Guid Id ) =>
{
    var item = seznam.SingleOrDefault(x=> x.Id == Id);
    if (item == null) 
        return Results.NotFound("Tato položka nebyla nalezena!!");
    seznam.Remove(item);
    return Results.Ok();
}
);




// NEW ->>>>

app.MapPut("/vybaveni", (VybaveniModel prichoziModel) =>
{
    seznam.Remove(prichoziModel);

    Guid Id = Guid.NewGuid();
    VybaveniModel item = new();
    item.Id = Id;
    seznam.Insert(0, item);
   
});

app.MapGet("/vybaveni/jenurciteid/{Id}", (Guid Id) =>
{

    return seznam.Where(x => x.Id == Id);

});


app.Run();

//record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
//{
//    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
//}