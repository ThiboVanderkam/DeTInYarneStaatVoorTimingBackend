using LiteDB;
using DeTInYarneStaatVoorTiming.Models;

namespace DeTInYarneStaatVoorTiming.Data
{
    public class DataBase : IDataContext
    {
        private LiteDatabase _db = new LiteDatabase(@"data.db");
        private const string _APPOINTMENT = "Appointment";
        private const string _ACCOUNT = "Account";

        public IEnumerable<Appointment> GetAllAppointments()
        {
            return _db.GetCollection<Appointment>(_APPOINTMENT).FindAll();
        }

        public IEnumerable<Appointment> GetAppointmentsByDate(string date)
        {
            return _db.GetCollection<Appointment>(_APPOINTMENT).FindAll().Where(a => a.date == date);
        }

        public string AddAppointment(Appointment appointment)
        {
            List<Appointment> existingAppointments = _db.GetCollection<Appointment>(_APPOINTMENT).FindAll().Where(a => 
                a.activityTitle == appointment.activityTitle && 
                a.submitterName == appointment.submitterName && 
                a.date == appointment.date
                ).ToList();

            if (existingAppointments.Count > 0) return "already exists";
            _db.GetCollection<Appointment>(_APPOINTMENT).Insert(appointment);
            return "success";
        }

        public bool DeleteAppointmentByInput(string activityTitle, string submitterName)
        {
            Appointment appointment = _db.GetCollection<Appointment>(_APPOINTMENT).FindAll().Where(a =>
                a.activityTitle == activityTitle && a.submitterName == submitterName).ToList()[0];

            return _db.GetCollection<Appointment>(_APPOINTMENT).Delete(appointment.ID);
        }

        public bool DeleteAppointmentByDate(string date)
        {
            DateOnly deleteDate = DateOnly.Parse(date);
            List<Appointment> appointments = _db.GetCollection<Appointment>(_APPOINTMENT).FindAll().Where(a =>
                DateOnly.Parse(a.date) <= deleteDate).ToList();
            foreach (Appointment appointment in appointments)
            {
                _db.GetCollection<Appointment>(_APPOINTMENT).Delete(appointment.ID);
            }

            return appointments.Count > 0;
        }


        //Accounts
        public IEnumerable<Account> GetAllAccounts()
        {
            return _db.GetCollection<Account>(_ACCOUNT).FindAll().ToList();
        }

        public void AddAccount(Account account)
        {
            _db.GetCollection<Account>(_ACCOUNT).Insert(account);
        }

        public bool DeleteAccount(int id)
        {
            return _db.GetCollection<Account>(_ACCOUNT).Delete(id);
        }

        public Account GetAccountByRole(string role)
        {
            List<Account> temp = _db.GetCollection<Account>(_ACCOUNT).FindAll().Where(a => a.role == role).ToList();
            if (temp.Count <= 0) return null;
            return temp[0];

        }

    }
}
