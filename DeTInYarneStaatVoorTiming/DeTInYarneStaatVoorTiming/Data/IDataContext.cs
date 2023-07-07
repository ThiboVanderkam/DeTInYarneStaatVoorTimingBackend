using DeTInYarneStaatVoorTiming.Models;

namespace DeTInYarneStaatVoorTiming.Data
{
    public interface IDataContext
    {
        //Appointment
        public IEnumerable<Appointment> GetAllAppointments();
        public IEnumerable<Appointment> GetAppointmentsByDate(string date);
        public string AddAppointment(Appointment appointment);
        public bool DeleteAppointmentByInput(string appointmentTitle, string submitterName);
        public bool DeleteAppointmentByDate(string date);

        //Account
        public IEnumerable<Account> GetAllAccounts();
        public void AddAccount(Account account);
        public bool DeleteAccount(int id);
        public Account GetAccountByRole(string role);
    }
}
