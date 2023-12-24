using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using CourseProjectDB.Clases;

namespace CourseProjectDB
{
    /// <summary>
    /// Логика взаимодействия для AddSupplier.xaml
    /// </summary>
    public partial class AddSupplier : Window
    {
        AdminPanel AdminPanel;
        public AddSupplier(AdminPanel adminPanel)
        {
            InitializeComponent();
            AdminPanel = adminPanel;
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            this.Close();
            AdminPanel.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int supplierId = 0;
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
                    command.Parameters.Add("p_User_Login", OracleDbType.NVarchar2).Value = login;

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

                // AddSupplier

                using (OracleCommand command = new OracleCommand("AddSupplier", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.Add("p_Supplier_Name", OracleDbType.NVarchar2).Value = org;
                    command.Parameters.Add("p_Supplier_Address", OracleDbType.NVarchar2).Value = address; 
                    command.Parameters.Add("p_Supplier_Contact", OracleDbType.NVarchar2).Value = contact; 

                    OracleParameter supplierIdParam = new OracleParameter("p_Supplier_Id", OracleDbType.Decimal);
                    supplierIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(supplierIdParam);

                    try
                    {
                        command.ExecuteNonQuery();

                        OracleDecimal oracleDecimalValuesupplierId = (OracleDecimal)supplierIdParam.Value;

                        supplierId = (int)oracleDecimalValuesupplierId.Value;

                        //MessageBox.Show($"ID созданного постафщика: {supplierId}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ошибка при выполнении процедуры: " + ex.Message);
                    }
                }

                // AddToUsers
                using (OracleCommand command = new OracleCommand("AddCustomerToUserRole", connection)) // AddSupplierToUserList
                {
                    try
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add("p_User_Role", OracleDbType.Int32).Value = 5;
                        if (supplierId == 0) throw new Exception("Ошибка, поставщик не был добавлен id = 0");
                        command.Parameters.Add("p_User_MyId", OracleDbType.Int32).Value = supplierId;
                        command.Parameters.Add("p_User_Login", OracleDbType.NVarchar2).Value = login;
                        command.Parameters.Add("p_User_Password", OracleDbType.NVarchar2).Value = password;

                        command.ExecuteNonQuery();

                        Jsoner.CreateLogAndPassJsonFile(login, password);
                        MessageBox.Show($"ID созданного постафщика: {supplierId}");
                        Console.WriteLine("Процедура выполнена успешно.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при выполнении процедуры: " + ex.Message);
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
