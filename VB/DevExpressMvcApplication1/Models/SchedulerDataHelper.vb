Imports Microsoft.VisualBasic
Imports DevExpress.Web.Mvc
Imports DevExpress.XtraScheduler
Imports DXWebApplication1.Models
Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Web

Namespace DevExpressMvcApplication1.Models
	Public Class SchedulerDataHelper
		Public Shared Function GetResources() As List(Of CustomResource)
			Dim resources As New List(Of CustomResource)()
			resources.Add(CustomResource.CreateCustomResource(1, "Max Fowler", Color.Yellow.ToArgb()))
			resources.Add(CustomResource.CreateCustomResource(2, "Nancy Drewmore", Color.Green.ToArgb()))
			resources.Add(CustomResource.CreateCustomResource(3, "Pak Jang", Color.LightPink.ToArgb()))
			Return resources
		End Function

		Private Shared myRand As New Random()
		Public Shared Function GetAppointments(ByVal resources As List(Of CustomResource)) As List(Of CustomAppointment)
			Dim appointments As New List(Of CustomAppointment)()
			For Each item As CustomResource In resources
				Dim subjPrefix As String = item.Name & "'s "
				appointments.Add(CustomAppointment.CreateCustomAppointment("meeting", item.ResID, 2, 5, lastInsertedID))
				lastInsertedID += 1
				appointments.Add(CustomAppointment.CreateCustomAppointment("travel", item.ResID, 3, 6, lastInsertedID))
				lastInsertedID += 1
				appointments.Add(CustomAppointment.CreateCustomAppointment("phone call", item.ResID, 0, 10, lastInsertedID))
				lastInsertedID += 1
			Next item
			Return appointments
		End Function


		Public Shared ReadOnly Property DataObject() As SchedulerDataObject
			Get
                Dim dataObject_Renamed As New SchedulerDataObject()


                If HttpContext.Current.Session("ResourcesList") Is Nothing Then
					HttpContext.Current.Session("ResourcesList") = GetResources()
				End If
				dataObject_Renamed.Resources = TryCast(HttpContext.Current.Session("ResourcesList"), List(Of CustomResource))

				If HttpContext.Current.Session("AppointmentsList") Is Nothing Then
					HttpContext.Current.Session("AppointmentsList") = GetAppointments(dataObject_Renamed.Resources)
				End If
				dataObject_Renamed.Appointments = TryCast(HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
				Return dataObject_Renamed
			End Get
		End Property

		Private Shared defaultAppointmentStorage_Renamed As MVCxAppointmentStorage
		Public Shared ReadOnly Property DefaultAppointmentStorage() As MVCxAppointmentStorage
			Get
				If defaultAppointmentStorage_Renamed Is Nothing Then
					defaultAppointmentStorage_Renamed = CreateDefaultAppointmentStorage()
				End If
				Return defaultAppointmentStorage_Renamed

			End Get
		End Property

		Private Shared Function CreateDefaultAppointmentStorage() As MVCxAppointmentStorage
			Dim appointmentStorage As New MVCxAppointmentStorage()
			appointmentStorage.AutoRetrieveId = True
			appointmentStorage.Mappings.AppointmentId = "ID"
			appointmentStorage.Mappings.Start = "StartTime"
			appointmentStorage.Mappings.End = "EndTime"
			appointmentStorage.Mappings.Subject = "Subject"
			appointmentStorage.Mappings.AllDay = "AllDay"
			appointmentStorage.Mappings.Description = "Description"
			appointmentStorage.Mappings.Label = "Label"
			appointmentStorage.Mappings.Location = "Location"
			appointmentStorage.Mappings.RecurrenceInfo = "RecurrenceInfo"
			appointmentStorage.Mappings.ReminderInfo = "ReminderInfo"
			appointmentStorage.Mappings.ResourceId = "OwnerId"
			appointmentStorage.Mappings.Status = "Status"
			appointmentStorage.Mappings.Type = "EventType"

			appointmentStorage.CustomFieldMappings.Add(New DevExpress.Web.ASPxScheduler.ASPxAppointmentCustomFieldMapping("AppointmentCompany", "CompanyID"))
			appointmentStorage.CustomFieldMappings.Add(New DevExpress.Web.ASPxScheduler.ASPxAppointmentCustomFieldMapping("AppointmentContact", "ContactID"))
			Return appointmentStorage

		End Function

		Private Shared defaultResourceStorage_Renamed As MVCxResourceStorage
		Public Shared ReadOnly Property DefaultResourceStorage() As MVCxResourceStorage
			Get
				If defaultResourceStorage_Renamed Is Nothing Then
					defaultResourceStorage_Renamed = CreateDefaultResourceStorage()
				End If
				Return defaultResourceStorage_Renamed

			End Get
		End Property

		Private Shared Function CreateDefaultResourceStorage() As MVCxResourceStorage
			Dim resourceStorage As New MVCxResourceStorage()
			resourceStorage.Mappings.ResourceId = "ResID"
			resourceStorage.Mappings.Caption = "Name"
			resourceStorage.Mappings.Color = "Color"
			Return resourceStorage
		End Function

		Public Shared Function GetSchedulerSettings() As SchedulerSettings
			Dim settings As New SchedulerSettings()
			settings.Name = "scheduler"
			settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "SchedulerPartial"}
			settings.EditAppointmentRouteValues = New With {Key .Controller = "Home", Key .Action = "EditAppointment"}
			settings.CustomActionRouteValues = New With {Key .Controller = "Home", Key .Action = "CustomCallBackAction"}

			settings.Storage.Appointments.Assign(SchedulerDataHelper.DefaultAppointmentStorage)
			settings.Storage.Resources.Assign(SchedulerDataHelper.DefaultResourceStorage)

			settings.Storage.EnableReminders = True
			settings.GroupType = SchedulerGroupType.Resource
			settings.Views.DayView.Styles.ScrollAreaHeight = 400
			settings.Start = DateTime.Now

            Dim dialog = settings.OptionsForms.DialogLayoutSettings.AppointmentDialog
            dialog.ViewModel = New CustomAppointmentEditDialogViewModel()
            dialog.GenerateDefaultLayoutElements()

            Dim companies = dialog.LayoutElements.CreateField(Function(m As CustomAppointmentEditDialogViewModel) m.AppointmentCompany)
            Dim contacts = dialog.LayoutElements.CreateField(Function(m As CustomAppointmentEditDialogViewModel) m.AppointmentContact)
            dialog.InsertBefore(companies, dialog.FindLayoutElement("Description"))
            dialog.InsertAfter(contacts, companies)

            Return settings
		End Function

		Private Shared Sub Storage_ReminderAlert(ByVal sender As Object, ByVal e As ReminderEventArgs)

		End Sub

		Private Shared lastInsertedID As Integer = 0

		' CRUD operations implementation
		Public Shared Sub InsertAppointments(ByVal appts() As CustomAppointment)
			If appts.Length = 0 Then
				Return
			End If

			Dim appointmnets As List(Of CustomAppointment) = TryCast(HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
			For i As Integer = 0 To appts.Length - 1
				appts(i).ID = lastInsertedID
				lastInsertedID += 1
				appointmnets.Add(appts(i))
			Next i
		End Sub

		Public Shared Sub UpdateAppointments(ByVal appts() As CustomAppointment)
			If appts.Length = 0 Then
				Return
			End If

			Dim appointmnets As List(Of CustomAppointment) = TryCast(System.Web.HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
			For i As Integer = 0 To appts.Length - 1
                Dim sourceObject As CustomAppointment = appointmnets.First(Function(apt) apt.ID = appts(i).ID)
                appts(i).ID = sourceObject.ID
				appointmnets.Remove(sourceObject)
				appointmnets.Add(appts(i))
			Next i
		End Sub

		Public Shared Sub RemoveAppointments(ByVal appts() As CustomAppointment)
			If appts.Length = 0 Then
				Return
			End If

			Dim appointmnets As List(Of CustomAppointment) = TryCast(HttpContext.Current.Session("AppointmentsList"), List(Of CustomAppointment))
			For i As Integer = 0 To appts.Length - 1
                Dim sourceObject As CustomAppointment = appointmnets.First(Function(apt) apt.ID = appts(i).ID)
                appointmnets.Remove(sourceObject)
			Next i
		End Sub
	End Class
End Namespace