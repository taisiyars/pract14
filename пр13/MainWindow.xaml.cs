using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using libmas;
using System.IO;

namespace пр13
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int[,] matr;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
        }



        private void dgInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int rowCount = DataGridMatr.Items.Count;
            int columnCount = (matr != null) ? matr.GetLength(1) : 0;

            StatusTableSize.Text = $"Размер таблицы: {rowCount}x{columnCount}";
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime d = DateTime.Now;
            tbTime.Text = d.ToString("HH:mm:ss");
            tbData.Text = d.ToString("dd.MM.yyyy");
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            ArrayMatrix.Open(ref matr);
            DataGridMatr.ItemsSource = VisualArray.ToDataTable(matr).DefaultView;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ArrayMatrix.Save(matr);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дана целочисленная матрица размера M * N. \r\nНайти номер последней из ее строк,содержащих максимальное количество одинаковых элементов.");
        }

        private void btnFill_Click(object sender, RoutedEventArgs e)
        {
            bool f, c;
            int value, value1;

            f = Int32.TryParse(tbCols.Text, out value);
            c= Int32.TryParse(tbRows.Text, out value1);
            if (f && c == true)
            {
                ArrayMatrix.InitMas(out matr, value1, value);
                DataGridMatr.ItemsSource = VisualArray.ToDataTable(matr).DefaultView;
                tbRows.Clear();
                tbCols.Clear();
            }
            else
            {
                MessageBox.Show("введите корректные значения");
            }
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridMatr.ItemsSource != null)
            {
                tbRez.Text = LastRow(matr).ToString();
            }
            else MessageBox.Show("создайте таблицу");
        }

        int LastRow(int[,] matr)
        {
            int maxCount = 0;
            int lastRow = -1;

            //проходим по элементам
            for (int i = 0; i < matr.GetLength(0); i++)
            {
                for (int j = 0; j < matr.GetLength(1) - 1; j++)
                {
                    int count = 1; //счетчик для текущего элемента

                    //считаем кол-во текущего элемента в строке
                    for (int k = 0; k < matr.GetLength(1); k++)
                    {
                        //сравнение двух элементов
                        if (matr[i, j] == matr[i, k] && j != k)
                        {
                            count++;
                        }
                    }

                    //проверяем, является ли текущее кол-во максимальным
                    if (count > maxCount)
                    {
                        maxCount = count;
                        lastRow = i + 1; //обновляем индекс строки
                    }
                    else if (count == maxCount)
                    {
                        lastRow = i + 1; //в случае равенства обновляем на последнюю строку
                    }
                }
            }
            return lastRow;
        }

        private void CellEdit(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid != null)
            {
                int rowIndex = e.Row.GetIndex();
                int columnIndex = e.Column.DisplayIndex;

                TextBox editedTextbox = e.EditingElement as TextBox;
                if (editedTextbox != null)
                {
                    int newValue;
                    if (int.TryParse(editedTextbox.Text, out newValue))
                    {
                        matr[rowIndex, columnIndex] = newValue;
                    }
                    else
                    {
                        MessageBox.Show("введите корректное число");
                        editedTextbox.Text = matr[rowIndex, columnIndex].ToString();
                    }
                }
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Window1 pas = new Window1();
            pas.Owner = this;
            pas.ShowDialog();
        }

        private void btn_SettingsClick(object sender, RoutedEventArgs e)
        {
            Window2 settingsWindow = new Window2();
            if (settingsWindow.ShowDialog() == true)
            {
                int rows = settingsWindow.Rows;
                int cols = settingsWindow.Cols;

                tbRows.Text = rows.ToString();
                tbCols.Text = cols.ToString();
            }
        }

        private void LoadSettings()
        {
            if (File.Exists("config.ini"))
            {
                var content = File.ReadAllText("config.ini").Split(',');
                if (int.TryParse(content[0], out int rows) && int.TryParse(content[1], out int cols))
                {
                    tbRows.Text = rows.ToString();
                    tbCols.Text = cols.ToString();
                }
            }
        }
    }
}