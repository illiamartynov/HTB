namespace HTB
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    public class Attempt
    {
        // Статический список для хранения всех попыток
        public static List<Attempt> Extent { get; set; } = new List<Attempt>();

        // Свойства попытки
        public int AttemptID { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Result { get; private set; }

        // Конструктор попытки
        public Attempt(int attemptID, DateTime timestamp, string result)
        {
            AttemptID = attemptID;
            Timestamp = timestamp;
            Result = result;

            // Добавляем объект в экстент
            Extent.Add(this);
        }

        // Сохранение экстента в файл
        public static void SaveExtent(string filename = "attempt_extent.json")
        {
            try
            {
                var json = JsonSerializer.Serialize(Extent);
                File.WriteAllText(filename, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving extent: {ex.Message}");
            }
        }

        // Загрузка экстента из файла
        public static void LoadExtent(string filename = "attempt_extent.json")
        {
            if (File.Exists(filename))
            {
                try
                {
                    var json = File.ReadAllText(filename);
                    var loadedExtent = JsonSerializer.Deserialize<List<Attempt>>(json);

                    // Очищаем текущий экстент и загружаем новые данные
                    Extent.Clear();
                    if (loadedExtent != null)
                    {
                        Extent.AddRange(loadedExtent);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading extent: {ex.Message}");
                }
            }
        }
    }
}
