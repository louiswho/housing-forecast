using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Newtonsoft.Json;

namespace Housing.Forecast.Context.Models
{
    public class Batch
    {
        /// <summary>Default Constructor</summary>>
        /// <remarks>Sets all properties to empty, null, or impossible values that correspond 
        /// to invalid models that should be invalid if not replaced.</remarks>
        public Batch()
        {
            Id = Guid.Empty;
            BatchId = Guid.Empty;
            BatchName = "";
            BatchOccupancy = -1;
            BatchSkill = "";
            State = "";
        }

        /// <summary>Property validation</summary>>
        /// <remarks>Returns true if all properties are valid
        /// By default (through constructor), the model is invalid, properties to be filled in</remarks>
        public bool Validate()
        {
            if (Id == Guid.Empty) { return false; }
            if (BatchId == Guid.Empty) { return false; }
            if (String.IsNullOrEmpty(BatchName)) { return false; }
            if (BatchOccupancy < 0 || BatchOccupancy > 100) { return false; }
            if (String.IsNullOrEmpty(BatchSkill)) { return false; }
            if (String.IsNullOrEmpty(State)) { return false; }

            return true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid BatchId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }

        public string BatchName { get; set; }

        public int BatchOccupancy { get; set; }

        public string BatchSkill { get; set; }

        public string State { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Deleted { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }

    }
}
