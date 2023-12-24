using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace CourseProjectDB.Windows
{
    /// <summary>
    /// Логика взаимодействия для DelFacts.xaml
    /// </summary>
    public partial class DelFacts : Window
    {
        AdminPanel AdminPanel;
        public DelFacts(AdminPanel main)
        {
            this.AdminPanel = main;
            InitializeComponent();
            LoadComboBoxItems();
        }

        private void LoadComboBoxItems()
        {
            // Создание коллекции элементов, которые вы хотите добавить
            List<string> items = new List<string> { "Список заказов", "Список поставок" };

            // Добавление элементов в ComboBox
            foreach (var item in items)
            {
                Combobox.Items.Add(item);
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAdmin;PASSWORD = AdminPass;"; // Замените на свою строку подключения

            if (Combobox.SelectedValue.ToString() == "Список заказов")
            {

                try
                {
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        connection.Open();

                        using (OracleCommand command = new OracleCommand("GetFactShipmentIds", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            OracleParameter cursorParam = command.Parameters.Add("CursorShipmentId", OracleDbType.RefCursor);
                            cursorParam.Direction = ParameterDirection.Output;

                            using (OracleDataReader reader = command.ExecuteReader())
                            {
                                List<int> shipmentIds = new List<int>();

                                while (reader.Read())
                                {
                                    int shipmentId = reader.GetInt32(0);
                                    shipmentIds.Add(shipmentId);
                                }

                                shipmentComboBox.ItemsSource = shipmentIds;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось обнаружить записи о заказах/поставках");
                }
            }
            else
            {
                try
                {
                    using (OracleConnection connection = new OracleConnection(connectionString))
                    {
                        connection.Open();

                        using (OracleCommand command = new OracleCommand("GetFactArivalIds", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            OracleParameter cursorParam = command.Parameters.Add("CursorArivalId", OracleDbType.RefCursor);
                            cursorParam.Direction = ParameterDirection.Output;

                            using (OracleDataReader reader = command.ExecuteReader())
                            {
                                List<int> shipmentIds = new List<int>();

                                while (reader.Read())
                                {
                                    int shipmentId = reader.GetInt32(0);
                                    shipmentIds.Add(shipmentId);
                                }

                                shipmentComboBox.ItemsSource = shipmentIds;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось обнаружить записи о заказах/поставках");
                }
            }
        }

        private void Button_ClickBack(object sender, RoutedEventArgs e)
        {
            this.Close();
            AdminPanel.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAdmin;PASSWORD = AdminPass;"; // Замените на свою строку подключения

            if (Combobox.SelectedValue.ToString() == "Список заказов")
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            connection.Open();

                            // Создаем команду для вызова процедуры
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "DeleteShipmentAndProduct";

                            // Получаем выбранное значение из ComboBox
                            ComboBox comboBox = shipmentComboBox;
                            if (comboBox.SelectedItem != null)
                            {
                                int arrivalIdToDelete = (int)comboBox.SelectedItem;

                                // Создаем параметр для передачи Arrival_Id в процедуру
                                OracleParameter arrivalIdParameter = new OracleParameter("p_Shipment_Id", OracleDbType.Int32);
                                arrivalIdParameter.Value = arrivalIdToDelete;
                                command.Parameters.Add(arrivalIdParameter);

                                // Выполняем процедуру
                                command.ExecuteNonQuery();
                                MessageBox.Show("Процедура успешно выполнена.");
                            }
                            else
                            {
                                MessageBox.Show("Выберите Id из удаляемой поставки/заказа");
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    using (OracleCommand command = connection.CreateCommand())
                    {
                        try
                        {
                            connection.Open();

                            // Создаем команду для вызова процедуры
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "DeleteArrivalAndProduct";

                            // Получаем выбранное значение из ComboBox
                            ComboBox comboBox = shipmentComboBox;
                            if (comboBox.SelectedItem != null)
                            {
                                int arrivalIdToDelete = (int)comboBox.SelectedItem;

                                // Создаем параметр для передачи Arrival_Id в процедуру
                                OracleParameter arrivalIdParameter = new OracleParameter("p_Arrival_Id", OracleDbType.Int32);
                                arrivalIdParameter.Value = arrivalIdToDelete;
                                command.Parameters.Add(arrivalIdParameter);

                                // Выполняем процедуру
                                command.ExecuteNonQuery();
                                MessageBox.Show("Процедура успешно выполнена.");
                            }
                            else
                            {
                                MessageBox.Show("Выберите Id из удаляемой поставки/заказа");
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Ошибка: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
}
