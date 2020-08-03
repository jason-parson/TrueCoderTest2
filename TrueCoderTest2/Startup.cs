// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.BotFramework;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.TrueCoderTest2;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.AspNetCore.Mvc.Razor.TagHelpers;
using Microsoft.Bot.Builder.AI.QnA.Dialogs;
using Newtonsoft.Json.Serialization;

namespace Microsoft.TrueCoderTest2

{
    public class Startup 
    {

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();

            // Add the HttpClientFactory to be used for the QnAMaker calls.
            services.AddHttpClient();

            // Create the credential provider to be used with the Bot Framework Adapter.
            services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();

            // Create the Bot Framework Adapter with error handling enabled. 
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the bot as a transient. In this case the ASP Controller is expecting an IBot.
            services.AddTransient<IBot, QnABot>();

            //services.AddSingleton(new QnAMakerEndpoint
            //{
            //    KnowledgeBaseId = Configuration.GetValue<string>($"67e21116-aade-4b8e-8aa3-4cf171905479"),
            //    EndpointKey = Configuration.GetValue<string>($"3a9b23d2-34dd-41c5-83d7-c6ba8e0a6896"),
            //    Host = Configuration.GetValue<string>($"https://customqa.azurewebsites.net/qnamaker")
            //});

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseDefaultFiles()
                    .UseStaticFiles()
                    .UseRouting()
                    .UseAuthorization()
                    .UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                    });

                // app.UseHttpsRedirection();
            }
        }
    }
}
