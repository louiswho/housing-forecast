using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

using Newtonsoft.Json;

namespace Housing.Forecast.Context.Models
{
    ///<summary>The Address class contains standard address information that will be used for Users, Batches, and Rooms</summary>
    ///<remarks>
    ///Each Address Object will have its own uniquely generated Guid Id and retain the primary key Guid that was generated for it in the previous database into AddressId.
    ///Each Address Object will have a collection of Users, Objects, and Batches that share the Address.
    ///</remarks>
    public class Address
    {
        /// <summary>Default Constructor</summary>>
        /// <remarks>Sets all properties to empty, null, or impossible values that correspond 
        /// to invalid models that should be invalid if not replaced.</remarks>
        public Address()
        {
            Id = Guid.Empty;
            AddressId = Guid.Empty;
            Address1 = "";
            City = "";
            State = "";
            PostalCode = "";
            Country = "";
        }

        /// <summary>Property validation</summary>>
        /// <remarks>Returns true if all properties are valid
        /// By default (through constructor), the model is invalid, properties to be filled in</remarks>
        public bool Validate()
        {
            if (Id == Guid.Empty) { return false; }
            if (AddressId == Guid.Empty) { return false; }
            if (String.IsNullOrEmpty(Address1)) { return false; }
            if (String.IsNullOrEmpty(City)) { return false; }
            if (String.IsNullOrEmpty(State)) { return false; }
            if (String.IsNullOrEmpty(PostalCode)) { return false; }
            if (String.IsNullOrEmpty(Country)) { return false; }
            return true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid AddressId { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public DateTime Created { get; set; }

        [JsonIgnore]
        public ICollection<User> Users { get; set; }

        [JsonIgnore]
        public ICollection<Room> Rooms { get; set; }
    }
}
