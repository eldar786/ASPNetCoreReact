using aspnetserver.Data;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

//Adding a CORS policy to allow 
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        builder =>
    {
        builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000", "https://appname.azurestaticapps.net");//adding also kind of holder for React app when it's running in Azure later on.
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Changed AddSwaggerGen - added a little more information in our Swagger UI.
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.NET React Tutorial", Version = "v1" });
});

var app = builder.Build();

//Deleted IF statement for IsDevelopment, bcause I want Swagger UI to always show up, not only if the environment is development.
app.UseSwagger();
//Added SwaggerUI options
app.UseSwaggerUI(SwaggerUIOptions =>
{
    SwaggerUIOptions.DocumentTitle = "ASP.NET React Tutorial";
    SwaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API serving a very simple Post model.");
    SwaggerUIOptions.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

//Adding out pre set (above) CORSPolicy
app.UseCors("CORSPolicy");

//Adding an endpoint for getting all the posts
//Added a tag to our MapGet, so that the name of the little block above our endpoint changes.
app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId) =>
{
    Post postToReturn = await PostsRepository.GetPostByIdAsync(postId);

    if (postToReturn != null)
    {
        return Results.Ok(postToReturn);
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Posts Endpoints");

app.MapPost("/create-post", async (Post postToCreate) =>
{
    bool createSuccessful = await PostsRepository.CreatePostAsync(postToCreate);

    if (createSuccessful)
    {
        return Results.Ok("Create successful.");
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Posts Endpoints");

app.MapPut("/update-post", async (Post postToUpdate) =>
{
    bool updateSuccessful = await PostsRepository.UpdatePostAsync(postToUpdate);

    if (updateSuccessful)
    {
        return Results.Ok("Update successful.");
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Posts Endpoints");

app.MapDelete("/delete-post-by-id/{postId}", async (int postId) =>
{
    bool deleteSuccessful = await PostsRepository.DeletePostAsync(postId);

    if (deleteSuccessful)
    {
        return Results.Ok("Delete successful.");
    }
    else
    {
        return Results.BadRequest();
    }
}).WithTags("Posts Endpoints");

app.Run();