using NotPad.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotPad.Core.Infrastructure
{
    public class BaseEntity : IBaseEntity, IDeletable
    {
        [Key]
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
    }

    public interface IBaseEntity
    {
        int Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
