using NotPad.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotPad.Core
{
    public class Note : BaseEntity
    {
        [Required]
        public string Content { get; set; }
    }
}
