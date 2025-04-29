using System.Collections.Generic;

namespace ClinicManagement.Core.Models
{
   

    public class PredictResponse
    {
        public string Explanation { get; set; }
        public int Prediction { get; set; }
        public List<string> Recommendations { get; set; }
    }


}


