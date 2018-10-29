using System.Collections.Generic;

namespace Api.Controllers
{
    public class PutModel
    {
        public string Expression { get; set; }
        public List<PutValueModel> Values { get; set; }
    }
}
