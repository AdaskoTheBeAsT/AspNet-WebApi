using Swashbuckle.Application;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace ReferenceProject
{
    public static class SwaggerConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            // Use http://localhost:5000/swagger/ui/index to inspect API docs
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "A title for your API");
                    c.PrettyPrint();
                    c.RootUrl(
                        req => string.Concat(
                                req.RequestUri.GetLeftPart(UriPartial.Authority),
                                req.GetRequestContext().VirtualPathRoot.TrimEnd('/')));

                    // This code allow you to use XML-comments
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                    var commentsFile = Path.Combine(baseDirectory, "bin/" + commentsFileName);

                    c.IncludeXmlComments(commentsFile);
                })
                .EnableSwaggerUi();
        }
    }
}