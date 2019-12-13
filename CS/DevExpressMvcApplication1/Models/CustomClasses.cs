using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevExpressMvcApplication1.Models {
    public class CustomAppointment {
        private DateTime m_Start;
        private DateTime m_End;
        private string m_Subject;
        private int m_Status;
        private string m_Description;
        private int m_Label;
        private string m_Location;
        private bool m_Allday;
        private int m_EventType;
        private string m_RecurrenceInfo;
        private string m_ReminderInfo;
        private object m_OwnerId;
        private int m_Id;

        private int m_company_id;
        private int m_conact_id;


        public DateTime StartTime { get { return m_Start; } set { m_Start = value; } }
        public DateTime EndTime { get { return m_End; } set { m_End = value; } }
        public string Subject { get { return m_Subject; } set { m_Subject = value; } }
        public int Status { get { return m_Status; } set { m_Status = value; } }
        public string Description { get { return m_Description; } set { m_Description = value; } }
        public int Label { get { return m_Label; } set { m_Label = value; } }
        public string Location { get { return m_Location; } set { m_Location = value; } }
        public bool AllDay { get { return m_Allday; } set { m_Allday = value; } }
        public int EventType { get { return m_EventType; } set { m_EventType = value; } }
        public string RecurrenceInfo { get { return m_RecurrenceInfo; } set { m_RecurrenceInfo = value; } }
        public string ReminderInfo { get { return m_ReminderInfo; } set { m_ReminderInfo = value; } }
        public object OwnerId { get { return m_OwnerId; } set { m_OwnerId = value; } }
        public int ID { get { return m_Id; } set { m_Id = value; } }

        public int CompanyID { get { return m_company_id; } set { m_company_id = value; } }
        public int ContactID { get { return m_conact_id; } set { m_conact_id = value; } }

        public CustomAppointment() {
        }

        public static CustomAppointment CreateCustomAppointment(string subject, object resourceId, int status, int label, int id) {
            CustomAppointment apt = new CustomAppointment();
            apt.ID = id;
            apt.Subject = subject;
            apt.OwnerId = resourceId;
            apt.StartTime = DateTime.Now.AddHours(id);
            apt.EndTime = apt.StartTime.AddHours(3);
            apt.Status = status;
            apt.Label = label;
            return apt;
        }
    }
    public class CustomResource {
        private string m_name;
        private int m_res_id;
        private int m_res_color;

        public string Name { get { return m_name; } set { m_name = value; } }
        public int ResID { get { return m_res_id; } set { m_res_id = value; } }
        public int Color { get { return m_res_color; } set { m_res_color = value; } }

        public CustomResource() {

        }

        public static CustomResource CreateCustomResource(int res_id, string caption, int ResColor) {
            CustomResource cr = new CustomResource();
            cr.ResID = res_id;
            cr.Name = caption;
            cr.Color = ResColor;
            return cr;
        }

    }
    
    public class Company {
        private string m_company_name;
        private int m_company_id;

        public string CompanyName { get { return m_company_name; } set { m_company_name = value; } }
        public int CompanyID { get { return m_company_id; } set { m_company_id = value; } }

        public Company() {
        }

        public static List<Company> GenerateCompanyDataSource() {
            List<Company> returnedResult = new List<Company>();
            for (int i = 0; i < 10; i++) {
                returnedResult.Add(new Company() { CompanyID = i, CompanyName = "Company " + i.ToString() });
            }
            return returnedResult;
        }
    }

    public class CompanyContact {
        private string m_contact_name;
        private int m_contact_id;
        private int m_company_id;

        public string ContactName { get { return m_contact_name; } set { m_contact_name = value; } }
        public int ContactID { get { return m_contact_id; } set { m_contact_id = value; } }
        public int CompanyID { get { return m_company_id; } set { m_company_id = value; } }

        public CompanyContact() {
        }

        public static List<CompanyContact> GenerateContactDataSource() {
            List<CompanyContact> returnedResult = new List<CompanyContact>();
            List<Company> companies = Company.GenerateCompanyDataSource();

            int uniqueContactID = 0;
            for (int i = 0; i < companies.Count; i++) {
                for (int j = 0; j < 5; j++) {
                    returnedResult.Add(new CompanyContact() { CompanyID = i, ContactName = "Contact " + j.ToString() + ", Company " + i.ToString(), ContactID = uniqueContactID });
                    uniqueContactID++;
                }
            }
            return returnedResult;
        }
    }


    public class SchedulerDataObject {
        public List<CustomAppointment> Appointments { get; set; }
        public List<CustomResource> Resources { get; set; }
    }
}