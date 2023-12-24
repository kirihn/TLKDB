using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseProjectDB
{
    /// <summary>
    /// Логика взаимодействия для AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        AdminPanel AdminPanel;
        public AddEmployee(AdminPanel adminPanel)
        {
            InitializeComponent();
            this.AdminPanel = adminPanel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AdminPanel.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string dolzhnost = Dolzhnost.Text;
            string employeeName = EmployeeName.Text;
            string contact = Contact.Text;

            if (string.IsNullOrEmpty(dolzhnost) || string.IsNullOrEmpty(employeeName) || string.IsNullOrEmpty(contact))
            {
                // Один из полей пуст, выполните необходимые действия, например, выведите сообщение об ошибке
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!IsPhoneNumberValid(contact))
            {
                MessageBox.Show("Неверный формат номера телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAdmin;PASSWORD = AdminPass;";

            try
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand("AddEmployee", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_Employee_Name", OracleDbType.NVarchar2).Value = employeeName;
                        command.Parameters.Add("p_Employee_Role", OracleDbType.NVarchar2).Value = dolzhnost;
                        command.Parameters.Add("p_Employee_Contact", OracleDbType.NVarchar2).Value = contact;


                        command.ExecuteNonQuery();


                        MessageBox.Show("Работник добавлен");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении процедуры: {ex.Message}");
            }
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            // Регулярное выражение для проверки формата номера телефона
            string pattern = @"^\+?[0-9]{7,13}$";

            // Проверка совпадения с регулярным выражением
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
