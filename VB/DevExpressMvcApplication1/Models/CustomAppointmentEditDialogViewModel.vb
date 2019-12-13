Imports Microsoft.VisualBasic
Imports DevExpress.Web
Imports DevExpress.Web.ASPxScheduler.Dialogs
Imports DevExpress.Web.ASPxScheduler.Internal
Imports DevExpressMvcApplication1.Models
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace DXWebApplication1.Models
	Public Class CustomAppointmentEditDialogViewModel
		Inherits AppointmentEditDialogViewModel

        Private privateAppointmentCompany As Integer
        <DialogFieldViewSettings(Caption:="Company", EditorType:=DialogFieldEditorType.ComboBox)>
        Public Property AppointmentCompany() As Integer
            Get
                Return privateAppointmentCompany
            End Get
            Set(ByVal value As Integer)
                privateAppointmentCompany = value
            End Set
        End Property
        Private privateAppointmentContact As Integer
        <DialogFieldViewSettings(Caption:="Contact", EditorType:=DialogFieldEditorType.ComboBox)>
        Public Property AppointmentContact() As Integer
            Get
                Return privateAppointmentContact
            End Get
            Set(ByVal value As Integer)
                privateAppointmentContact = value
            End Set
        End Property

        Public Overrides Sub Load(ByVal appointmentController As AppointmentFormController)
            MyBase.Load(appointmentController)

            SetEditorTypeFor(Function(m) m.Subject, DialogFieldEditorType.ComboBox)
            SetDataItemsFor(Function(m) m.Subject, Sub(addItemDelegate)
                                                       addItemDelegate("meeting", "meeting")
                                                       addItemDelegate("travel", "travel")
                                                       addItemDelegate("phone call", "phonecall")
                                                   End Sub)


            SetDataItemsFor(Function(m) CType(m, CustomAppointmentEditDialogViewModel).AppointmentCompany, Sub(addItemDelegate)
                                                                                                               Dim companies As List(Of Company) = Company.GenerateCompanyDataSource()
                                                                                                               For Each comp As Company In companies
                                                                                                                   addItemDelegate(comp.CompanyName, comp.CompanyID)
                                                                                                               Next comp
                                                                                                           End Sub)

            SetDataItemsFor(Function(m) CType(m, CustomAppointmentEditDialogViewModel).AppointmentContact, Sub(addItemDelegate)
                                                                                                               Dim contacts As List(Of CompanyContact) = CompanyContact.GenerateContactDataSource().Where(Function(c) c.CompanyID = AppointmentCompany).ToList()
                                                                                                               addItemDelegate("", 0)
                                                                                                               For Each cont As CompanyContact In contacts
                                                                                                                   addItemDelegate(cont.ContactName, cont.ContactID)
                                                                                                               Next cont
                                                                                                           End Sub)

            TrackPropertyChangeFor(Function(m) CType(m, CustomAppointmentEditDialogViewModel).AppointmentCompany, Sub()
                                                                                                                      AppointmentContact = 0
                                                                                                                  End Sub)

            TrackPropertyChangeFor(Function(m) m.Subject, Sub()
                                                          End Sub)
        End Sub

        Public Overrides Sub SetDialogElementStateConditions()
            MyBase.SetDialogElementStateConditions()
            SetItemVisibilityCondition("Location", False)
            SetItemVisibilityCondition(Function(vm) vm.IsAllDay, False)
            SetItemVisibilityCondition(Function(vm) vm.Reminder, False)
            SetEditorEnabledCondition(Function(vm As CustomAppointmentEditDialogViewModel) vm.AppointmentContact, AppointmentCompany > 0)
            SetItemVisibilityCondition(Function(vm As CustomAppointmentEditDialogViewModel) vm.AppointmentContact, Subject <> "phonecall")
            SetItemVisibilityCondition(Function(vm As CustomAppointmentEditDialogViewModel) vm.AppointmentCompany, Subject <> "phonecall")
        End Sub
    End Class
End Namespace