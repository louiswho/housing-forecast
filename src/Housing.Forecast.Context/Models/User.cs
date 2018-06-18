using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Housing.Forecast.Context.Models
{
    ///<summary>The User model is used to contain all of the pertinent information about a user including name, location, room, address, email, gender, employee type, and batch they belong to. </summary>
    ///<remarks>    
    ///Each User object will have its own uniquely generated Guid Id and retain the primary key Guid that was generated for it in the previous database into UserId.
    ///</remarks>
    public class User
    {
        /// <summary>Default Constructor</summary>>
        /// <remarks>Sets all properties to empty, null, or impossible values that correspond 
        /// to invalid models that should be invalid if not replaced.</remarks>
        public User()
        {
            Id = Guid.Empty;
            UserId = Guid.Empty;
            Name = null;
            Room = null;
            Address = null;
            Location = null;
            Email = null;
            Gender = null;
            Type = null;
            Batch = null;
        }

        /// <summary>Property validation</summary>>
        /// <remarks>Returns true if all properties are valid
        /// By default (through constructor), the model is invalid, properties to be filled in</remarks>
        public bool Validate()
        {
            if (Id == Guid.Empty) { return false; }
            if (UserId == Guid.Empty) { return false; }
            if (Name == null) { return false; }
            if (String.IsNullOrEmpty(Location)) { return false; }
            if (String.IsNullOrEmpty(Email)) { return false; }
            if (String.IsNullOrEmpty(Gender)) { return false; }
            if (String.IsNullOrEmpty(Type)) { return false; }
            if (Batch == null) { return false; }

            return true;
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Name Name { get; set; }

        public string Location { get; set; }

        public Room Room { get; set; }
        public Address Address { get; set; }

        public string Email { get; set; }

        public string Gender { get; set; }

        public string Type { get; set; }

        public Batch Batch { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? Deleted { get; set; }
    }
        
}
