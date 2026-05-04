using CTHelper.Domain.Common.Enums;
using CTHelper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTHelper.Application.Models.TestModels
{
    public class TestPassingResponseModel
    {
        public long TestId { get; set; }
        public string TestName { get; set; } = default!;
        public long AttemptId { get; set; }
        public TestAttemptStatusTypeEnum Status { get; set; }
        public int Duration { get; set; }
        public short? RawScore { get; set; }
        public IEnumerable<TestPassingProblemModel> Problems { get; set; } = new List<TestPassingProblemModel>();
    }
    public class TestPassingProblemModel
    {
        public string? UserAnswer { get; set; }
        public long UserAnswerId { get; set; } 
        public string Code { get; set; } = default!;
        public ProblemTypeEnum Type { get; set; }
        public string Statement { get; set; } = default!;
    }
}
