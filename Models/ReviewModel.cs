using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApkCenterAdminApi.Models
{
    public interface IReviewModel
    {
        double Star { get; set; }
    }

    public class ReviewModel
    {
        public double Star { get; set; } = 0;
        public double Amount { get; set; } = 0;
    }
}
