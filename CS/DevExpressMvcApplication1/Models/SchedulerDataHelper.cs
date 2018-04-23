using System.Collections;
using System.Linq;
using DevExpressMvcApplication1;
using DevExpress.Web.Mvc;
using DevExpressMvcApplication1.Models;
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Web;
using DevExpress.XtraScheduler;
using DevExpress.Web.ASPxScheduler.Dialogs;

public class SchedulerDataHelper {
    public static List<CustomResource> GetResources() {
        List<CustomResource> resources = new List<CustomResource>();
        resources.Add(CustomResource.CreateCustomResource(1, "Max Fowler", Color.Yellow.ToArgb()));
        resources.Add(CustomResource.CreateCustomResource(2, "Nancy Drewmore", Color.Green.ToArgb()));
        resources.Add(CustomResource.CreateCustomResource(3, "Pak Jang", Color.LightPink.ToArgb()));
        return resources;
    }

    static Random myRand = new Random();
    public static List<CustomAppointment> GetAppointments(List<CustomResource> resources) {
        List<CustomAppointment> appointments = new List<CustomAppointment>();
        foreach(CustomResource item in resources) {
            string subjPrefix = item.Name + "'s ";
            appointments.Add(CustomAppointment.CreateCustomAppointment(subjPrefix + "meeting", item.ResID, 2, 5, lastInsertedID++));
            appointments.Add(CustomAppointment.CreateCustomAppointment(subjPrefix + "travel", item.ResID, 3, 6, lastInsertedID++));
            appointments.Add(CustomAppointment.CreateCustomAppointment(subjPrefix + "phone call", item.ResID, 0, 10, lastInsertedID++));
        }
        return appointments;
    }


    public static SchedulerDataObject DataObject {
        get {
            SchedulerDataObject dataObject = new SchedulerDataObject();


            if(HttpContext.Current.Session["ResourcesList"] == null) {
                HttpContext.Current.Session["ResourcesList"] = GetResources();
            }
            dataObject.Resources = HttpContext.Current.Session["ResourcesList"] as List<CustomResource>;

            if(HttpContext.Current.Session["AppointmentsList"] == null) {
                HttpContext.Current.Session["AppointmentsList"] = GetAppointments(dataObject.Resources);
            }
            dataObject.Appointments = HttpContext.Current.Session["AppointmentsList"] as List<CustomAppointment>;
            return dataObject;
        }
    }

    static MVCxAppointmentStorage defaultAppointmentStorage;
    public static MVCxAppointmentStorage DefaultAppointmentStorage {
        get {
            if(defaultAppointmentStorage == null) {
                defaultAppointmentStorage = CreateDefaultAppointmentStorage();
            }
            return defaultAppointmentStorage;

        }
    }

    static MVCxAppointmentStorage CreateDefaultAppointmentStorage() {
        MVCxAppointmentStorage appointmentStorage = new MVCxAppointmentStorage();
        appointmentStorage.AutoRetrieveId = true;
        appointmentStorage.Mappings.AppointmentId = "ID";
        appointmentStorage.Mappings.Start = "StartTime";
        appointmentStorage.Mappings.End = "EndTime";
        appointmentStorage.Mappings.Subject = "Subject";
        appointmentStorage.Mappings.AllDay = "AllDay";
        appointmentStorage.Mappings.Description = "Description";
        appointmentStorage.Mappings.Label = "Label";
        appointmentStorage.Mappings.Location = "Location";
        appointmentStorage.Mappings.RecurrenceInfo = "RecurrenceInfo";
        appointmentStorage.Mappings.ReminderInfo = "ReminderInfo";
        appointmentStorage.Mappings.ResourceId = "OwnerId";
        appointmentStorage.Mappings.Status = "Status";
        appointmentStorage.Mappings.Type = "EventType";

        appointmentStorage.CustomFieldMappings.Add(new DevExpress.Web.ASPxScheduler.ASPxAppointmentCustomFieldMapping("AppointmentCompany", "CompanyID"));
        appointmentStorage.CustomFieldMappings.Add(new DevExpress.Web.ASPxScheduler.ASPxAppointmentCustomFieldMapping("AppointmentContact", "ContactID")); return appointmentStorage;

    }

    static MVCxResourceStorage defaultResourceStorage;
    public static MVCxResourceStorage DefaultResourceStorage {
        get {
            if(defaultResourceStorage == null) {
                defaultResourceStorage = CreateDefaultResourceStorage();
            }
            return defaultResourceStorage;

        }
    }   

    static MVCxResourceStorage CreateDefaultResourceStorage() {
        MVCxResourceStorage resourceStorage = new MVCxResourceStorage();
        resourceStorage.Mappings.ResourceId = "ResID";
        resourceStorage.Mappings.Caption = "Name";
        resourceStorage.Mappings.Color = "Color";
        return resourceStorage;
    }

    public static SchedulerSettings GetSchedulerSettings() {
        SchedulerSettings settings = new SchedulerSettings();
        settings.Name = "scheduler";
        settings.CallbackRouteValues = new { Controller = "Home", Action = "SchedulerPartial" };
        settings.EditAppointmentRouteValues = new { Controller = "Home", Action = "EditAppointment" };
        settings.CustomActionRouteValues = new { Controller = "Home", Action = "CustomCallBackAction" };

        settings.Storage.Appointments.Assign(SchedulerDataHelper.DefaultAppointmentStorage);
        settings.Storage.Resources.Assign(SchedulerDataHelper.DefaultResourceStorage);

        settings.Storage.EnableReminders = true;
        settings.GroupType = SchedulerGroupType.Resource;
        settings.Views.DayView.Styles.ScrollAreaHeight = 400;
        settings.Start = DateTime.Now;

        var dialog = settings.OptionsForms.DialogLayoutSettings.AppointmentDialog;
        dialog.ViewModel = new CustomAppointmentEditDialogViewModel();
        dialog.GenerateDefaultLayoutElements();

        var companies = dialog.LayoutElements.CreateField((CustomAppointmentEditDialogViewModel m) => m.AppointmentCompany);
        var contacts = dialog.LayoutElements.CreateField((CustomAppointmentEditDialogViewModel m) => m.AppointmentContact);
        dialog.InsertBefore(companies, dialog.FindLayoutElement("Description"));
        dialog.InsertAfter(contacts, companies);

        return settings;
    }

    static void Storage_ReminderAlert(object sender, ReminderEventArgs e) {
        
    }

    static int lastInsertedID = 0;

    // CRUD operations implementation
    public static void InsertAppointments(CustomAppointment[] appts) {
        if(appts.Length == 0)
            return;

        List<CustomAppointment> appointmnets = HttpContext.Current.Session["AppointmentsList"] as List<CustomAppointment>;
        for(int i = 0; i < appts.Length; i++) {
            appts[i].ID = lastInsertedID++;
            appointmnets.Add(appts[i]);
        }
    }

    public static void UpdateAppointments(CustomAppointment[] appts) {
        if(appts.Length == 0)
            return;

        List<CustomAppointment> appointmnets = System.Web.HttpContext.Current.Session["AppointmentsList"] as List<CustomAppointment>;
        for(int i = 0; i < appts.Length; i++) {
            CustomAppointment sourceObject = appointmnets.First<CustomAppointment>(apt => apt.ID == appts[i].ID);
            appts[i].ID = sourceObject.ID;
            appointmnets.Remove(sourceObject);
            appointmnets.Add(appts[i]);
        }
    }

    public static void RemoveAppointments(CustomAppointment[] appts) {
        if(appts.Length == 0)
            return;

        List<CustomAppointment> appointmnets = HttpContext.Current.Session["AppointmentsList"] as List<CustomAppointment>;
        for(int i = 0; i < appts.Length; i++) {
            CustomAppointment sourceObject = appointmnets.First<CustomAppointment>(apt => apt.ID == appts[i].ID);
            appointmnets.Remove(sourceObject);
        }
    }
}