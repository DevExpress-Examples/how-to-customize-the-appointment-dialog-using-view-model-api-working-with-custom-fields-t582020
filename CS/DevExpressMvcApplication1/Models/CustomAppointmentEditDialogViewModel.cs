using DevExpress.Web;
using DevExpress.Web.ASPxScheduler.Dialogs;
using DevExpress.Web.ASPxScheduler.Internal;
using DevExpressMvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXWebApplication1.Models {
    public class CustomAppointmentEditDialogViewModel : AppointmentEditDialogViewModel {

        [DialogFieldViewSettings(Caption = "Company", EditorType = DialogFieldEditorType.ComboBox)]
        public int AppointmentCompany { get; set; }
        [DialogFieldViewSettings(Caption = "Contact", EditorType = DialogFieldEditorType.ComboBox)]
        public int AppointmentContact { get; set; }

        public override void Load(AppointmentFormController appointmentController) {
            base.Load(appointmentController);

            SetEditorTypeFor(m => m.Subject, DialogFieldEditorType.ComboBox);
            SetDataItemsFor(m => m.Subject, (addItemDelegate) => {
                addItemDelegate("meeting", "meeting");
                addItemDelegate("travel", "travel");
                addItemDelegate("phone call", "phonecall");
            });

            List<Company> companies = Company.GenerateCompanyDataSource();
            SetDataItemsFor((CustomAppointmentEditDialogViewModel m) => m.AppointmentCompany, (addItemDelegate) => {
                foreach (Company comp in companies) {
                    addItemDelegate(comp.CompanyName, comp.CompanyID);
                }
            });

            SetDataItemsFor((CustomAppointmentEditDialogViewModel m) => m.AppointmentContact, (addItemDelegate) => {
                List<CompanyContact> contacts = CompanyContact.GenerateContactDataSource().Where(c => c.CompanyID == AppointmentCompany).ToList();
                addItemDelegate("", 0);
                foreach (CompanyContact cont in contacts) {
                    addItemDelegate(cont.ContactName, cont.ContactID);
                }
            });

            TrackPropertyChangeFor((CustomAppointmentEditDialogViewModel m) => m.AppointmentCompany, () => {
                AppointmentContact = 0;
            });

            TrackPropertyChangeFor(m => m.Subject, () => { });// just to process changing via server to show/hide company/contact
        }

        public override void SetDialogElementStateConditions() {
            base.SetDialogElementStateConditions();
            SetItemVisibilityCondition("Location", false);
            SetItemVisibilityCondition(vm => vm.IsAllDay, false);
            SetItemVisibilityCondition(vm => vm.Reminder, false);
            SetEditorEnabledCondition((CustomAppointmentEditDialogViewModel vm) => vm.AppointmentContact, AppointmentCompany > 0);
            SetItemVisibilityCondition((CustomAppointmentEditDialogViewModel vm) => vm.AppointmentContact, Subject != "phonecall");
            SetItemVisibilityCondition((CustomAppointmentEditDialogViewModel vm) => vm.AppointmentCompany, Subject != "phonecall");
        }
    }
}