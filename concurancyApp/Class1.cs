﻿
namespace CurrencyConverter_Static
{
    internal class Rates //Make sure API return Value Name and where you want to store the name are the same. Like Get Response
    {
        public double GEL { get; set; }
        public double INR { get; set; }
        public double JPY { get; set; }
        public double USD { get; set; }
        public double NZD { get; set; }
        public double EUR { get; set; }
        public double CAD { get; set; }
        public double ISK { get; set; }
        public double PHP { get; set; }
        public double DKK { get; set; }
        public double CZK { get; set; }

    }
    internal class Root // This is the Main Class.API returns rates and it returns all currency name with value
    {
        public Rates rates { get; set; }
        public long timestamp;
        public string license;
    }
}