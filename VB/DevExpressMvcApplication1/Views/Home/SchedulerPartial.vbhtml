@Imports DevExpressMvcApplication1.Models

@Html.DevExpress().Scheduler(SchedulerDataHelper.GetSchedulerSettings()).Bind(Model.Appointments, Model.Resources).GetHtml()
