using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using Oracle.ManagedDataAccess.Client;
using CourseProjectDB.Clases;

namespace CourseProjectDB.Windows
{
    /// <summary>
    /// Логика взаимодействия для AnalyticWindow.xaml
    /// </summary>
    public partial class AnalyticWindow : Window
    {
        MainWindow main;
        DataTable LastSucssesSelectData = null;
        public AnalyticWindow(MainWindow mainn)
        {
            InitializeComponent();
            main = mainn;
        }
        private void ExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = QueryTextBox.Text;

                string queryUpper = query.ToUpper();
                queryUpper = queryUpper.TrimStart();

                if (query.Length < 7)
                {
                    MessageBox.Show("неверный запрос");
                    return;
                }
                else if (queryUpper.Contains("UPDATE") || queryUpper.Contains("DELETE") || queryUpper.Contains("INSERT") ||
                    queryUpper.Contains("ALTER ") || queryUpper.Contains("CREATE") || queryUpper.Contains("DROP") ||
                    queryUpper.Contains("COMMIT") || queryUpper.Contains("GRANT") || queryUpper.Contains("REVOKE") ||
                    queryUpper.Contains("ROLLBACK") || queryUpper.Contains("SAVEPOINT") || queryUpper.Contains("DESCRIBE ") ||
                    queryUpper.Contains("BEGIN") || queryUpper.Contains("TRANSACTION") || queryUpper.Contains("TRIGGER") ||
                    queryUpper.Contains("LOOP") || queryUpper.Contains("FOR") || queryUpper.Contains("WHILE") ||
                    queryUpper.Contains("END") || queryUpper.Contains("IF") || queryUpper.Contains("ELSE") ||
                    queryUpper.Contains("EXCEPTION") || queryUpper.Contains("RAISE") || queryUpper.Contains("CASE") ||
                    queryUpper.Contains("OPEN") || queryUpper.Contains("FETCH") || queryUpper.Contains("CLOSE") || queryUpper.Contains(";")
                )
                {
                    MessageBox.Show("Разрешено использовать только select запросы, без использования ключевых слов для создания иных запросов или ;");
                    return;
                }
                else if (!queryUpper.StartsWith("SELECT"))
                {
                    MessageBox.Show("Неверный select запрос");
                    return;
                }

                DataTable resultTable = ExecuteSelectQuery(query);
                ResultDataGrid.ItemsSource = resultTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный select запрос");
            }
        }

        private DataTable ExecuteSelectQuery(string query)
        {
            DataTable resultTable0 = new DataTable();

            try
            {


                DataTable resultTable = new DataTable();

                string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAnalytic;PASSWORD = AnalyticPass;";

                using (OracleConnection connection = new OracleConnection(connectionString))
                {
                    connection.Open();

                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataReader reader = command.ExecuteReader())
                        {
                            // Динамически создаем столбцы на основе результата запроса
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                resultTable.Columns.Add(reader.GetName(i), typeof(string));
                            }

                            // Заполняем данные
                            while (reader.Read())
                            {
                                DataRow row = resultTable.NewRow();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    row[i] = reader[i];
                                }
                                resultTable.Rows.Add(row);
                            }
                        }
                    }
                }
                LastSucssesSelectData = resultTable;
                return resultTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный select запрос");
                LastSucssesSelectData = null;
                return resultTable0;
            }
        }

        private void ExecuteQuery1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable resultTable = ExecuteSelectQueryByButton("GetShipmentDetails");
                ResultDataGrid.ItemsSource = resultTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный select запрос");
            }
        }

        private void ExecuteQuery2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable resultTable = ExecuteSelectQueryByButton("GetArrivalDetails");
                ResultDataGrid.ItemsSource = resultTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный select запрос");
            }
        }

        private void ExecuteQuery3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable resultTable = ExecuteSelectQueryByButton("GetArrivalProductsDetails");
                ResultDataGrid.ItemsSource = resultTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный select запрос");
            }
        }

        private void ExecuteQuery4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable resultTable = ExecuteSelectQueryByButton("GetShipmentProductsDetails");
                ResultDataGrid.ItemsSource = resultTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неверный select запрос");
            }
        }

        private DataTable ExecuteSelectQueryByButton(string ProcedureName)
        {
            DataTable resultTable = new DataTable();

            string connectionString = "DATA SOURCE=DESKTOP-ET2RS6V:1521/tlkzykovdb.mshome.net;USER ID = TLKAnalytic;PASSWORD = AnalyticPass;";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                using (OracleCommand command = new OracleCommand(ProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметр для курсора OUT
                    OracleParameter cursorParameter = new OracleParameter("p_Cursor", OracleDbType.RefCursor);
                    cursorParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(cursorParameter);

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        // Динамически создаем столбцы на основе результата запроса
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            resultTable.Columns.Add(reader.GetName(i), typeof(string));
                        }

                        // Заполняем данные
                        while (reader.Read())
                        {
                            DataRow row = resultTable.NewRow();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[i] = reader[i];
                            }
                            resultTable.Rows.Add(row);
                        }
                    }
                }
            }
            LastSucssesSelectData = resultTable;
            return resultTable;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            main.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Jsoner.InputAnalyzToJson(LastSucssesSelectData);
        }
    }
}