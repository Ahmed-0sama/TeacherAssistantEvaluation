using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class UserDataDto
    {
            [Required(ErrorMessage = "Requird")]
            public string Name { get; set; } = string.Empty;

            [Required(ErrorMessage = "Requird")]
            public int EmployeeNumber { get; set; }

            [Required(ErrorMessage = "Requird")]
            public string Qualification { get; set; } = string.Empty;

            [Required(ErrorMessage = "Requird")]
            public string Department { get; set; } = string.Empty;
            [Required(ErrorMessage = "Requird")]
            public string College { get; set; } = string.Empty;
            public string status { get; set; } = string.Empty;
    }
}
