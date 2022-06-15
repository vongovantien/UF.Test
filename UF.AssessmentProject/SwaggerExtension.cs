using System; 
using System.IO;
using System.Linq;
using System.Reflection; 
using Microsoft.AspNetCore.Builder; 
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace UF.AssessmentProject
{
    /// <summary>
    /// SwaggerExtension 
    /// all swagger related configuration put here
    /// </summary> 
    public static class SwaggerExtension
    {
        /// <summary>
        /// Swagger Configuration
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v0.0.1",
                    Title = "SwaggerBase Api",
                    Description = "A simple example ASP.NET Core Web API. For Reference please click [getting-started-with-swashbuckle](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio) or [swagger.io/docs/](https://swagger.io/docs/)",
                    TermsOfService = new Uri("https://example.com/termsIfApplicable"),
                    License = new OpenApiLicense
                    {
                        Name = "License Use under UF testing",
                        Url = new Uri("https://example.com/license"),
                    }
                });


                    // Set the comments path for the Swagger JSON and UI.
                    //ensure the xml created in your API project is read here. for this sample is SwaggerBase Project.
                    //ensure the xml created in your API Model project is read here. for this sample is UF.SwaggerBase.Model.API
                    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}*.xml";
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, xmlFileName);
                foreach (var xmlFile in xmlFiles)
                {
                    x.IncludeXmlComments(xmlFile);
                }


                x.UseAllOfToExtendReferenceSchemas();
                x.UseInlineDefinitionsForEnums();


                    #region  EnumDesc
                    x.SchemaFilter<EnumSchemaFilter>();
                    #endregion

                    x.EnableAnnotations();
            });
        }

        /// <summary>
        /// Swagger UI Configuration
        /// 1. Add Swagger Options settings here 
        /// 2. set Configuration Service
        /// </summary>
        /// <param name="app"></param>
        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(option => { option.RouteTemplate = "swagger/{documentName}/swagger.json"; });

            //// Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(option =>
            {
                option.DefaultModelsExpandDepth(0);

                option.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");

                option.DisplayOperationId();
                option.DisplayRequestDuration();

                option.DefaultModelExpandDepth(5);
                    //option.DefaultModelRendering(ModelRendering.Model);  
                    option.DocExpansion(DocExpansion.None);
                    //option.EnableDeepLinking();
                    //option.EnableFilter();
                    //option.MaxDisplayedTags(5);
                    option.ShowExtensions();
                option.ShowCommonExtensions();
                    //option.EnableValidator(); 
                    //option.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head); 
                    //option.HeadContent = "<link rel='stylesheet' type='text/css' href='ui\\custom.css'>"; //to modify the ui
                });


        }



        /// <summary>
        /// Customize enum Display 
        /// </summary>
        public class EnumSchemaFilter : ISchemaFilter
        {
            public void Apply(OpenApiSchema schema, SchemaFilterContext context)
            {
                if (context.Type.IsEnum)
                {
                    var enumValues = schema.Enum.ToArray();
                    var i = 0;
                    schema.Enum.Clear();
                    foreach (var n in Enum.GetNames(context.Type).ToList())
                    {
                        schema.Enum.Add(new OpenApiString(n + $" = {((OpenApiPrimitive<int>)enumValues[i]).Value}"));
                        i++;
                    }
                }
            }
        }
    }
}