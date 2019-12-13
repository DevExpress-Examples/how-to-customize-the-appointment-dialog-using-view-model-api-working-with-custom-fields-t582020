Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace DevExpressMvcApplication1.Models
	Public Class CustomAppointment
		Private m_Start As DateTime
		Private m_End As DateTime
		Private m_Subject As String
		Private m_Status As Integer
		Private m_Description As String
		Private m_Label As Integer
		Private m_Location As String
		Private m_Allday As Boolean
		Private m_EventType As Integer
		Private m_RecurrenceInfo As String
		Private m_ReminderInfo As String
		Private m_OwnerId As Object
		Private m_Id As Integer

		Private m_company_id As Integer
		Private m_conact_id As Integer


		Public Property StartTime() As DateTime
			Get
				Return m_Start
			End Get
			Set(ByVal value As DateTime)
				m_Start = value
			End Set
		End Property
		Public Property EndTime() As DateTime
			Get
				Return m_End
			End Get
			Set(ByVal value As DateTime)
				m_End = value
			End Set
		End Property
		Public Property Subject() As String
			Get
				Return m_Subject
			End Get
			Set(ByVal value As String)
				m_Subject = value
			End Set
		End Property
		Public Property Status() As Integer
			Get
				Return m_Status
			End Get
			Set(ByVal value As Integer)
				m_Status = value
			End Set
		End Property
		Public Property Description() As String
			Get
				Return m_Description
			End Get
			Set(ByVal value As String)
				m_Description = value
			End Set
		End Property
		Public Property Label() As Integer
			Get
				Return m_Label
			End Get
			Set(ByVal value As Integer)
				m_Label = value
			End Set
		End Property
		Public Property Location() As String
			Get
				Return m_Location
			End Get
			Set(ByVal value As String)
				m_Location = value
			End Set
		End Property
		Public Property AllDay() As Boolean
			Get
				Return m_Allday
			End Get
			Set(ByVal value As Boolean)
				m_Allday = value
			End Set
		End Property
		Public Property EventType() As Integer
			Get
				Return m_EventType
			End Get
			Set(ByVal value As Integer)
				m_EventType = value
			End Set
		End Property
		Public Property RecurrenceInfo() As String
			Get
				Return m_RecurrenceInfo
			End Get
			Set(ByVal value As String)
				m_RecurrenceInfo = value
			End Set
		End Property
		Public Property ReminderInfo() As String
			Get
				Return m_ReminderInfo
			End Get
			Set(ByVal value As String)
				m_ReminderInfo = value
			End Set
		End Property
		Public Property OwnerId() As Object
			Get
				Return m_OwnerId
			End Get
			Set(ByVal value As Object)
				m_OwnerId = value
			End Set
		End Property
		Public Property ID() As Integer
			Get
				Return m_Id
			End Get
			Set(ByVal value As Integer)
				m_Id = value
			End Set
		End Property

		Public Property CompanyID() As Integer
			Get
				Return m_company_id
			End Get
			Set(ByVal value As Integer)
				m_company_id = value
			End Set
		End Property
		Public Property ContactID() As Integer
			Get
				Return m_conact_id
			End Get
			Set(ByVal value As Integer)
				m_conact_id = value
			End Set
		End Property

		Public Sub New()
		End Sub

		Public Shared Function CreateCustomAppointment(ByVal subject As String, ByVal resourceId As Object, ByVal status As Integer, ByVal label As Integer, ByVal id As Integer) As CustomAppointment
			Dim apt As New CustomAppointment()
			apt.ID = id
			apt.Subject = subject
			apt.OwnerId = resourceId
			apt.StartTime = DateTime.Now.AddHours(id)
			apt.EndTime = apt.StartTime.AddHours(3)
			apt.Status = status
			apt.Label = label
			Return apt
		End Function
	End Class
	Public Class CustomResource
		Private m_name As String
		Private m_res_id As Integer
		Private m_res_color As Integer

		Public Property Name() As String
			Get
				Return m_name
			End Get
			Set(ByVal value As String)
				m_name = value
			End Set
		End Property
		Public Property ResID() As Integer
			Get
				Return m_res_id
			End Get
			Set(ByVal value As Integer)
				m_res_id = value
			End Set
		End Property
		Public Property Color() As Integer
			Get
				Return m_res_color
			End Get
			Set(ByVal value As Integer)
				m_res_color = value
			End Set
		End Property

		Public Sub New()

		End Sub

		Public Shared Function CreateCustomResource(ByVal res_id As Integer, ByVal caption As String, ByVal ResColor As Integer) As CustomResource
			Dim cr As New CustomResource()
			cr.ResID = res_id
			cr.Name = caption
			cr.Color = ResColor
			Return cr
		End Function

	End Class

	Public Class Company
		Private m_company_name As String
		Private m_company_id As Integer

		Public Property CompanyName() As String
			Get
				Return m_company_name
			End Get
			Set(ByVal value As String)
				m_company_name = value
			End Set
		End Property
		Public Property CompanyID() As Integer
			Get
				Return m_company_id
			End Get
			Set(ByVal value As Integer)
				m_company_id = value
			End Set
		End Property

		Public Sub New()
		End Sub

		Public Shared Function GenerateCompanyDataSource() As List(Of Company)
			Dim returnedResult As New List(Of Company)()
			For i As Integer = 0 To 9
				returnedResult.Add(New Company() With {.CompanyID = i, .CompanyName = "Company " & i.ToString()})
			Next i
			Return returnedResult
		End Function
	End Class

	Public Class CompanyContact
		Private m_contact_name As String
		Private m_contact_id As Integer
		Private m_company_id As Integer

		Public Property ContactName() As String
			Get
				Return m_contact_name
			End Get
			Set(ByVal value As String)
				m_contact_name = value
			End Set
		End Property
		Public Property ContactID() As Integer
			Get
				Return m_contact_id
			End Get
			Set(ByVal value As Integer)
				m_contact_id = value
			End Set
		End Property
		Public Property CompanyID() As Integer
			Get
				Return m_company_id
			End Get
			Set(ByVal value As Integer)
				m_company_id = value
			End Set
		End Property

		Public Sub New()
		End Sub

		Public Shared Function GenerateContactDataSource() As List(Of CompanyContact)
			Dim returnedResult As New List(Of CompanyContact)()
			Dim companies As List(Of Company) = Company.GenerateCompanyDataSource()

			Dim uniqueContactID As Integer = 0
			For i As Integer = 0 To companies.Count - 1
				For j As Integer = 0 To 4
					returnedResult.Add(New CompanyContact() With {.CompanyID = i, .ContactName = "Contact " & j.ToString() & ", Company " & i.ToString(), .ContactID = uniqueContactID})
					uniqueContactID += 1
				Next j
			Next i
			Return returnedResult
		End Function
	End Class


	Public Class SchedulerDataObject
		Private privateAppointments As List(Of CustomAppointment)
		Public Property Appointments() As List(Of CustomAppointment)
			Get
				Return privateAppointments
			End Get
			Set(ByVal value As List(Of CustomAppointment))
				privateAppointments = value
			End Set
		End Property
		Private privateResources As List(Of CustomResource)
		Public Property Resources() As List(Of CustomResource)
			Get
				Return privateResources
			End Get
			Set(ByVal value As List(Of CustomResource))
				privateResources = value
			End Set
		End Property
	End Class
End Namespace