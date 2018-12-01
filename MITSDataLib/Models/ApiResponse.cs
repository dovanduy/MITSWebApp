using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;

namespace MITSDataLib.Models
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public T Model { get; set; }
        public ModelStateDictionary ModelState { get; set; }
    }
}
