using System;
namespace VotacionesApp.Models
{
    public class Candidate
    {
        public int VotingId { get; set; }

        public int CandidateId { get; set; }

        public int QuantityVotes { get; set; }

        public UserResponse User { get; set; }
    }
}
