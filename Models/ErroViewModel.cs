using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteMazzaFC.WebApi.Models
{
    public class ErroViewModel
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; }
    }
}