using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CourseProjectDB.Clases;
using CourseProjectDB.Windows;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;

namespace CourseProjectDB
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Jsoner.InpuoEmployeesToTable();
            //Jsoner.GeneratyEmployeesJsonFile();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустое значение в ComboBox
            if (Login.Text == null)
            {
                MessageBox.Show("Пожалуйста, выберите роль пользователя.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверка на пустое значение в PasswordBox
            if (string.IsNullOrEmpty(Password.Password))
            {
                MessageBox.Show("Пожалуйста, введите пароль.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получение логина
            TextBox selectedItem = (TextBox)Login;
            string User = selectedItem.Text.ToString();

            // Получение данных из PasswordBox
            string password = new NetworkCredential(string.Empty, Password.SecurePassword).Password;

            ConnectToDB(User, password);

        }

        private void ConnectToDB(string user, string password)
        {
            Console.WriteLine(user + " " + password);
            string ConnectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKProgram;PASSWORD = ProgramPass;";

            try
            {
                Int32 userRole, userMyId;
                OracleConnection connection = new OracleConnection(ConnectionString);

                connection.Open();

                if (connection.State == ConnectionState.Open)
                {

                    using (OracleCommand command = new OracleCommand("CHECKUSERLOGINPASSWORD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("p_Login", OracleDbType.NVarchar2).Value = user;
                        command.Parameters.Add("p_Password", OracleDbType.NVarchar2).Value = password;

                        OracleParameter userRoleParam = new OracleParameter("p_UserRole", OracleDbType.Int32);
                        userRoleParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(userRoleParam);

                        OracleParameter userMyIdParam = new OracleParameter("p_UserMyId", OracleDbType.Int32);
                        userMyIdParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(userMyIdParam);

                        command.ExecuteNonQuery();

                        OracleDecimal oracleDecimalValueuserRole = (OracleDecimal)userRoleParam.Value;
                        OracleDecimal oracleDecimalValueuserMyId = (OracleDecimal)userMyIdParam.Value;

                        userRole = oracleDecimalValueuserRole.ToInt32();
                        userMyId = oracleDecimalValueuserMyId.ToInt32();

                    }

                    connection.Close();

                    //Console.WriteLine(userRole + " " + userMyId);

                    switch (userRole)
                    {
                        case 1:

                            AdminPanel adminPanel = new AdminPanel(this);
                            adminPanel.Show();

                            this.Hide();
                            break;
                        case 2:
                            AnalyticWindow analytic = new AnalyticWindow(this);
                            analytic.Show();
                            this.Hide();
                            break;

                        case 4:
                            CustomerPanel customerPanel = new CustomerPanel(this, userMyId);
                            customerPanel.Show();
                            this.Hide();
                            break;

                        case 5:
                            SupplierPanel supplierPanel = new SupplierPanel(this, userMyId);
                            supplierPanel.Show();
                            this.Hide();
                            break;
                        default:
                            Console.WriteLine("Неизвестная роль!");
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("Ошибка соединения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка подключения к базе данных Oracle: " + ex.Message);
                MessageBox.Show("Неверный логин или пароль", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
