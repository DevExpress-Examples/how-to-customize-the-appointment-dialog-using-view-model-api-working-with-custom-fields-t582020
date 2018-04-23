Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Namespace DevExpressMvcApplication1
    ' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    ' visit http://go.microsoft.com/?LinkId=9394801

    Public Class MvcApplication
        Inherits System.Web.HttpApplication

        Public Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
            filters.Add(New HandleErrorAttribute())
        End Sub

        Public Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}")

            routes.MapRoute("Default", "{controller}/{action}/{id}", New With { _
                Key .controller = "Home", _
                Key .action = "Index", _
                Key .id = UrlParameter.Optional _
            })

        End Sub

        Protected Sub Application_Start()
            AreaRegistration.RegisterAllAreas()

            RegisterGlobalFilters(GlobalFilters.Filters)
            RegisterRoutes(RouteTable.Routes)

            ModelBinders.Binders.DefaultBinder = New DevExpress.Web.Mvc.DevExpressEditorsBinder()
        End Sub
    End Class
End Namespace