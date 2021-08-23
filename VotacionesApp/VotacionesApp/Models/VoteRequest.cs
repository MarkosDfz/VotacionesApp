using System;
namespace VotacionesApp.Models
{
    public class VoteRequest
    {
        public int VotingId { get; set; }

        public int UserId { get; set; }

        public int CandidateId { get; set; }
    }
}
