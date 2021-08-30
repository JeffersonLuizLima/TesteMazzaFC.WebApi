using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteMazzaFC.WebApi.Models
{
    public class CategoriaViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Categoria")]
        public string Descricao { get; set; }
    }
}