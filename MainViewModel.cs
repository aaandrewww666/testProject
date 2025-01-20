using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CsvHelper;
using ExcelDataReader;
using Microsoft.VisualBasic.FileIO;

namespace testProject
{
    public class MainViewModel
    {
        public ObservableCollection<ObjectData> Objects { get; set; }

        private ObjectData? _selectedObject;
        public ObjectData? SelectedObject
        {
            get => _selectedObject;
            set
            {
                _selectedObject = value;
                OnPropertyChanged(nameof(SelectedObject));
            }
        }

        public MainViewModel() => Objects = [];

        public void LoadExcelFile(string filePath)
        {

            Objects.Clear();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        if (reader.Depth == 0) continue; // пропускаем заголовок

                        var ObjectData = new ObjectData
                        {
                            Name = reader.GetString(0),
                            Distance = reader.GetDouble(1),
                            Angle = reader.GetDouble(2),
                            Width = reader.GetDouble(3),
                            Heigth = reader.GetDouble(4),
                            IsDefect = reader.GetString(5).ToLower() == "yes"
                        };

                        Objects.Add(ObjectData);

                    }
                }
            }
        }

        public void LoadCsvFile(string filePath)
        {
            Objects.Clear();

            TextFieldParser parser = new(filePath)
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(";");
            while (!parser.EndOfData)
            {
                // Обработка строки
                var myDouble = 0.0;
                string[]? fields = parser.ReadFields();
                if (fields != null)
                {
                    if (fields[0].StartsWith("Name"))
                    {
                        //var text = "nonon";
                    } else
                    {
                        var ObjectData = new ObjectData
                        {
                            Name = fields[0],
                            Distance = Double.TryParse(fields[1], out myDouble) ? double.Parse(String.Format("{0:0.0}", myDouble)) : 0,
                            Angle = Double.TryParse(fields[2], out myDouble) ? double.Parse(String.Format("{0:0.0}", myDouble)) : 0,
                            Width = Double.TryParse(fields[3], out myDouble) ? double.Parse(String.Format("{0:0.0}", myDouble)) : 0,
                            Heigth = Double.TryParse(fields[4], out myDouble) ? double.Parse(String.Format("{0:0.0}", myDouble)) : 0,
                            IsDefect = fields[5] == "yes"
                        };

                        Objects.Add(ObjectData);
                    }
                }
            }
            parser.Close();

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
