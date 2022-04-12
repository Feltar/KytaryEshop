using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using KytaryEshop.Data;
using KytaryEshop.Areas.Identity.Data;
using Kytary.Backend.Business_Logika;
using Kytary.Models;
using Kytary.Backend.BModels;
using Dapper;
using System.Data.SqlClient;
using NHibernate;







ArtiklBModel artikl;
ObjednavkaBModel objednavka;
PolozkaKosikuBModel polozka;
PolozkaObjednavkyBModel polozkaObj;



KytaryEshop.PersistenceManager.ConfigureNHibernate();
ISessionFactory Fabrika =  KytaryEshop.PersistenceManager.SessionFabrika;








using (var session = Fabrika.OpenSession()) {
    using (var tx = session.BeginTransaction()){

         //var a = session.Get<PolozkaKosikuBModel>(24005);
         //a.PocetKusu += 1;
         //session.Update(a);


         int CenaKus = session.Query<ArtiklBModel>()
                .Where(y => y.IdArtikl == 2011)
                .Select(x => x.CenaKus)
                .Single();



        int sklademArtiklu = session.Query<ArtiklBModel>()
                        .Where(y => y.IdArtikl == 2011)
                        .Select(x => x.KusuNaSklade)
                        .Single();

        //[TO DO]
        var nacteneArtikly = session.QueryOver<ArtiklBModel>()
                                    .Where(x => x.TypArtiklu == 0)
                                    .Skip(12 * (1 - 1))
                                    .Take(12)             
                                    .List<ArtiklBModel>();




        artikl = session.Get<ArtiklBModel>(2011);

        polozkaObj = session.Get<PolozkaObjednavkyBModel>(7005);


        objednavka = session.Get<ObjednavkaBModel>(8005);



        polozka = session.Get<PolozkaKosikuBModel>(23003);
        tx.Commit();
    }
}




var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("UzivatelIdentitaDbContextConnection");
builder.Services.AddDbContext<UzivatelIdentitaDbContext>(options =>
    options.UseSqlServer(connectionString));
/*builder.Services.AddDefaultIdentity<UzivatelIdentita>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<UzivatelIdentitaDbContext>();*/
builder.Services.AddDefaultIdentity<UzivatelIdentita>(options => {
        
        options.SignIn.RequireConfirmedAccount = false;
        //nastaveni co chceme od hesla
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
    
    
    })
    .AddEntityFrameworkStores<UzivatelIdentitaDbContext>();




// Add services to the container.
builder.Services.AddControllersWithViews();




//--------------------
builder.Services.AddDistributedMemoryCache();
builder.Services.AddMvc();
builder.Services.AddSession(
        option =>
        {
            option.Cookie.IsEssential = true;
            option.IdleTimeout = TimeSpan.FromMinutes(20); //po 20 minuTACH NEAKTIvITY odhlasen

        }
    );









var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




//-------------------------
app.UseSession();
//------------------------










app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
