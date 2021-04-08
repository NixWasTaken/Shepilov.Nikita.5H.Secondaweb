using System;

namespace Shepilov.Nikita._5H.Secondaweb.Models
{
    public class Prenotazione {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public bool? Partecipazione { get; set; }
        public DateTime DataPrenotazione { get; set; } = DateTime.Now;

    }
}