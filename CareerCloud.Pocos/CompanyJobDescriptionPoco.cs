using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CareerCloud.Pocos
{
    [Table("Company_Jobs_Descriptions")]
    public class CompanyJobDescriptionPoco : IPoco
    {
        [Key]
        public Guid Id { set; get; }
        public Guid Job { set; get; }
        [Column("Job_Name")]
        public string JobName { set; get; }
        [Column("Job_Descriptions")]
        public string JobDescriptions { set; get; }
        [Column("Time_Stamp")]
        [NotMapped]
        public byte[] TimeStamp { set; get; }
        public virtual CompanyJobPoco CompanyJob { set; get; }


    }
}
