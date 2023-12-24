using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace CourseProjectDB
{
    /// <summary>
    /// Логика взаимодействия для StoreHause.xaml
    /// </summary>
    public partial class StoreHause : Window
    {
        AdminPanel AdminPanel;
        public StoreHause(AdminPanel adminPanel)
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
            string number = Number.Text;
            string adress = Adress.Text;
            string copacity = Copacity.Text;
            string temperature = Temperature.Text;

            // проверка на поля
            if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(adress) || string.IsNullOrEmpty(copacity) ||
               string.IsNullOrEmpty(temperature))
            {
                // Один из полей пуст, выполните необходимые действия, например, выведите сообщение об ошибке
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (Convert.ToInt32(number) < 0)
            {
                MessageBox.Show("Номер склада не может быть отрицательным");
            }
            else if (Convert.ToInt32(temperature) < -30 || Convert.ToInt32(temperature) > 30)
            {
                MessageBox.Show("Температура склада не может быть выше 30 или ниже -30 грдусов Цельсия");
                return;
            }

            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAdmin;PASSWORD = AdminPass;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                try
                {

                    connection.Open();

                    using (OracleCommand command = new OracleCommand("AddStoreHouseIfNotExists", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_StoreHouse_Address", OracleDbType.NVarchar2).Value = adress;
                        command.Parameters.Add("p_StoreHouse_Number", OracleDbType.Int32).Value = Convert.ToInt32(number);
                        command.Parameters.Add("p_StoreHouse_Storage", OracleDbType.Int32).Value = Convert.ToInt32(copacity);
                        command.Parameters.Add("p_StoreHouse_Temperature", OracleDbType.Int32).Value = Convert.ToInt32(temperature);

                        OracleParameter resultParam = new OracleParameter("p_Result", OracleDbType.Int32);
                        resultParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultParam);

                        command.ExecuteNonQuery();

                        
                        OracleDecimal oracleDecimalValuestoreHouseId = (OracleDecimal)resultParam.Value;

                        int storeHouseId = (int)oracleDecimalValuestoreHouseId.Value;
                        if (storeHouseId == -1)
                        {
                            MessageBox.Show("Склад с данным номером уже существует");
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Склад построен!");
                        }
                    }




                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
