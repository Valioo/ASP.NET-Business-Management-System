using aspnet_core_mvc_crud.Enums;
using System;

namespace aspnet_core_mvc_crud.Models
{
    public class ServiceRequest
    {
        public ServiceRequest()
        {

        }
        public ServiceRequest(int id, string name, string description, string address, byte[] picture, Status status, DateTime? dateOfVisit, string assignedTechnicianId, string creatorId)
        {
            Id = id;
            Name = name;
            Description = description;
            Address = address;
            Picture = picture;
            Status = status;
            DateOfVisit = dateOfVisit;
            AssignedTechnicianId = assignedTechnicianId;
            CreatorId = creatorId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public byte[]? Picture { get; set; }
        public Status Status { get; set; } = Status.Awaiting;
        public DateTime? DateOfVisit { get; set; }
        public string AssignedTechnicianId { get; set; }
        public string CreatorId { get; set; }
    }
}
