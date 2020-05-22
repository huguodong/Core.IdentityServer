using Core.IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Core.IdentityServer
{
    public static class IdentityServerSetup
    {
        public static void AddIdentityServerSetup(this IServiceCollection services, IHostEnvironment environment, string connectionString)
        {
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            if (services == null) throw new ArgumentNullException(nameof(services));


            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // 数据库配置系统应用用户数据上下文 
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseSqlServer(connectionString));
            // 启用 Identity 服务 添加指定的用户和角色类型的默认标识系统配置
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            // 添加配置数据（客户端 和 资源）
            .AddAspNetIdentity<ApplicationUser>()
            .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                })
            // 添加操作数据 (codes, tokens, consents)
            .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString,
                            sql => sql.MigrationsAssembly(migrationsAssembly));
                    // 自动清理 token ，可选
                    options.EnableTokenCleanup = true;
                })
            // 数据库配置系统应用用户数据上下文 

            //// in-memory, code config
            .AddTestUsers(InMemoryConfig.Users().ToList())
            .AddInMemoryApiResources(InMemoryConfig.GetApiResources())
            .AddInMemoryClients(InMemoryConfig.GetClients());


            //builder.AddDeveloperSigningCredential();
            builder.AddDeveloperSigningCredential();
            //if (environment.IsDevelopment())
            //{
            //    builder.AddDeveloperSigningCredential();
            //}
            //else
            //{
            //    throw new Exception("need to configure key material");
            //}
        }
    }
}
