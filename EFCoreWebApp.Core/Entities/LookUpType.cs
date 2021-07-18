using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCoreWebApp.Core.Entities
{
    public enum LookUpType
    {
        State=0,
        Country=1
    }

    public class LookUp
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public LookUpType LookUpType { get; set; }
    }
}
