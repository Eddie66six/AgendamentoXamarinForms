using System;

namespace AgendamentoXamarinForms.Models
{
    public class Errors
    {
        public Error[] errors { get; set; }
    }

    public class Error
    {
        public string key { get; set; }
        public string value { get; set; }
        public DateTime date { get; set; }
    }
}
