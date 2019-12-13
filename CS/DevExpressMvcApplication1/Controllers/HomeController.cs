using DevExpress.Web.Mvc;
using DevExpressMvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevExpressMvcApplication1.Controllers
{
    public class HomeController : Controller
    {
		// GET: Home
		public ActionResult Index() {
			return View(SchedulerDataHelper.DataObject);
		}

		public ActionResult SchedulerPartial() {
			return PartialView("SchedulerPartial", SchedulerDataHelper.DataObject);
		}

		public ActionResult EditAppointment() {
			UpdateAppointment();
			return PartialView("SchedulerPartial", SchedulerDataHelper.DataObject);
		}

		static void UpdateAppointment() {
			List<CustomAppointment> appointmnets = System.Web.HttpContext.Current.Session["AppointmentsList"] as List<CustomAppointment>;
			List<CustomResource> resources = System.Web.HttpContext.Current.Session["ResourcesList"] as List<CustomResource>;

			CustomAppointment[] insertedAppts = SchedulerExtension.GetAppointmentsToInsert<CustomAppointment>(SchedulerDataHelper.GetSchedulerSettings(), appointmnets, resources);
			SchedulerDataHelper.InsertAppointments(insertedAppts);


			CustomAppointment[] updatedAppts = SchedulerExtension.GetAppointmentsToUpdate<CustomAppointment>(SchedulerDataHelper.GetSchedulerSettings(), appointmnets, resources);
			SchedulerDataHelper.UpdateAppointments(updatedAppts);

			CustomAppointment[] removedAppts = SchedulerExtension.GetAppointmentsToRemove<CustomAppointment>(SchedulerDataHelper.GetSchedulerSettings(), appointmnets, resources);
			SchedulerDataHelper.RemoveAppointments(removedAppts);
		}
	}
}