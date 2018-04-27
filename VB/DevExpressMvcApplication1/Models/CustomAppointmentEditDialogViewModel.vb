Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports DevExpress.Web.ASPxScheduler.Dialogs
Imports DevExpress.Web.ASPxScheduler.Internal

Namespace DevExpressMvcApplication1.Models
    Public Class CustomAppointmentEditDialogViewModel
        Inherits AppointmentEditDialogViewModel

        <DialogFieldViewSettings(Caption := "Company", EditorType := DialogFieldEditorType.ComboBox)> _
        Public Property AppointmentCompany() As Integer
        <DialogFieldViewSettings(Caption := "Contact", EditorType := DialogFieldEditorType.ComboBox)> _
        Public Property AppointmentContact() As Integer

        Public Overrides Sub Load(ByVal appointmentController As AppointmentFormController)
            MyBase.Load(appointmentController)

            SetEditorTypeFor(Function(m) m.Subject, DialogFieldEditorType.ComboBox)
            SetDataItemsFor(Function(m) m.Subject, Sub(addItemDelegate)
                addItemDelegate("meeting", "meeting")
                addItemDelegate("travel", "travel")
                addItemDelegate("phone call", "phonecall")
            End Sub)

            Dim companies As List(Of Company) = Company.GenerateCompanyDataSource()
            SetDataItemsFor(Function(m As CustomAppointmentEditDialogViewModel) m.AppointmentCompany, Sub(addItemDelegate)
                For Each comp As Company In companies
                    addItemDelegate(comp.CompanyName, comp.CompanyID)
                Next comp
            End Sub)

            SetDataItemsFor(Function(m As CustomAppointmentEditDialogViewModel) m.AppointmentContact, Sub(addItemDelegate)
                Dim contacts As List(Of CompanyContact) = CompanyContact.GenerateContactDataSource().Where(Function(c) c.CompanyID = AppointmentCompany).ToList()
                addItemDelegate("", 0)
                For Each cont As CompanyContact In contacts
                    addItemDelegate(cont.ContactName, cont.ContactID)
                Next cont
            End Sub)

            TrackPropertyChangeFor(Function(m As CustomAppointmentEditDialogViewModel) m.AppointmentCompany, Sub()
                AppointmentContact = 0
            End Sub)

            TrackPropertyChangeFor(Function(m) m.Subject, Sub()
            End Sub); ' just to process changing via server to show/hide company/contact
        End Sub

        Public Overrides Sub SetDialogElementStateConditions()
            MyBase.SetDialogElementStateConditions()
            SetItemVisibilityCondition("Location", False)
            SetItemVisibilityCondition(Function(vm) vm.IsAllDay, False)
            SetItemVisibilityCondition(Function(vm) vm.Reminder, False)
            SetEditorEnabledCondition(Function(vm As CustomAppointmentEditDialogViewModel) vm.AppointmentContact, AppointmentCompany > 0)
            SetItemVisibilityCondition(Function(vm As CustomAppointmentEditDialogViewModel) vm.AppointmentContact, Subject = "phonecall")
            SetItemVisibilityCondition(Function(vm As CustomAppointmentEditDialogViewModel) vm.AppointmentCompany, Subject = "phonecall")
        End Sub
    End Class
End Namespace