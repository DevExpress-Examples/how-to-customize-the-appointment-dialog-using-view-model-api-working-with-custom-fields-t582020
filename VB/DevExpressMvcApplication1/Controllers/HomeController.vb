Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports DevExpressMvcApplication1.Models

Namespace DevExpressMvcApplication1.Controllers
    Public Class HomeController
        Inherits Controller

        '
        ' GET: /Home/

        Public Function Index() As ActionResult
            Return View(SchedulerDataHelper.DataObject)
        End Function

        Public Function SchedulerPartial() As ActionResult
            Return PartialView("SchedulerPartial", SchedulerDataHelper.DataObject)
        End Function

        Public Function EditAppointment() As ActionResult
            UpdateAppointment()
            Return PartialView("SchedulerPartial", SchedulerDataHelper.DataObject)
        End Function

        Private Shared Sub UpdateAppointment()
            Dim appointmnets As List(Of CustomAppointment) = TryCast(System.Web.HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
            Dim resources As List(Of CustomResource) = TryCast(System.Web.HttpContext.Current.Session("ResourcesList"), List(Of CustomResource))

            Dim insertedAppts() As CustomAppointment = SchedulerExtension.GetAppointmentsToInsert(Of CustomAppointment)(SchedulerDataHelper.GetSchedulerSettings(), appointmnets, resources)
            SchedulerDataHelper.InsertAppointments(insertedAppts)


            Dim updatedAppts() As CustomAppointment = SchedulerExtension.GetAppointmentsToUpdate(Of CustomAppointment)(SchedulerDataHelper.GetSchedulerSettings(), appointmnets, resources)
            SchedulerDataHelper.UpdateAppointments(updatedAppts)

            Dim removedAppts() As CustomAppointment = SchedulerExtension.GetAppointmentsToRemove(Of CustomAppointment)(SchedulerDataHelper.GetSchedulerSettings(), appointmnets, resources)
            SchedulerDataHelper.RemoveAppointments(removedAppts)
        End Sub
    End Class
End Namespace
