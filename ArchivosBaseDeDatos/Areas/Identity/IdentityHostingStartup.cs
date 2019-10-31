using System;
using ArchivosBaseDeDatos.Areas.Identity.Data;
using ArchivosBaseDeDatos.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(ArchivosBaseDeDatos.Areas.Identity.IdentityHostingStartup))]
namespace ArchivosBaseDeDatos.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddDefaultIdentity<GestorUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<GestorContext>();
            });
        }
    }
}