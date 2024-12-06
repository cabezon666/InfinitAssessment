using InfinitAssessment.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IGitHubService, GitHubService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["GitHub:ApiBase"]);
    client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
    client.DefaultRequestHeaders.Add("User-Agent", "InfinitAssessment");
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/analyze", async (IGitHubService gitHubService) =>
{
    var result = await gitHubService.AnalyzeRepositoryAsync(builder.Configuration["GitHub:Owner"], builder.Configuration["GitHub:Repo"]);
    return Results.Ok(result);
});


app.MapGet("/", async context =>
{
    var html = @"
    <html lang='en'>
    <head>
        <meta charset='UTF-8'>
        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        <title>Analyze Repository -- Assessment for Infinit</title>     
        <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css' rel='stylesheet'>
        <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/js/bootstrap.bundle.min.js'></script>
    </head>
    <body>
        <div class='container mt-5'>
            <h1 class='text-center'>Repository Analysis</h1>
            <div id='loading' class='text-center my-5'>
                <div class='spinner-border text-primary' role='status'>
                    <span class='visually-hidden'>Loading...</span>
                </div>
            </div>
            <div id='summary' class='my-4' style='display:none;'>
                <h2>Summary</h2>
                <ul class='list-group'>
                    <li class='list-group-item'>Number of JS Files: <span id='jsFiles'></span></li>
                    <li class='list-group-item'>Number of TS Files: <span id='tsFiles'></span></li>
                    <li class='list-group-item'>Total Letters: <span id='totalLetters'></span></li>
                </ul>
            </div>
            <table class='table table-striped' id='resultsTable' style='display:none;'>
                <thead>
                    <tr>
                        <th>Letter</th>
                        <th>Quantity</th>
                        <th>Percentage (%)</th>
                    </tr>
                </thead>
                <tbody>
                
                </tbody>
            </table>
            <div class='alert alert-danger text-center' id='errorAlert' style='display:none;'>Error fetching data!</div>
        </div>

        <script>           
            fetch('/analyze')
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {              
                    document.getElementById('loading').style.display = 'none';             
                    document.getElementById('jsFiles').innerText = data.numberOfJsFiles;
                    document.getElementById('tsFiles').innerText = data.numberOfTsFiles;
                    document.getElementById('totalLetters').innerText = data.totalLetters;
                    document.getElementById('summary').style.display = '';   
                    const table = document.getElementById('resultsTable');
                    const tbody = table.querySelector('tbody');
                    data.letterCounts.forEach(item => {
                        const percentage = ((item.quantity / data.totalLetters) * 100).toFixed(2);
                        const row = `<tr><td>${item.letter}</td><td>${item.quantity}</td><td>${percentage}</td></tr>`;
                        tbody.innerHTML += row;
                    });
                    table.style.display = '';
                })
                .catch(error => {
                    console.error('Error fetching data:', error);
                    document.getElementById('loading').style.display = 'none';
                    document.getElementById('errorAlert').style.display = '';
                });
        </script>
    </body>
    </html>";

    context.Response.ContentType = "text/html";
    await context.Response.WriteAsync(html);
});

app.Run();