using System;
using Microsoft.AspNetCore.Http;

namespace Shepilov.Nikita._5H.Secondaweb.Models
{
    public class CreatePost
    {
        public IFormFile MyCSV {get; set;}
        public string Descrizione { get; set; }

    }
}