using System;
using System.Collections.Generic;

namespace WebApplication1.Data
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }

    public class JakubDedio
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }

    public class ToDoList
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    
}
