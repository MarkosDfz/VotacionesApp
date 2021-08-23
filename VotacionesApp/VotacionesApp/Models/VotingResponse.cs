using System;
using System.Collections.Generic;

namespace VotacionesApp.Models
{
    public class VotingResponse
    {
        public int VotingId { get; set; }

        public string Description { get; set; }

        public string Remarks { get; set; }

        public DateTime DateTimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public bool IsForAllUsers { get; set; }

        public int QuantityVotes { get; set; }

        public UserResponse Winner { get; set; }

        public State State { get; set; }

        public List<Candidate> Candidates { get; set; }
    }
}