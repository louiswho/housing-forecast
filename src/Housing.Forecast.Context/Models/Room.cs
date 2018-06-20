using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Newtonsoft.Json;

namespace Housing.Forecast.Context.Models
{
    public class Room
    {
        /// <summary>Default Constructor</summary>>
        /// <remarks>Sets all properties to empty, null, or impossible values that correspond 
        /// to invalid models that should be invalid if not replaced.</remarks>
        public Room()
        {
            Id = Guid.Empty;
            RoomId = Guid.Empty;
            Location = null;
            Vacancy = -1;
            Occupancy = -1;
            Gender = null;
            Address = null;
        }

        /// <summary>Property validation</summary>>
        /// <remarks>Returns true if all properties are valid
        /// By default (through constructor), the model is invalid, properties to be filled in</remarks>
        public bool Validate()
        {
            if (Id == Guid.Empty) { return false; }
            if (RoomId == Guid.Empty) { return false; }
            if (String.IsNullOrEmpty(Location)) { return false; }
            if (Vacancy == -1) { return false; }
            if (Occupancy == -1) { return false; }
            if (String.IsNullOrEmpty(Gender)) { return false; }
            if (Address == null) { return false; }

            return true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public string Location { get; set; }

        public int Vacancy { get; set; }

        public int Occupancy { get; set; }

        public string Gender { get; set; }

        public Address Address { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Deleted { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }
    }
}
