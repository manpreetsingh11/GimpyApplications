//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GimpySoftwareNewTemplate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CampaignActivity
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Note { get; set; }
        public Nullable<int> StatusId { get; set; }
        public int CompanyId { get; set; }
        public int CampaignId { get; set; }
    
        public virtual Campaign Campaign { get; set; }
        public virtual Company Company { get; set; }
        public virtual Status Status { get; set; }
    }
}
