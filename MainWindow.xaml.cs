using System;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace testProject
{
    public partial class MainWindow : Window
    {

        static int upgrade = 20;
        MainViewModel viewModel;
        public MainWindow()
        {
            viewModel = new MainViewModel();
            InitializeComponent();
        }
        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.txt;*.csv;*.xls;*.xlsx;*.xlsm)|*.txt;*.csv;*.xls;*.xlsx;*.xlsm|Text Files (*.txt;*.csv)|*.txt;*.csv|Excel Files|*.xls;*.xlsx;*.xlsm",
                Title = "Select a Data File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName.Contains(".csv"))
                {
                    viewModel.LoadCsvFile(openFileDialog.FileName);
                } else
                {
                    viewModel.LoadExcelFile(openFileDialog.FileName);
                }
                ObjectList.Items.Clear();
                DrawingCanvas.Children.Clear();
                DataGrid.ItemsSource = viewModel.Objects;
                DrawAllObjects();

            }
        }
        private void DrawObject(ObjectData obj)
        {
            DrawingCanvas.Children.Clear();
            var rect1 = new System.Windows.Shapes.Rectangle
            {
                Width = 20 * upgrade,
                Height = 12 * upgrade,
                Fill = Brushes.Gray
            };

            var rect2 = new System.Windows.Shapes.Rectangle
            {
                
                Width = obj.Width * upgrade,
                Height = obj.Heigth * upgrade,
                Fill = obj.IsDefect ? Brushes.Red : Brushes.Green
            };
            Canvas.SetLeft(rect2, obj.Distance*upgrade);
            Canvas.SetTop(rect2, obj.Angle * upgrade);


            DrawingCanvas.Children.Add(rect1);
            DrawingCanvas.Children.Add(rect2);
            DrawingCanvas.UpdateLayout();
        }

        private void DrawAllObjects()
        {
            DrawingCanvasAll.Children.Clear();
            var rectMain = new System.Windows.Shapes.Rectangle
            {
                Width = 20 * upgrade,
                Height = 12 * upgrade,
                Fill = Brushes.Gray
            };
            DrawingCanvasAll.Children.Add(rectMain);

            foreach (ObjectData obj in viewModel.Objects)
            {
                var rect = new System.Windows.Shapes.Rectangle
                {
                    Width = obj.Width * upgrade,
                    Height = obj.Heigth * upgrade,
                    Fill = obj.IsDefect ? Brushes.Red : Brushes.Green
                };
                Canvas.SetLeft(rect, obj.Distance * upgrade);
                Canvas.SetTop(rect, obj.Angle * upgrade);

                DrawingCanvasAll.Children.Add(rect);
            }
        }

        private void AddObject (ObjectData obj)
        {
            ObjectList.Items.Add(obj.Name);
            ObjectList.Items.Add(obj.Distance);
            ObjectList.Items.Add(obj.Angle);
            ObjectList.Items.Add(obj.Width);
            ObjectList.Items.Add(obj.Heigth);
            if(obj.IsDefect) {
                ObjectList.Items.Add("yes"); 
            } 
            else { 
                ObjectList.Items.Add("no"); 
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null && dataGrid.SelectedItem != null)
            {
                var selectedItem = dataGrid.SelectedItem as ObjectData;
                if (selectedItem != null)
                {
                    ObjectList.Items.Clear();
                    AddObject(selectedItem);
                    DrawObject(selectedItem);
                    DrawAllObjects();
                }
            }
            else
            {
                if (viewModel.Objects.Count > 0)
                {
                    MessageBox.Show("Для добавления объекта введите данные по столбцам");
                }
            }               
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                
                var updatedData = (ObjectData)DataGrid.SelectedItem;
                string newValue;
                // Обновляем соответствующее свойство выбранного объекта
                if (updatedData != null)
                {
                    // Определяем, какая ячейка была изменена
                    if (e.Column.Header.ToString() == "Название")
                    {
                        newValue = ((TextBox)e.EditingElement).Text;
                        updatedData.Name = newValue;
                    }
                    else if (e.Column.Header.ToString() == "Горизонтальная координата")
                    {
                        newValue = ((TextBox)e.EditingElement).Text;
                        if (double.TryParse(newValue, out double doubleValue))
                        {
                            updatedData.Distance = doubleValue;
                        }
                    }
                    else if (e.Column.Header.ToString() == "Вертикальная координата")
                    {
                        newValue = ((TextBox)e.EditingElement).Text;
                        if (double.TryParse(newValue, out double doubleValue))
                        {
                            updatedData.Angle = doubleValue;
                        }
                    }
                    else if (e.Column.Header.ToString() == "Горизонтальный размер")
                    {
                        newValue = ((TextBox)e.EditingElement).Text;
                        if (double.TryParse(newValue, out double doubleValue))
                        {
                            updatedData.Width = doubleValue;
                        }
                    }
                    else if (e.Column.Header.ToString() == "Вертикальный размер")
                    {
                        newValue = ((TextBox)e.EditingElement).Text;
                        if (double.TryParse(newValue, out double doubleValue))
                        {
                            updatedData.Heigth = doubleValue;
                        }
                    }
                    else if (e.Column.Header.ToString() == "Дефект")
                    {
                        if(((CheckBox)e.EditingElement).IsChecked == true)
                        {
                            updatedData.IsDefect = true;
                        } else
                        {
                            updatedData.IsDefect = false;
                        }
                    }

                    ObjectList.Items.Clear();
                    AddObject(updatedData);
                    ObjectList.UpdateLayout();
                    DrawObject(updatedData);
                    DrawAllObjects();
                }
            }
            
        }
    }
}