using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Newtonsoft.Json;

namespace Housing.Forecast.Context.Models
{
    ///<summary>The Name Class contains standard information regarding a User's name including first name, middle name, and last name.</summary>
    ///<remarks>
    ///Each Name will have a uniquely generated Guid Id as well as retain the primary key Guid of the previous database, which is stored in NameId.
    ///Each Name object will have a collection of Users in the case that multiple users share the same name.
    ///</remarks>
    public class Name
    {
        /// <summary>Default Constructor</summary>>
        /// <remarks>Sets all properties to empty, null, or impossible values that correspond 
        /// to invalid models that should be invalid if not replaced.</remarks>
        public Name()
        {
            Id = Guid.Empty;
            NameId = Guid.Empty;
            First = null;
            Last = null;
        }

        /// <summary>Property validation</summary>>
        /// <remarks>Returns true if all properties are valid
        /// By default (through constructor), the model is invalid, properties to be filled in</remarks>
        public bool Validate()
        {
            if (Id == Guid.Empty) { return false; }
            if (NameId == Guid.Empty) { return false; }
            if (String.IsNullOrEmpty(First)) { return false; }
            if (String.IsNullOrEmpty(Last)) { return false; }

            return true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid NameId { get; set; }

        public string First { get; set; }

        public string Middle { get; set; }

        public string Last { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
