using CourseProjectDB.Clases;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    /// Логика взаимодействия для AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        AdminPanel AdminPanel;
        public AddCustomer( AdminPanel adminPanel)
        {
            InitializeComponent();
            AdminPanel = adminPanel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            AdminPanel.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int customer_Id;

            string org = Org.Text;
            string address = Adress.Text;
            string contact = Contact.Text;
            string login = Login.Text;
            string password = Password.Password;
            string repeatPassword = RepitPassword.Password;


            // Проверка на поля
            if (string.IsNullOrEmpty(org) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(contact) ||
                string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword))
            {
                // Один из полей пуст, выполните необходимые действия, например, выведите сообщение об ошибке
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (password != repeatPassword)
            {
                // Пароли не совпадают, выполните необходимые действия
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!IsPhoneNumberValid(contact))
            {
                MessageBox.Show("Неверный формат номера телефона.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Add To DB

            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAdmin;PASSWORD = AdminPass;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                // IsHaveSuchLogin

                using (OracleCommand command = new OracleCommand("CheckUserLoginExists", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Добавьте параметры процедуры
                    command.Parameters.Add("p_User_Login", OracleDbType.NVarchar2).Value = login; // Замените значением

                    OracleParameter userExistsParam = new OracleParameter("p_UserExists", OracleDbType.Int32);
                    userExistsParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(userExistsParam);

                    try
                    {
                        command.ExecuteNonQuery();

                        OracleDecimal oracleDecimalValueuserExists = (OracleDecimal)userExistsParam.Value;

                        int userExists = (int)oracleDecimalValueuserExists.Value;

                        if (userExists > 0)
                        {
                            MessageBox.Show("Логин уже занят.");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Логин не занят.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при выполнении процедуры проверки логина: " + ex.Message);
                    }
                }

                // addToCustomer
                using (OracleCommand command = new OracleCommand("AddCustomer", connection)) // AddCustomerToDB
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("p_Customer_Name", OracleDbType.NVarchar2).Value = org;
                    command.Parameters.Add("p_Customer_Address", OracleDbType.NVarchar2).Value = address;
                    command.Parameters.Add("p_Customer_Contact", OracleDbType.NVarchar2).Value = contact;

                    OracleParameter customer_Id_Param = new OracleParameter("p_Customer_Id", OracleDbType.Decimal);
                    customer_Id_Param.Direction = ParameterDirection.Output;
                    command.Parameters.Add(customer_Id_Param);

                    try
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Процедура выполнена успешно.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при выполнении процедуры: " + ex.Message);
                    }

                    // Получение значения Customer_Id после выполнения процедуры
                    OracleDecimal oracleDecimalValuecustomer_Id_Param = (OracleDecimal)customer_Id_Param.Value;

                    customer_Id = (int)oracleDecimalValuecustomer_Id_Param.Value;
                    Console.WriteLine($"Created Customer Id: {customer_Id}");
                    MessageBox.Show($"Created Customer Id: {customer_Id}");
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////

                // AddToUsers
                using (OracleCommand command = new OracleCommand("AddCustomerToUserRole", connection)) // AddCustomerToUserList
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("p_User_Role", OracleDbType.Int32).Value = 4;
                    command.Parameters.Add("p_User_MyId", OracleDbType.Int32).Value = customer_Id;
                    command.Parameters.Add("p_User_Login", OracleDbType.NVarchar2).Value = login;
                    command.Parameters.Add("p_User_Password", OracleDbType.NVarchar2).Value = password;

                    try
                    {
                        command.ExecuteNonQuery();

                        Jsoner.CreateLogAndPassJsonFile(login, password);

                        Console.WriteLine("Процедура выполнена успешно.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при выполнении процедуры: " + ex.Message);
                    }
                }
            }
        }

        private bool IsPhoneNumberValid(string phoneNumber)
        {
            // Регулярное выражение для проверки формата номера телефона
            string pattern = @"^\+?[0-9]{7,15}$";

            // Проверка совпадения с регулярным выражением
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
