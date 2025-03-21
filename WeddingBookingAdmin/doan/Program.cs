using Google.Cloud.Firestore;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Google.Cloud.Firestore.V1;

var builder = WebApplication.CreateBuilder(args);

// Đọc đường dẫn từ appsettings.json
var credentialsPath = builder.Configuration["Firestore:CredentialsPath"];

// Tạo FirestoreDb
var firestoreDb = FirestoreDb.Create("diamond-c4486", new FirestoreClientBuilder
{
    Credential = GoogleCredential.FromFile(credentialsPath)
}.Build());

// Đăng ký FirestoreDb như một dịch vụ
builder.Services.AddSingleton(firestoreDb);

// Thêm các dịch vụ khác vào container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình pipeline yêu cầu HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=AllDish}/{action=Index}/{id?}");

app.Run();
